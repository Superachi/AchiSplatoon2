using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.AgentEight
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Body)]
    internal class EightSuit : EightItem
    {
        public override void AddRecipes() => AddRecipeWithSheldonLicenseBasic();
    }
}
