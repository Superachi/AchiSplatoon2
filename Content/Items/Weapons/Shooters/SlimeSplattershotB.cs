using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SlimeSplattershotB : SlimeSplattershot
    {
        public override float InkCost { get => 6f; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 64;
            Item.knockBack = 5;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<SlimeSplattershot>());
    }
}
