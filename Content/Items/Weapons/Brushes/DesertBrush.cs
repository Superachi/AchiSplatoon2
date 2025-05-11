using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class DesertBrush : BaseBrush
    {
        public override float InkCost { get => 5f; }

        protected override int ArmorPierce => 20;
        public override float AimDeviation { get => 3f; }
        public override float ShotVelocity => 8f;
        public override float BaseWeaponUseTime => 20f;
        public override int WindupTime => 30;
        public override int SwingArc => 160;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.shoot = ModContent.ProjectileType<BrushSwingProjectile>();

            Item.damage = 6;
            Item.crit = 10;
            Item.knockBack = 6;

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
