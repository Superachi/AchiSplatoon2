using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Bson;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseProjectile : ModProjectile
    {
        protected BaseWeapon weaponSource;

        // Audio
        protected string shootSample = "SplattershotShoot";
        protected string shootWeakSample = "SplattershotShoot";

        // Colors
        private InkColor primaryColor = InkColor.Red;
        private InkColor secondaryColor = InkColor.Red;
        private int primaryHighest = 0;
        private int secondaryHighest = 0;

        // Modifiers
        // See <InkWeaponPlayer.cs>
        protected float chargeSpeedModifier = 1f;
        protected float explosionRadiusModifier = 1f;
        protected float attackSpeedModifier = 1f;
        protected int piercingModifier = 0;
        protected float damageModifierAfterPierce = 0.8f;
        protected virtual bool EnablePierceDamageFalloff { get => true; }

        protected virtual bool CountDamageForSpecialCharge { get => true; }

        public void Initialize()
        {
            // Check the highest color chip amounts, set the ink color to match the top 2
            if (IsThisClientTheProjectileOwner())
            {
                // In BaseWeapon.cs -> Shoot(), we create an instance of said weapon class and store the object inside the ModPlayer
                // This object is then referenced by child classes to get alter certain mechanics
                var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
                weaponSource = Main.LocalPlayer.GetModPlayer<ItemTrackerPlayer>().lastUsedWeapon;

                for (int i = 0; i < modPlayer.ColorChipAmounts.Length; i++)
                {
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

                    // Apply color chip buffs
                    // See also the calculations in InkWeaponPlayer.cs
                    if (!modPlayer.IsPaletteValid()) return;

                    // Red chips > faster attack speed (mainly for splatlings in this case)
                    if (i == (int)InkWeaponPlayer.ChipColor.Red)
                    {
                        attackSpeedModifier += modPlayer.CalculateAttackSpeedBonus();
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
                            Projectile.usesLocalNPCImmunity = true;
                            Projectile.localNPCHitCooldown = 20 * FrameSpeed();
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsThisClientTheProjectileOwner())
            {
                if (EnablePierceDamageFalloff)
                {
                    Projectile.damage = (int)(Projectile.damage * damageModifierAfterPierce);
                }

                DamageToSpecialCharge(damageDone);
            }
        }

        public void DamageToSpecialCharge(int damage)
        {
            if (!CountDamageForSpecialCharge) { return; }

            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            modPlayer.AddSpecialPoints(damage);
            CombatTextHelper.DisplayText($"{(int)modPlayer.SpecialPoints}", Main.LocalPlayer.Center);
        }

        protected int FrameSpeed()
        {
            return 1 + Projectile.extraUpdates;
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
                owner.itemRotation = MathHelper.ToRadians((mouseDirDegrees + 180) % 360);
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
    }
}
