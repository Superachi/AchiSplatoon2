using AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Unclassed
{
    internal class HoneyScepter : BaseWeapon
    {
        public override float InkCost { get => 8f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Other;

        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<HoneyScepterProjectile>(),
                singleShotTime: 30,
                shotVelocity: 1);

            Item.DamageType = DamageClass.Magic;

            Item.damage = 18;
            Item.knockBack = 0;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
