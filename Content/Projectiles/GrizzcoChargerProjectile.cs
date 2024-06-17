using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class GrizzcoChargerProjectile : BaseProjectile
    {
        protected string ShootSample { get => "BambooChargerShoot"; }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.extraUpdates = 32;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120 * Projectile.extraUpdates;

            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            PlayAudio(ShootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 1);
        }

        public override void AI()
        {
            Color dustColor = GenerateInkColor();
            var randomDustVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: randomDustVelocity, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
        }
    }
}
