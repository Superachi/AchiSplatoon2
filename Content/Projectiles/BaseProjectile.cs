using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Bson;
using ReLogic.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    enum ProjNetUpdateType : byte
    {
        Initialize,
        SyncMovement,
    }

    internal class BaseProjectile : ModProjectile
    {
        protected BaseWeapon weaponSource;
        protected virtual bool FallThroughPlatforms => true;

        // Audio
        protected string shootSample = "SplattershotShoot";
        protected string shootWeakSample = "SplattershotShoot";
        protected string shootAltSample = "SplattershotShoot";

        // Colors
        private InkColor primaryColor = InkColor.Order;
        private InkColor secondaryColor = InkColor.Order;
        private int primaryHighest = 0;
        private int secondaryHighest = 0;

        // Modifiers
        // See <InkWeaponPlayer.cs>
        protected float chargeSpeedModifier = 1f;
        protected float explosionRadiusModifier = 1f;
        protected int armorPierceModifier = 0;
        protected int piercingModifier = 0;
        protected float damageModifierAfterPierce = 0.8f;
        protected virtual bool EnablePierceDamageFalloff { get => true; }
        protected virtual bool CountDamageForSpecialCharge { get => true; }

        // State machine
        protected int state = 0;

        protected virtual void SetState(int targetState)
        {
            // In this method, you can do something different per changed state
            state = targetState;
            Projectile.netUpdate = true;
        }

        protected virtual void AdvanceState()
        {
            state++;
            SetState(state);
        }

        public void Initialize(bool ignoreAimDeviation = false)
        {
            // Check the highest color chip amounts, set the ink color to match the top 2
            // In BaseWeapon.cs -> Shoot(), we create an instance of said weapon class and store the object inside the ModPlayer
            // This object is then referenced by child classes to get alter certain mechanics
            var owner = Main.player[Projectile.owner];
            var modPlayer = owner.GetModPlayer<InkWeaponPlayer>();
            weaponSource = owner.GetModPlayer<ItemTrackerPlayer>().lastUsedWeapon;

            if (modPlayer.IsPaletteValid())
            {
                for (int i = 0; i < modPlayer.ColorChipAmounts.Length; i++)
                {
                    Projectile.usesLocalNPCImmunity = true;
                    Projectile.localNPCHitCooldown = 20 * FrameSpeed();

                    // Apply color chip buffs
                    // See also the calculations in InkWeaponPlayer.cs
                    int value = modPlayer.ColorChipAmounts[i];

                    // Only consider the color if we have any chips for it
                    if (value > 0)
                    {
                        // Change the primary color if we see a new highest count
                        if (value > primaryHighest)
                        {
                            // If we've no other colors, make the secondary color match the primary one
                            if (secondaryHighest == 0)
                            {
                                secondaryColor = (InkColor)i;
                                secondaryHighest = value;
                            }
                            // If we do, mark the previous primary color as the secondary color
                            else
                            {
                                secondaryColor = primaryColor;
                                secondaryHighest = primaryHighest;
                            }

                            primaryColor = (InkColor)i;
                            primaryHighest = value;
                        }
                        // What if we don't have the highest count?
                        else if (primaryColor == secondaryColor || value > secondaryHighest)
                        {
                            secondaryColor = (InkColor)i;
                            secondaryHighest = value;
                        }
                    }

                    // Red chips > more attack damage (configured in ChipPalette.cs) + armor piercing
                    if (i == (int)InkWeaponPlayer.ChipColor.Red)
                    {
                        armorPierceModifier += modPlayer.CalculateArmorPierceBonus();
                        Projectile.ArmorPenetration += armorPierceModifier;
                    }

                    // Purple chips > faster charge speed
                    if (i == (int)InkWeaponPlayer.ChipColor.Purple)
                    {
                        chargeSpeedModifier += modPlayer.CalculateChargeSpeedBonus();
                    }

                    // Yellow chips > bigger explosions + projectile piercing
                    if (i == (int)InkWeaponPlayer.ChipColor.Yellow)
                    {
                        explosionRadiusModifier += modPlayer.CalculateExplosionRadiusBonus();
                        piercingModifier += modPlayer.CalculatePiercingBonus();

                        if (Projectile.penetrate != -1)
                        {
                            Projectile.maxPenetrate += piercingModifier;
                            Projectile.penetrate += piercingModifier;
                        }
                    }
                }
            }

            if (!ignoreAimDeviation && weaponSource.AimDeviation != 0)
            {
                var vel = Projectile.velocity;
                var projSpeed = Vector2.Distance(Main.LocalPlayer.Center, Main.LocalPlayer.Center + vel);

                var dev = weaponSource.AimDeviation;
                float startRad = vel.ToRotation();
                float startDeg = MathHelper.ToDegrees(startRad);
                float endDeg = startDeg + Main.rand.NextFloat(-dev, dev);
                float endRad = MathHelper.ToRadians(endDeg);
                Vector2 angleVec = endRad.ToRotationVector2();
                Projectile.velocity = angleVec * projSpeed;
            }

            Projectile.netUpdate = true;
            if (NetHelper.IsPlayerSameAsLocalPlayer(owner))
            {
                modPlayer.UpdateInkColor(GenerateInkColor());
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)primaryColor);
            writer.Write((byte)secondaryColor);
            writer.Write((byte)Projectile.extraUpdates);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            primaryColor = (InkColor)reader.ReadByte();
            secondaryColor = (InkColor)reader.ReadByte();
            Projectile.extraUpdates = reader.ReadByte();
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = FallThroughPlatforms;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsThisClientTheProjectileOwner())
            {
                if (EnablePierceDamageFalloff)
                {
                    Projectile.damage = (int)(Projectile.damage * damageModifierAfterPierce);
                }

                if (target.type != NPCID.TargetDummy)
                {
                    DamageToSpecialCharge(damageDone, target.lifeMax);
                }
            }
        }

        public void DamageToSpecialCharge(float damage, float targetMaxLife)
        {
            if (!CountDamageForSpecialCharge) { return; }
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

            // The increment is a % of the target's max life, up to 5%
            float increment = Math.Min(damage * 2 / targetMaxLife, targetMaxLife / 20);
            modPlayer.AddSpecialPointsForDamage(increment);
        }

        protected int FrameSpeed(int frames = 1)
        {
            return frames + Projectile.extraUpdates;
        }

        protected float FrameSpeedDivide(float input)
        {
            return input / (1 + Projectile.extraUpdates);
        }

        public Color GenerateInkColor()
        {
            // If there are two color chips being considered, add a bias towards the color that we have more chips of
            var amount = 0.5f;
            if (primaryHighest != secondaryHighest) { amount = 0.35f; }
            return ColorHelper.LerpBetweenInkColors(primaryColor, secondaryColor, amount);
        }

        /// <summary>
        /// This check makes sure that the current player (aka client) is the owner of the projectile
        /// This is used for example to make sure not all clients/server spawns ammo when the projectile is destroyed
        /// See OnKill method here: http://docs.tmodloader.net/docs/stable/class_mod_projectile.html
        /// </summary>
        /// <returns></returns>
        public bool IsThisClientTheProjectileOwner()
        {
            return Main.myPlayer == Projectile.owner;
        }

        protected void SyncProjectilePosWithPlayer(Player owner, float offsetX = 0, float offsetY = 0)
        {
            Projectile.position = owner.Center + new Vector2(offsetX, offsetY);
        }

        protected void SyncProjectilePosWithWeaponBarrel(Vector2 position, Vector2 velocity, BaseWeapon weaponData)
        {
            Vector2 weaponOffset = weaponData.HoldoutOffset() ?? new Vector2(0, 0);
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * weaponData.MuzzleOffsetPx;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }
        }

        protected void PlayerItemAnimationFaceCursor(Player owner, Vector2? offset)
        {
            // Change player direction depending on what direction the charger is held when charging
            var mouseDirRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
            var mouseDirDegrees = MathHelper.ToDegrees(mouseDirRadians);
            
            if (mouseDirDegrees >= -90 && mouseDirDegrees <= 90)
            {
                owner.direction = 1;
                owner.itemRotation = mouseDirRadians;
            }
            else
            {
                owner.direction = -1;
                var sign = Math.Sign(mouseDirDegrees);
                if (sign > 0)
                {
                    owner.itemRotation = MathHelper.ToRadians((mouseDirDegrees - 180));
                }
                else
                {
                    owner.itemRotation = MathHelper.ToRadians(mouseDirDegrees + 180);
                }
            }

            if (offset != null)
            {
                owner.itemLocation += (Vector2)offset;
            }
            owner.itemAnimation = owner.itemAnimationMax;
            owner.itemTime = owner.itemTimeMax;
        }

        private SlotId PlaySoundFinal(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            if (position == null)
            {
                position = Projectile.position;
            }

            var sound = soundStyle with
            {
                Volume = volume,
                PitchVariance = pitchVariance,
                MaxInstances = maxInstances,
                Pitch = pitch,
            };

            return SoundEngine.PlaySound(sound, position);
        }

        protected SlotId PlayAudio(string soundPath, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            var style = new SoundStyle($"AchiSplatoon2/Content/Assets/Sounds/{soundPath}");
            return PlaySoundFinal(style, volume, pitchVariance, maxInstances, pitch, position);
        }

        protected SlotId PlayAudio(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
        {
            return PlaySoundFinal(soundStyle, volume, pitchVariance, maxInstances, pitch, position);
        }

        protected static void StopAudio(string soundPath)
        {
            var sample = new SoundStyle($"AchiSplatoon2/Content/Assets/Sounds/{soundPath}");
            var chargeSound = sample with
            {
                Volume = 0f,
                MaxInstances = 1,
            };
            SoundEngine.PlaySound(chargeSound);
        }

        protected void debugMessage(bool isDebug, string message)
        {
            if (isDebug) Main.NewText(message);
        }

        #region DustEffects
        protected void EmitBurstDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, float radiusModifier = 100f)
        {
            float radiusMult = radiusModifier / 160;
            amount = Convert.ToInt32(amount * radiusMult);

            // Ink
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = GenerateInkColor();
                var dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BlasterExplosionDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor, Main.rand.NextFloat(minScale, maxScale));
                dust.velocity *= radiusMult;
            }

            // Firework
            for (int i = 0; i < amount / 2; i++)
            {
                Color dustColor = GenerateInkColor();
                var dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB,
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor);
                dust.velocity *= radiusMult / 2;
            }
        }
        #endregion

        protected void DrawProjectile(Color inkColor, float rotation, float scale = 1f, bool considerWorldLight = true)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(Main.projFrames[Projectile.type], frameX: Projectile.frame); // The sourceRectangle says which frame to use.
            Vector2 origin = sourceRectangle.Size() / 2f;

            // The light value in the world
            var lightInWorld = Lighting.GetColor(Projectile.Center.ToTileCoordinates());

            var finalColor = new Color(inkColor.R, inkColor.G, inkColor.B);
            if (considerWorldLight)
            {
                finalColor = new Color(inkColor.R * lightInWorld.R / 255, inkColor.G * lightInWorld.G / 255, inkColor.B * lightInWorld.G / 255);
            }

            Main.EntitySpriteDraw(texture, position, sourceRectangle, finalColor, rotation, origin, scale, new SpriteEffects(), 0f);
        }

        protected void DirectHitDustBurst(Vector2? position = null)
        {
            if (position == null)
            {
                position = Projectile.Center;
            }

            if (IsThisClientTheProjectileOwner())
            {
                void spawnDust(Vector2 velocity, float scale, Color? newColor = null)
                {
                    Color color;
                    if (newColor == null)
                    {
                        color = new Color(255, 255, 255);
                    }
                    else
                    {
                        color = ColorHelper.LerpBetweenColors((Color)newColor, new Color(255, 255, 255), 0.8f);
                    }

                    var dust = Dust.NewDustPerfect((Vector2)position, 306,
                        velocity,
                        0, color, scale);
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.noLight = true;
                    dust.noLightEmittence = true;
                    dust.rotation = Main.rand.NextFloatDirection();
                }

                PlayAudio("DirectHit", pitchVariance: 0.1f);

                var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
                Color inkColor = modPlayer.ColorFromChips;

                for (int i = 0; i < 10; i++)
                {
                    float hspeed = i * 1.5f;
                    float vspeed = i / 1.5f;
                    float scale = 2 - (i / 10) * 2;
                    spawnDust(new Vector2(hspeed, 0), scale, inkColor);
                    spawnDust(new Vector2(-hspeed, 0), scale, inkColor);
                    spawnDust(new Vector2(0, vspeed), scale, inkColor);
                    spawnDust(new Vector2(0, -vspeed), scale, inkColor);
                    spawnDust(Main.rand.NextVector2Circular(32, 32), Main.rand.NextFloat(1.5f, 3f), inkColor);
                }
            }
        }
    }
}
