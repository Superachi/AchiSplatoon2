using AchiSplatoon2.Content.Items.CraftingMaterials;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.Vanity
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Head)]
    internal class PearlescentCrown : BaseVanityItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 10)
                .AddIngredient(ItemID.Sapphire, 2)
                .AddIngredient(ItemID.Ruby, 2)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 25)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 10)
                .AddIngredient(ItemID.Sapphire, 2)
                .AddIngredient(ItemID.Ruby, 2)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
