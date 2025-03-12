using AchiSplatoon2.StaticData;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace AchiSplatoon2.ExtensionMethods
{
    public static class LootExtensions
    {
        public static LootTableIndex FromAnotherSource(this LootTableIndex index, int npcId, int treasureBagId = -1)
        {
            var newIndex = new LootTableIndex(
                modItemId: index.ModItemId,
                minimumDropped: index.MinimumDropped,
                maximumDropped: index.MaximumDropped,
                chanceDenominator: index.ChanceDenominator,
                npcId: npcId,
                treasureBagId: treasureBagId
            );

            return newIndex;
        }

        public static void RegisterDirectDrop(this LootTableIndex index, NPCLoot npcLoot)
        {
            // If no boss bag is associated, just drop directly from the NPC
            // Otherwise, only drop directly from the NPC if it's not expert mode
            if (index.TreasureBagId == -1)
            {
                var dropRule = ItemDropRule.Common(
                    index.ModItemId,
                    index.ChanceDenominator,
                    index.MinimumDropped,
                    index.MaximumDropped);

                npcLoot.Add(dropRule);
            }
            else
            {
                var dropRule = ItemDropRule.ByCondition(
                    new Conditions.NotExpert(),
                    index.ModItemId,
                    index.ChanceDenominator,
                    index.MinimumDropped,
                    index.MaximumDropped);

                npcLoot.Add(dropRule);
            }
        }

        public static void RegisterBagDrop(this LootTableIndex index, ItemLoot itemLoot)
        {
            var dropRule = ItemDropRule.Common(
                index.ModItemId,
                index.ChanceDenominator,
                index.MinimumDropped,
                index.MaximumDropped);

            itemLoot.Add(dropRule);
        }
    }
}
