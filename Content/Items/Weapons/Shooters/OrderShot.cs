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

            Item.useTime = 9;
            Item.useAnimation = Item.useTime;
            Item.damage = 8;
            Item.knockBack = 0.8f;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
