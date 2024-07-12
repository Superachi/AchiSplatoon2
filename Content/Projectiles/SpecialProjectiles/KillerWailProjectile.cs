using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class KillerWailProjectile : BaseProjectile
    {
        protected override bool CountDamageForSpecialCharge { get => false; }
        protected virtual bool enableDust => true;
        protected float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 5;
            Projectile.width = 74;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            AIType = ProjectileID.Bullet;
        }

        public override void AfterSpawn()
        {
            Initialize(ignoreAimDeviation: true);
            Projectile.alpha = 255;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, GenerateInkColor().ToVector3());

            if (enableDust && Main.rand.NextBool(5))
            {
                var dust = Dust.NewDust(
                    Position: Projectile.position,
                    Width: Projectile.width,
                    Height: Projectile.height,
                    Type: DustID.CrystalPulse,
                    SpeedX: Projectile.velocity.X / 3,
                    SpeedY: Projectile.velocity.Y / 3,
                    newColor: GenerateInkColor(),
                    Scale: 0.5f);
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }

            if (Projectile.scale < 1)
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1, 0.2f);
            }

            if (Projectile.timeLeft <= 45)
            {
                Projectile.velocity = Projectile.velocity * 0.98f;
                Projectile.alpha += 3;
            }
        }
    }
}
