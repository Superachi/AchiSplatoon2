using AchiSplatoon2.Content.Items.CraftingMaterials;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.Vanity
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Head)]
    internal class MarinatedHeadphones : BaseVanityItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 5)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.Wire, 20)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 25)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 5)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.Wire, 20)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
