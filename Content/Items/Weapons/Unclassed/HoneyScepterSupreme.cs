using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Unclassed
{
    internal class HoneyScepterSupreme : HoneyScepter
    {
        public override float InkCost { get => 12f; }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 56;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<HoneyScepter>());
    }
}
