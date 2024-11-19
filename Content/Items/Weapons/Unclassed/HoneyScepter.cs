using AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Humanizer;

namespace AchiSplatoon2.Content.Items.Weapons.Unclassed
{
    internal class HoneyScepter : BaseWeapon
    {
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
                shotVelocity: 0f);

            Item.DamageType = DamageClass.Magic;

            Item.damage = 18;
            Item.mana = 18;
            Item.knockBack = 0;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
