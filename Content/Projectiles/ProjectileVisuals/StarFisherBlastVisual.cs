using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ProjectileVisuals
{
    internal class StarFisherBlastVisual : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 6;
        }

        public override void AfterSpawn()
        {
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.FireworksRGB,
                    Main.rand.NextVector2Circular(10, 10),
                    255,
                    GenerateInkColor());

                dust.noGravity = true;
            }
        }
    }
}
