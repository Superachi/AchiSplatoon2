using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class SplatBomb : BaseWeapon
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<SplatBombProjectile>();
            Item.damage = 36;
            Item.knockBack = 8;
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useTime = 30;
            Item.useAnimation = Item.useTime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
