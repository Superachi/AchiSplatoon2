using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.AgentEight
{
    [Autoload(true)]
    [AutoloadEquip(EquipType.Head)]
    internal class EightMask : EightItem
    {
        public override void AddRecipes() => AddRecipeWithSheldonLicenseBasic();

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EightSuit>() && legs.type == ModContent.ItemType<EightBoots>();
        }
    }
}
