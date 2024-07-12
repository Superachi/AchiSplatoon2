using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AchiSplatoon2.Content.Projectiles
{
    enum ProjNetUpdateType : byte
    {
        None,
        Initialize,
        EveryFrame,
        DustExplosion,
        UpdateCharge,
        ReleaseCharge,
        ShootAnimation,
        SyncMovement,
    }

    internal class BaseProjectile : ModProjectile
    {
        public int itemIdentifier = -1;
        private BaseWeapon weaponSource;
        public BaseWeapon WeaponInstance
        {
            get
            {
                return weaponSource;
            }
            set
            {
                weaponSource = value;
            }
        }

        public bool dataReady = false;
        protected virtual bool FallThroughPlatforms => true;

        public int parentIdentity = -1;

        // If dissolvable = true, it means this projectile gets destroyed by liquids by default
        // An exception is made for shimmer
        protected bool dissolvable = true;

        // Audio
        protected string shootSample = "SplattershotShoot";
        protected string shootWeakSample = "SplattershotShoot";
        protected string shootAltSample = "SplattershotShoot";

        // Colors
        public InkColor primaryColor = InkColor.Order;
        public InkColor secondaryColor = InkColor.Order;
        private int primaryHighest = 0;
        private int secondaryHighest = 0;

        // Modifiers
        // See <InkWeaponPlayer.cs>
        protected float chargeSpeedModifier = 1f;
        protected float explosionRadiusModifier = 1f;
        protected int armorPierceModifier = 0;
        protected int piercingModifier = 0;
        protected int originalDamage = 0;
        protected float damageModifierAfterPierce = 0.7f;
        protected bool enablePierceDamagefalloff;

        protected virtual bool CountDamageForSpecialCharge { get => true; }
        protected bool wormDamageReduction = false;

        // State machine
        protected int state = 0;

        // Netcode
        protected byte netUpdateType = (byte)ProjNetUpdateType.None;
        protected int afterInitializeDelay = 1;

        /// <summary>
        /// Used to declare a projectile destroyed locally, without invoking the Projectile.Kill() method
        /// Example use-case: makes slosher projectiles go 'inactive' on the owner client, so that other clients have time to show the projectile hitting the target/ground
        /// </summary>
        protected bool isFakeDestroyed = false;

        /// <summary>
        /// Declare a projectile destroyed locally. This sets its damage to 0, and sets Projectile.friendly to false
        /// </summary>
        protected virtual void FakeDestroy()
        {
            isFakeDestroyed = true;
            Projectile.damage = 0;
            Projectile.friendly = false;
        }

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

        public virtual void ApplyWeaponInstanceData()
        {
            // Cast the provided WeaponInstance to the correct child of BaseWeapon
            // Then use it to set the projectile's properties
        }

        public virtual void AfterSpawn()
        {
            // Do something after spawning
            // Using this, rather than OnSpawn, provides time to set properties of the projectile after its spawned into the world
        }

        public virtual void AfterInitialize()
        {
            NetUpdate(ProjNetUpdateType.Initialize, true);
        }

        public bool Initialize(bool ignoreAimDeviation = false, bool isDissolvable = true)
        {
            if (WeaponInstance == null)
            {
                // Attempt to get the source via the itemIdentifier
                if (itemIdentifier != -1)
                {
                    // DebugHelper.PrintWarning($"itemId => {itemIdentifier}");
                    ModItem modItem = ModContent.GetModItem(itemIdentifier);
                    WeaponInstance = (BaseWeapon)modItem;
                }

                if (WeaponInstance == null)
                {
                    PrintStackTrace(3);
                    DebugHelper.PrintWarning($"Data for this projectile is not ready yet! (weaponSource: {WeaponInstance}, itemIdentifier: {itemIdentifier})");
                    Projectile.Kill();
                    return false;
                }
            }

            // Check the highest color chip amounts, set the ink color to match the top 2
            // In BaseWeapon.cs -> Shoot(), we create an instance of said weapon class and store the object inside the ModPlayer
            // This object is then referenced by child classes to get alter certain mechanics
            if (IsThisClientTheProjectileOwner())
            {
                var owner = Main.player[Projectile.owner];
                var modPlayer = owner.GetModPlayer<InkWeaponPlayer>();

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

                dissolvable = isDissolvable;
                if (!ignoreAimDeviation && WeaponInstance.AimDeviation != 0)
                {
                    var vel = Projectile.velocity;
                    var projSpeed = Vector2.Distance(Main.LocalPlayer.Center, Main.LocalPlayer.Center + vel);

                    var dev = WeaponInstance.AimDeviation;
                    float startRad = vel.ToRotation();
                    float startDeg = MathHelper.ToDegrees(startRad);
                    float endDeg = startDeg + Main.rand.NextFloat(-dev, dev);
                    float endRad = MathHelper.ToRadians(endDeg);
                    Vector2 angleVec = endRad.ToRotationVector2();
                    Projectile.velocity = angleVec * projSpeed;
                }

                if (Projectile.penetrate != 1)
                {
                    enablePierceDamagefalloff = true;
                    wormDamageReduction = true;
                }

                originalDamage = Projectile.damage;

                modPlayer.UpdateInkColor(GenerateInkColor());
            }

            return true;
        }

        protected virtual BaseProjectile CreateChildProjectile(Vector2 position, Vector2 velocity, int type, int damage, bool triggerAfterSpawn = true)
        {
            var p = Projectile.NewProjectileDirect(
                spawnSource: Projectile.GetSource_FromThis(),
                position: position,
                velocity: velocity,
                type: type,
                damage: damage,
                knockback: Projectile.knockBack,
                owner: Projectile.owner);

            var proj = p.ModProjectile as BaseProjectile;
            proj.WeaponInstance = WeaponInstance;
            proj.itemIdentifier = itemIdentifier;
            proj.parentIdentity = Projectile.identity;
            proj.primaryColor = primaryColor;
            proj.secondaryColor = secondaryColor;
            if (triggerAfterSpawn) proj.AfterSpawn();
            return proj;
        }

        protected Projectile? GetParentProjectile(int projectileId)
        {
            if (parentIdentity < Main.projectile.Length)
            {
                return Main.projectile[parentIdentity];
            }
            return null;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = FallThroughPlatforms;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        private bool isTargetWorm(NPC target)
        {
            bool isWorm = false;

            int n = target.type;
            if ((n >= NPCID.EaterofWorldsHead && n <= NPCID.EaterofWorldsTail)
            || (n >= NPCID.TheDestroyer && n <= NPCID.TheDestroyerTail)
            || (n >= NPCID.GiantWormHead && n <= NPCID.GiantWormTail)
            || (n >= NPCID.DiggerHead && n <= NPCID.DiggerTail)
            || (n >= NPCID.DevourerHead && n <= NPCID.DevourerTail)
            || (n >= NPCID.SeekerHead && n <= NPCID.SeekerTail)
            || (n >= NPCID.TombCrawlerHead && n <= NPCID.TombCrawlerTail)
            || (n >= NPCID.DuneSplicerHead && n <= NPCID.DuneSplicerTail)
            || (n >= NPCID.WyvernHead && n <= NPCID.WyvernTail)
            || (n >= NPCID.BoneSerpentHead && n <= NPCID.BoneSerpentTail))
            {
                isWorm = true;
            }

            return isWorm;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (wormDamageReduction && Main.expertMode && isTargetWorm(target))
            {
                modifiers.FinalDamage *= 0.3f;
            }

            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type != NPCID.TargetDummy)
            {
                DamageToSpecialCharge(damageDone, target.lifeMax);
            }

            if (enablePierceDamagefalloff && !isTargetWorm(target))
            {
                Projectile.damage = MultiplyProjectileDamage(damageModifierAfterPierce);
            }
        }

        protected Player GetOwner()
        {
            return Main.player[Projectile.owner];
        }

        protected int MultiplyProjectileDamage(float multiplier)
        {
            return (int)(Projectile.damage * multiplier);
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

            if (primaryHighest == 0 && secondaryHighest == 0 && NetHelper.IsThisAClient())
            {
                // Failsave for if the bullet color data is missing during online play
                var owner = Main.player[Projectile.owner];
                var modPlayer = owner.GetModPlayer<InkWeaponPlayer>();
                return modPlayer.ColorFromChips;
            }

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

        #region Item animation
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

        protected void PlayerItemAnimationFaceCursor(Player owner, Vector2? offset = null, float? radiansOverride = null)
        {
            // Change player direction depending on what direction the charger is held when charging
            float mouseDirRadians;
            if (radiansOverride == null)
            {
                mouseDirRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
            }
            else
            {
                mouseDirRadians = (float)radiansOverride;
            }

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
        #endregion

        #region Audio
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
        #endregion

        protected void debugMessage(bool isDebug, string message)
        {
            if (isDebug) Main.NewText(message);
        }

        #region DustEffects
        protected void EmitBurstDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, float radiusModifier = 100f)
        {
            float radiusMult = radiusModifier / 140;
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
        protected void EmitBurstDust(ExplosionDustModel dustModel)
        {
            EmitBurstDust(dustModel.dustMaxVelocity, dustModel.dustAmount, dustModel.minScale, dustModel.maxScale, dustModel.radiusModifier);
        }

        protected void CreateExplosionVisual(ExplosionDustModel expModel, PlayAudioModel? audioModel = null)
        {
            if (IsThisClientTheProjectileOwner())
            {
                if (expModel == null) return;

                var p = CreateChildProjectile(
                    position: Projectile.Center,
                    velocity: Vector2.Zero,
                    type: ModContent.ProjectileType<ExplosionProjectileVisual>(),
                    damage: 0);
                var proj = p as ExplosionProjectileVisual;

                proj.explosionDustModel = expModel;
                proj.playAudioModel = audioModel;
            }
        }

        protected void VisualizeRadius()
        {
            if (!IsThisClientTheProjectileOwner()) return;
            for (int i = 0; i < 30; i++)
            {
                int id = Dust.NewDust(Projectile.Center - new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.width, Projectile.height, DustID.BlueFairy, 0, 0);
                Dust d = Main.dust[id];
                d.velocity = Vector2.Zero;
            }
        }

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

        // For use by nozzlenoses, stringers, etc. Anything that shoots a burst of shots.
        protected void TripleHitDustBurst(Vector2? position = null)
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

                PlayAudio("TripleHit", pitchVariance: 0.1f);

                var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
                Color inkColor = modPlayer.ColorFromChips;

                for (int i = 0; i < 10; i++)
                {
                    float hspeed = i * 1.5f;
                    float vspeed = i / 1.5f;
                    float scale = 2 - (i / 10) * 2;
                    spawnDust(Main.rand.NextVector2Circular(32, 32), Main.rand.NextFloat(1.5f, 3f), inkColor);
                }
            }
        }
        #endregion

        private void Dissolve()
        {
            if (dissolvable)
            {
                Tile tile = Framing.GetTileSafely(Projectile.Center);
                if (tile.LiquidType >= LiquidID.Water && tile.LiquidType < LiquidID.Shimmer && tile.LiquidAmount > 100)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreAI()
        {
            afterInitializeDelay--;
            if (afterInitializeDelay == 0)
            {
                AfterInitialize();
            }

            Dissolve();
            
            return true;
        }

        #region NetCode
        public virtual void NetUpdate(ProjNetUpdateType type, bool ownerOnly = false)
        {
            // By default, perform the netUpdate
            // If ownerOnly is true, then we check if this client owns the projectile first
            bool willUpdate = true;
            if (ownerOnly)
            {
                willUpdate = IsThisClientTheProjectileOwner();
            }
            if (!willUpdate) return;

            netUpdateType = (byte)type;
            Projectile.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (NetHelper.IsSinglePlayer()) return;

            writer.Write(netUpdateType);

            // Fallbacks for getting the associated item data
            if (itemIdentifier == -1)
            {
                if (WeaponInstance != null)
                {
                    itemIdentifier = WeaponInstance.ItemIdentifier;
                }
                else
                {
                    WeaponInstance = (BaseWeapon)Main.LocalPlayer.HeldItem.ModItem;
                    itemIdentifier = WeaponInstance.ItemIdentifier;
                }
            }

            // DebugHelper.PrintInfo($"Packet type: {(ProjNetUpdateType)netUpdateType} | Item ID: {(Int16)itemIdentifier}");
            writer.Write((Int16)itemIdentifier);

            switch (netUpdateType)
            {
                case (byte)ProjNetUpdateType.Initialize:
                    NetSendInitialize(writer);
                    break;
                case (byte)ProjNetUpdateType.EveryFrame:
                    NetSendEveryFrame(writer);
                    break;
                case (byte)ProjNetUpdateType.DustExplosion:
                    NetSendDustExplosion(writer);
                    break;
                case (byte)ProjNetUpdateType.ReleaseCharge:
                    NetSendReleaseCharge(writer);
                    break;
                case (byte)ProjNetUpdateType.UpdateCharge:
                    NetSendUpdateCharge(writer);
                    break;
                case (byte)ProjNetUpdateType.ShootAnimation:
                    NetSendShootAnimation(writer);
                    break;
                case (byte)ProjNetUpdateType.SyncMovement:
                    NetSendSyncMovement(writer);
                    break;
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (NetHelper.IsSinglePlayer()) return;

            netUpdateType = reader.ReadByte();
            itemIdentifier = reader.ReadInt16();
            // DebugHelper.PrintInfo($"Receiving packet type: {(ProjNetUpdateType)netUpdateType} | Item ID: {(Int16)itemIdentifier}");

            switch (netUpdateType)
            {
                case (byte)ProjNetUpdateType.Initialize:
                    NetReceiveInitialize(reader);
                    break;
                case (byte)ProjNetUpdateType.EveryFrame:
                    NetReceiveEveryFrame(reader);
                    break;
                case (byte)ProjNetUpdateType.DustExplosion:
                    NetReceiveDustExplosion(reader);
                    break;
                case (byte)ProjNetUpdateType.ReleaseCharge:
                    NetReceiveReleaseCharge(reader);
                    break;
                case (byte)ProjNetUpdateType.UpdateCharge:
                    NetReceiveUpdateCharge(reader);
                    break;
                case (byte)ProjNetUpdateType.ShootAnimation:
                    NetReceiveShootAnimation(reader);
                    break;
                case (byte)ProjNetUpdateType.SyncMovement:
                    NetReceiveSyncMovement(reader);
                    break;
            }
        }

        protected virtual void NetSendInitialize(BinaryWriter writer)
        {
        }

        protected virtual void NetReceiveInitialize(BinaryReader reader)
        {
            AfterSpawn();
        }

        private string NotImplementedWarning()
        {
            PrintStackTrace(3);
            return $"No implementation for this projectile packet type yet.";
        }

        protected virtual void NetSendSyncMovement(BinaryWriter writer)
        {
        }

        protected virtual void NetReceiveSyncMovement(BinaryReader reader)
        {
            DebugHelper.PrintWarning(NotImplementedWarning());
        }

        protected virtual void NetSendEveryFrame(BinaryWriter writer)
        {
            // Given that this method will be called very often, try not to make the packet size too large
        }

        protected virtual void NetReceiveEveryFrame(BinaryReader reader)
        {
            DebugHelper.PrintWarning(NotImplementedWarning());
        }

        protected virtual void NetSendDustExplosion(BinaryWriter writer)
        {
        }

        protected virtual void NetReceiveDustExplosion(BinaryReader reader)
        {
            DebugHelper.PrintWarning(NotImplementedWarning());
        }

        // Shoot animation (for cases where it doesn't manually show)
        protected virtual void NetSendShootAnimation(BinaryWriter writer)
        {
        }

        protected virtual void NetReceiveShootAnimation(BinaryReader reader)
        {
            DebugHelper.PrintWarning(NotImplementedWarning());
        }

        // Charge mechanics
        protected virtual void NetSendUpdateCharge(BinaryWriter writer)
        {
        }
        protected virtual void NetReceiveUpdateCharge(BinaryReader reader)
        {
            DebugHelper.PrintWarning(NotImplementedWarning());
        }

        protected virtual void NetSendReleaseCharge(BinaryWriter writer)
        {
        }

        protected virtual void NetReceiveReleaseCharge(BinaryReader reader)
        {
            DebugHelper.PrintWarning(NotImplementedWarning());
        }
        #endregion

        #region Debug
        public void PrintAndLog(string message, Color? color = null)
        {
            if (color == null) color = Color.White;

            Main.NewText(message, color);
            Mod.Logger.Info(message);
        }

        public void PrintStackTrace(int amount, Color? color = null)
        {
            if (color == null) color = Color.White;

            string currentClass = "";
            PrintAndLog($"{DateTime.Now.TimeOfDay} ==========", color);

            if (currentClass != this.GetType().Name)
            {
                currentClass = this.GetType().Name;
                PrintAndLog($"Class: {currentClass}", Color.Orange);
            }

            for (var i = 0; i < amount; i++)
            {
                PrintAndLog($"{i}>{(new System.Diagnostics.StackTrace()).GetFrame(i + 2).GetMethod().Name}", Color.Yellow);
            }
        }
        #endregion
    }
}
