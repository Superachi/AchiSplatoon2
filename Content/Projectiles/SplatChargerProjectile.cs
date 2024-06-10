using AchiSplatoon2.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatChargerProjectile : ModProjectile
    {
        private bool chargeReady = false;
        private bool hasFired = false;
        private int dustTrailRadiusMult = 2;
        private int timeLeftAfterFiring = 120;

        protected virtual float RequiredChargeTime { get => 55f; }
        protected virtual SoundStyle ShootSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/SplatChargerShoot"); }
        protected virtual SoundStyle ShootWeakSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/SplatChargerShootWeak"); }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
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

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.channel && !hasFired)
            {
                if (ChargeTime == 0)
                {
                    // Main.NewText($"Owner started channeling. (Req. charge time = {RequiredChargeTime})");
                }

                // This weapon uses extra updates, so timers go extra fast!
                if (ChargeTime >= RequiredChargeTime * Projectile.extraUpdates)
                {
                    // Charge is ready!
                    if (!chargeReady)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 2);
                        }

                        chargeReady = true;

                        var readySample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/ChargeReady");
                        var readySound = readySample with
                        {
                            Volume = 0.4f,
                            MaxInstances = 1
                        };
                        SoundEngine.PlaySound(readySound);
                    }
                }

                ++ChargeTime;
                // Reset the animation and item timer while charging.
                Projectile.position.X = owner.position.X;
                Projectile.position.Y = owner.position.Y;

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
                // Main.NewText($"{MathHelper.ToDegrees(owner.itemRotation)}");

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

                    PunchCameraModifier modifier = new PunchCameraModifier(owner.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 3f, 8f, 20, 80f, FullName);
                    Main.instance.CameraModifiers.Add(modifier);
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
                Projectile.friendly = true;
                Projectile.tileCollide = true;
                SoundEngine.PlaySound(shootSound);

                // TODO: fix this scuffed way of cancelling the charge sound
                var chargeSample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/ChargeStart");
                var chargeSound = chargeSample with
                {
                    Volume = 0.0f,
                    MaxInstances = 1
                };
                SoundEngine.PlaySound(chargeSound);

                return;
            }

            Dust.NewDust(Projectile.position, Projectile.width * dustTrailRadiusMult, Projectile.height * dustTrailRadiusMult, DustID.BlueFairy,
                SpeedX: 0, SpeedY: 0, Scale: 1);
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
                float velX = (Projectile.velocity.X + Main.rand.Next(-2, 2)) * -0.5f;
                float velY = (Projectile.velocity.Y + Main.rand.Next(-2, 2)) * -0.5f;
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy, velX, velY);
            }
        }
    }
}