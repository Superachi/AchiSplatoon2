using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class SprinklerProjectile : BaseProjectile
    {
        private float delayUntilFall = 12f;
        private readonly float delayUntilDust = 2f;
        private float fallSpeed = 0.3f;
        private readonly float terminalVelocity = 20f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ArmorPenetration = 5;
            AIType = ProjectileID.Bullet;
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.2f;
            }

            Projectile.extraUpdates += 4;
            Projectile.timeLeft *= Projectile.extraUpdates;
            fallSpeed /= Projectile.extraUpdates * 2;
            delayUntilFall *= Projectile.extraUpdates * 2;
        }

        protected override void AfterSpawn()
        {
            Initialize();

            PlayAudio(SoundID.SplashWeak, volume: 0.5f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
            if (Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.SplashWeak, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5);
            }
            if (Main.rand.NextBool(2))
            {
                PlayAudio(SoundID.Splash, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
            }

            var spreadOffset = 0.5f;
            Projectile.velocity.X += Main.rand.NextFloat(-spreadOffset, spreadOffset);
            Projectile.velocity.Y += Main.rand.NextFloat(-spreadOffset, spreadOffset);
        }

        public override void AI()
        {
            if (timeSpentAlive >= delayUntilFall)
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.99f;
                }
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            if (timeSpentAlive >= delayUntilDust)
            {
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 2, newColor: CurrentColor, Scale: 1.2f);

                if (Main.rand.NextBool(20))
                {
                    Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: CurrentColor, Scale: 1f);
                }

                if (Main.rand.NextBool(500))
                {
                    var d = Dust.NewDustPerfect(Position: Projectile.Center, Type: DustID.AncientLight, Velocity: Vector2.Zero, newColor: CurrentColor, Scale: 1f);
                    d.noGravity = true;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this, false);
            return true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 20;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }
    }
}
