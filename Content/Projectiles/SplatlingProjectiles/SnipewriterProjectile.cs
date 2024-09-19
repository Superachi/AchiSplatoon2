using AchiSplatoon2.Content.Dusts;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Items.Weapons.Splatling;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles
{
    internal class SnipewriterProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.extraUpdates = 64;
            Projectile.timeLeft = 120 * Projectile.extraUpdates;
            AIType = ProjectileID.Bullet;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as Snipewriter5H;
            shootSample = weaponData.ShootSample;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayAudio(shootSample, volume: 0.4f, pitchVariance: 0.4f, maxInstances: 1);
        }

        public override void AI()
        {
            Color dustColor = GenerateInkColor();
            var randomDustVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            if (Main.rand.NextBool(5))
            {
                Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: randomDustVelocity, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<ChargerBulletDust>(), Velocity: Projectile.velocity / 2, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 20;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }
    }
}
