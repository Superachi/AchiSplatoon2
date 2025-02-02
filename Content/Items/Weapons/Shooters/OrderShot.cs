using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class OrderShot : Splattershot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.shootSpeed = 4.5f;
            Item.useTime = 12;
            Item.useAnimation = Item.useTime;
            Item.damage = 8;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
