using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class CoralStringerB : CoralStringer
    {
        public override float InkCost { get => 1f; }
        public override bool CanShotBounce => true;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 70;
            Item.knockBack = 6;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<CoralStringer>());
    }
}
