using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatChargerProjectile : ModProjectile
    {
        private bool chargeReady = false;
        private bool hasFired = false;
        private const float requiredChargeTime = 55f;
        private int dustTrailRadiusMult = 2;

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
            Projectile.extraUpdates = 24;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 36000;
            Projectile.penetrate = 5;
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
                    // Main.NewText($"Owner started channeling. (Charge Time = {ChargeTime})");
                }

                // This weapon uses extra updates, so timers go extra fast!
                if (ChargeTime >= requiredChargeTime * Projectile.extraUpdates)
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

            // Exit if charge isn't ready, otherwise, release attack
            if (!chargeReady)
            {
                // Main.NewText($"Owner stopped channeling. (Charge Time = {ChargeTime})");
                Projectile.Kill();
                return;
            }

            if (!hasFired)
            {
                // Release the attack
                hasFired = true;
                Projectile.velocity = owner.DirectionTo(Main.MouseWorld) * 3f;
                Projectile.timeLeft = 120 * Projectile.extraUpdates;
                Projectile.friendly = true;
                Projectile.tileCollide = true;

                // Main.NewText("Owner attacked.");

                var shootSample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/SplatChargerShoot");
                var shootSound = shootSample with
                {
                    Volume = 0.3f,
                    PitchVariance = 0.1f,
                    MaxInstances = 1
                };
                SoundEngine.PlaySound(shootSound);
                return;
            }

            Dust.NewDust(Projectile.position, Projectile.width * dustTrailRadiusMult, Projectile.height * dustTrailRadiusMult, DustID.Clentaminator_Blue,
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
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water, velX, velY);
            }
        }
    }
}