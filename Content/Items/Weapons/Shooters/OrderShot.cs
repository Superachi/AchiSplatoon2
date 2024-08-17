using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class OrderShot : Splattershot
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<Splattershot>();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useTime = 12;
            Item.useAnimation = Item.useTime;
            Item.damage = 8;
            Item.knockBack = 0.8f;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;

            // Note: hide this stat from the player-- the Order Shot shouldn't be seen as a swapout for high-def enemies
            Item.ArmorPenetration = 3;
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
