using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs
{
    internal class DroneDiscB : DroneDiscBase
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
