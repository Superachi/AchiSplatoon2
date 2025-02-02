using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Unclassed
{
    [ItemCategory("Unclassed", "Unclassed")]
    internal class SquidBoomerang : BaseWeapon
    {
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Other;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SquidBoomerangProjectile>(),
                singleShotTime: 12,
                shotVelocity: 8f);

            Item.DamageType = DamageClass.Melee;

            Item.damage = 15;
            Item.knockBack = 2;
            Item.autoReuse = false;
            Item.noMelee = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }

        public override bool CanUseItem(Player player)
        {
            if (base.CanUseItem(player))
            {
                return player.ownedProjectileCounts[Item.shoot] == 0;
            }

            return false;
        }
    }
}
