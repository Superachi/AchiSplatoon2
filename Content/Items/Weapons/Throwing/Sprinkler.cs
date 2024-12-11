using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria;
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
            Item.damage = 5;
            Item.knockBack = 2f;
            Item.width = 28;
            Item.height = 28;
        }
    }
}
