using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items
{
    internal class BaseItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            int? shimmerResult = ShimmerItemList.GetShimmerItemType(Item.type);
            if (shimmerResult != null)
            {
                ItemID.Sets.ShimmerTransformToItem[Item.type] = (int)shimmerResult;
            }
        }
    }
}
