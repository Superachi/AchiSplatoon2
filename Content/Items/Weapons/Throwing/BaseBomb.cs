using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class BaseBomb : BaseWeapon
    {
        public virtual int ExplosionRadius { get => 100; }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 15f;
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
