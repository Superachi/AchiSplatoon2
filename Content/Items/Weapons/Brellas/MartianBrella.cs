using AchiSplatoon2.Content.Projectiles.BrellaProjectiles.MartianBrellaProjectiles;
using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class MartianBrella : BaseBrella
    {
        public override float ShotGravity { get => 0.4f; }
        public override int ShotGravityDelay { get => 32; }
        public override int ShotExtraUpdates { get => 6; }
        public override float AimDeviation { get => 5f; }

        // Brella specific
        public override int ProjectileType { get => ModContent.ProjectileType<MartianBrellaPelletProjectile>(); }
        public override int MeleeProjectileType { get => ModContent.ProjectileType<MartianBrellaMeleeProjectile>(); }
        public override int ProjectileCount { get => 2; }
        public override float ShotgunArc { get => 10f; }
        public override float ShotVelocityRandomRange => 0f;
        public override int ShieldLife => 300;
        public override int ShieldCooldown => 450;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<MartianBrellaShotgunProjectile>(),
                singleShotTime: 12,
                shotVelocity: 8f);

            Item.damage = 46;
            Item.width = 54;
            Item.height = 62;
            Item.knockBack = 6;

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
