using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class SplatBomb : BaseBomb
    {
        public override int ExplosionRadius { get => 240; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<SplatBombProjectile>();
            Item.damage = 75;
            Item.knockBack = 8;
            Item.width = 28;
            Item.height = 28;
        }
    }
}
