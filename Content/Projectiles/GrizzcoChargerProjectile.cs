using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class GrizzcoChargerProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.extraUpdates = 48;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120 * Projectile.extraUpdates;

            AIType = ProjectileID.Bullet;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as GrizzcoCharger;

            shootSample = weaponData.ShootSample;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 1);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 20;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override void AI()
        {
            Color dustColor = GenerateInkColor();
            var randomDustVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            if (Main.rand.NextBool(10))
            {
                Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: randomDustVelocity, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            }
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 2, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
        }
    }
}
