using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class GrizzcoChargerProjectile : ChargerProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;

            Projectile.extraUpdates = 96;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120 * Projectile.extraUpdates;
        }

        public override void ApplyWeaponInstanceData()
        {
            var weaponData = WeaponInstance;

            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            wasParentChargeMaxed = true;
            PlayShootSound();

            wormDamageReduction = false;
            velocityBeforeTilePierce = Projectile.velocity;

            tilePiercesLeft = TentacularOcular.TerrainMaxPierceCount;
            hasTentacleScope = GetOwnerModPlayer<AccessoryPlayer>().hasTentacleScope;
            Projectile.tileCollide = !hasTentacleScope;
        }
    }
}
