using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class Sprinkler : BaseBomb
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<SprinklerSentry>();
            Item.shootSpeed = 2f;
            Item.damage = 12;
            Item.knockBack = 4;
            Item.width = 28;
            Item.height = 28;
        }
    }
}
