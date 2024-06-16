using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class AngleShooter : BaseBomb
    {
        public override int MaxBounces { get => 5; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<AngleShooterProjectile>();
            Item.shootSpeed = 1f;
            Item.damage = 24;
            Item.knockBack = 4;
            Item.width = 30;
            Item.height = 16;
        }
    }
}
