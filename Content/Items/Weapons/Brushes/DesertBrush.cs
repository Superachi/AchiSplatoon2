using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class DesertBrush : BaseBrush
    {
        public override float InkCost { get => 4f; }

        public override float AimDeviation { get => 4f; }
        public override float ShotVelocity => 7f;
        public override float BaseWeaponUseTime => 24f;
        public override int WindupTime => 26;
        public override int SwingArc => 160;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.shoot = ModContent.ProjectileType<BrushSwingProjectile>();

            Item.damage = 12;
            Item.knockBack = 5;

            Item.scale = 1;
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
