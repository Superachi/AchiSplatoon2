using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class Octobrush : BaseBrush
    {
        public override float AimDeviation { get => 6f; }
        public override float DelayUntilFall => 12f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 18;
            Item.knockBack = 4;
            Item.shootSpeed = 8f;

            Item.scale = 1.5f;
            Item.useTime = 18;
            Item.useAnimation = Item.useTime;

            Item.width = 60;
            Item.height = 60;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
