using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class CustomEliterCharger : EliterCharger
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 300;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofSight);
    }
}
