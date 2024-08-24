using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Projectiles.BrushProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class DesertBrush : BaseBrush
    {
        public override float AimDeviation { get => 4f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<DesertBrushProjectile>();

            Item.damage = 10;
            Item.knockBack = 3;

            Item.scale = 1;
            Item.useTime = 18;
            Item.useAnimation = Item.useTime;

            Item.width = 52;
            Item.height = 52;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.FossilOre, 8)
                .AddIngredient(ItemID.AntlionMandible, 5)
                .AddIngredient(ItemID.Sapphire, 1)
                .Register();
        }
    }
}
