using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.AgentEight
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Legs)]
    internal class EightBoots : EightItem
    {
        public override void AddRecipes() => AddRecipeWithSheldonLicenseBasic();
    }
}
