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

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatChargerProjectile : BaseProjectile
    {
        private bool chargeReady = false;
        private bool hasFired = false;
        private int timeLeftAfterFiring = 120;

        protected virtual float RequiredChargeTime { get => 55f; }
        protected virtual SoundStyle ShootSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/SplatChargerShoot"); }
        protected virtual SoundStyle ShootWeakSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/SplatChargerShootWeak"); }
        protected virtual bool ShakeScreenOnChargeShot { get => true; }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Initialize(color: InkColor.Pink, visible: false, visibleDelay: 24f);

            Projectile.extraUpdates = 32;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 36000;
            Projectile.penetrate = 10;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Vector2.Zero;

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

        private void syncProjectilePosWithPlayer(Player owner)
        {
            Projectile.position = owner.Center;
        }

        private void syncProjectilePosWithWeaponBarrel(Vector2 position, Vector2 velocity, SplatCharger weaponData)
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
                // This weapon uses extra updates, so timers go extra fast!
                if (ChargeTime >= RequiredChargeTime * Projectile.extraUpdates)
                {
                    // Charge is ready!
                    if (!chargeReady)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                        }

                        chargeReady = true;

                        var readySample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/ChargeReady");
                        var readySound = readySample with
                        {
                            Volume = 0.4f,
                            MaxInstances = 1
                        };
                        SoundEngine.PlaySound(readySound);
                        cancelChargeSound();
                    } else
                    {
                        if (Main.rand.NextBool(400))
                        {
                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                        }
                    }
                }

                ++ChargeTime;
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
                // Main.NewText("Owner attacked.");
                // Release the attack
                hasFired = true;

                // Adjust behaviour depending on the charge amount
                SoundStyle shootSound;
                if (chargeReady)
                {
                    Projectile.timeLeft = timeLeftAfterFiring * Projectile.extraUpdates;
                    shootSound = ShootSample with
                    {
                        Volume = 0.4f,
                        PitchVariance = 0.1f,
                        MaxInstances = 1
                    };

                    if (ShakeScreenOnChargeShot)
                    {
                        PunchCameraModifier modifier = new PunchCameraModifier(
                            startPosition: owner.Center,
                            direction: (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(),
                            strength: 4f,
                            vibrationCyclesPerSecond: 8f,
                            frames: 10, 80f, FullName);
                        Main.instance.CameraModifiers.Add(modifier);
                    }
                }
                else
                {
                    Projectile.penetrate = 1;
                    int chargeTimeNormalized = Convert.ToInt32(ChargeTime / Projectile.extraUpdates);

                    // Deal a min. of 10% damage and a max. of 40% damage
                    Projectile.damage = Convert.ToInt32(
                        (Projectile.damage * 0.1) + ((Projectile.damage * 0.3) * chargeTimeNormalized / RequiredChargeTime)
                    );

                    // Similar for the range (min. 3% range, max. 20%)
                    Projectile.timeLeft = Convert.ToInt32(
                        (timeLeftAfterFiring * 0.03) + ((timeLeftAfterFiring * 0.17) * chargeTimeNormalized / RequiredChargeTime)
                    ) * Projectile.extraUpdates;

                    shootSound = ShootWeakSample with
                    {
                        Volume = 0.2f,
                        PitchVariance = 0.1f,
                        MaxInstances = 1
                    };
                }

                Projectile.velocity = owner.DirectionTo(Main.MouseWorld) * 3f;
                syncProjectilePosWithWeaponBarrel(Projectile.position, Projectile.velocity, new SplatCharger());

                Projectile.tileCollide = true;
                Projectile.friendly = true;
                SoundEngine.PlaySound(shootSound);
                cancelChargeSound();
                return;
            }

            Color dustColor = ColorHelper.GenerateInkColor(inkColor);
            var randomDustVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: randomDustVelocity, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, System.Int32 damageDone)
        {
            target.AddBuff(103, 180);
        }

        public override void OnKill(int timeLeft)
        {
            if (!hasFired) return;

            for (int i = 0; i < 15; i++)
            {
                float random = Main.rand.NextFloat(-5, 5);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: ColorHelper.GenerateInkColor(inkColor), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
        }
    }
}