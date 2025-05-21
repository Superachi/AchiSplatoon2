using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class DesertBrush : BaseBrush
    {
        public override float InkCost { get => 3f; }

        protected override int ArmorPierce => 5;
        public override float AimDeviation { get => 0f; }
        public override float ShotVelocity => 8f;
        public override float BaseWeaponUseTime => 15f;
        public override int WindupTime => 0;
        public override int SwingArc => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.shoot = ModContent.ProjectileType<DesertBrushSwingProjectile>();

            Item.damage = 6;
            Item.crit = 10;
            Item.knockBack = 0.5f;

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
