using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.AgentEight
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Head)]
    internal class EightMask : EightItem
    {
        public override void AddRecipes() => AddRecipeWithSheldonLicenseBasic();
    }
}
