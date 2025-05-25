using AchiSplatoon2.Content.Items.CraftingMaterials;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.Vanity
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Head)]
    internal class EnchantedHat : BaseVanityItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 1)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 25)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}
