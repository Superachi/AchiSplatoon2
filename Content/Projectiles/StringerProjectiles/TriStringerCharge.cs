using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using rail;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons;
using Mono.Cecil;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using System.Linq;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerCharge : BaseProjectile
    {
        private bool hasFired = false;
        private int timeLeftAfterFiring = 120;

        protected virtual SoundStyle ShootSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/TriStringerShoot"); }
        protected virtual SoundStyle ShootWeakSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BambooChargerShootWeak"); }
        protected virtual float[] ChargeTimeThresholds { get => [36f, 72f]; }
        private int chargeLevel = 0;
        private float maxChargeTime;
        protected virtual float ShotgunArc { get => 4f; }
        protected virtual int ProjectileCount { get => 3; }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            maxChargeTime = ChargeTimeThresholds.Last();
            Projectile.timeLeft = 36000;
        }

        private void startChargeSound()
        {
            var chargeSample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/ChargeStart");
            var chargeSound = chargeSample with
            {
                Volume = 0.2f,
                PitchVariance = 0.1f,
                MaxInstances = 1
            };
            SoundEngine.PlaySound(chargeSound);
        }

        private void cancelChargeSound()
        {
            // TODO: fix this scuffed way of cancelling the charge sound
            var chargeSample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/ChargeStart");
            var chargeSound = chargeSample with
            {
                Volume = 0.0f,
                MaxInstances = 1
            };
            SoundEngine.PlaySound(chargeSound);
        }

        private void playShootSound(SoundStyle sample)
        {
            var chargeSound = sample with
            {
                Volume = 0.3f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(chargeSound);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Vector2.Zero;
            startChargeSound();
        }

        private void syncProjectilePosWithPlayer(Player owner)
        {
            Projectile.position = owner.Center;
        }

        private void syncProjectilePosWithWeaponBarrel(Vector2 position, Vector2 velocity, TriStringer weaponData)
        {
            Vector2 weaponOffset = weaponData.HoldoutOffset() ?? new Vector2(0, 0);
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * weaponData.MuzzleOffsetPx;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.channel && !hasFired)
            {
                var len = ChargeTimeThresholds.Length;
                if (chargeLevel < len)
                {
                    ++ChargeTime;
                    if (ChargeTime >= ChargeTimeThresholds[chargeLevel])
                    {
                        chargeLevel++;

                        for (int i = 0; i < 10; i++)
                        {
                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                        }

                        var readySample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/ChargeReady");
                        var readySound = readySample with
                        {
                            Pitch = (chargeLevel - 1) * 0.2f,
                            Volume = 0.4f,
                            MaxInstances = 1
                        };
                        SoundEngine.PlaySound(readySound);

                        if (chargeLevel == len)
                        {
                            cancelChargeSound();
                        }
                    }
                } else
                {
                    if (Main.rand.NextBool(50))
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                    }
                }

                syncProjectilePosWithPlayer(owner);

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

                owner.itemAnimation = owner.itemAnimationMax;
                owner.itemTime = owner.itemTimeMax;
                return;
            }

            if (!hasFired)
            {
                hasFired = true;
                syncProjectilePosWithWeaponBarrel(Projectile.position, Projectile.velocity, new TriStringer());

                // Adjust damage and velocity based on charge level
                float velocityModifier = 1;
                int projectileType = ModContent.ProjectileType<TriStringerProjectileWeak>();
                if (chargeLevel == 0)
                {
                    Projectile.damage /= 3;
                    playShootSound(ShootWeakSample);
                } else {
                    if (chargeLevel == 1)
                    {
                        Projectile.damage /= 2;
                        velocityModifier = 1.25f;
                    } else
                    {
                        velocityModifier = 2f;
                    }
                    projectileType = ModContent.ProjectileType<TriStringerProjectile>();
                    playShootSound(ShootSample);
                }

                // The angle that the player aims at (player-to-cursor)
                float aimAngle = MathHelper.ToDegrees(
                    owner.DirectionTo(Main.MouseWorld).ToRotation()
                );

                if (ChargeTime == 0) return; // Prevent division by 0, though we shouldn't end up with this anyway

                float finalArc = Math.Clamp(ShotgunArc * (maxChargeTime / ChargeTime), ShotgunArc, ShotgunArc * 10);
                float degreesPerProjectile = finalArc / ProjectileCount;
                int middleProjectile = ProjectileCount / 2;
                float degreesOffset = -(middleProjectile * degreesPerProjectile);

                for (int i = 0; i < ProjectileCount; i++)
                {
                    // Convert angle: degrees -> radians -> vector
                    float degrees = aimAngle + degreesOffset;
                    float radians = MathHelper.ToRadians(degrees);
                    Vector2 angleVector = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
                    Vector2 velocity = angleVector * velocityModifier * (0.95f + i * 0.05f);

                    // Spawn projectile
                    int proj = Projectile.NewProjectile(
                        spawnSource: Projectile.GetSource_FromThis(),
                        position: Projectile.position,
                        velocity: velocity,
                        Type: projectileType,
                        Damage: Projectile.damage,
                        KnockBack: Projectile.knockBack,
                        Owner: Main.myPlayer);

                    // Set a number in the arrow
                    // Can be used to make them explode in sequence
                    Main.projectile[proj].ai[2] = i;

                    // Adjust the angle for the next projectile
                    degreesOffset += degreesPerProjectile;
                }

                for (int i = 0; i < 15; i++)
                {
                    Color dustColor = ColorHelper.GenerateInkColor(inkColor);

                    float random = Main.rand.NextFloat(-5, 5);
                    float velX = ((Projectile.velocity.X + random) * 0.5f);
                    float velY = ((Projectile.velocity.Y + random) * 0.5f);

                    Dust.NewDust(Projectile.position, 1, 1, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                }

                cancelChargeSound();
                Projectile.Kill();
                return;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (!hasFired) return;
        }
    }
}