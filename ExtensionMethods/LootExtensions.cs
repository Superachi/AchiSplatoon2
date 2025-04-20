using AchiSplatoon2.StaticData;
using System;
using System.Linq;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace AchiSplatoon2.ExtensionMethods
{
    public static class LootExtensions
    {
        public static LootTableIndex FromAnotherSource(this LootTableIndex index, int npcId, int treasureBagId = -1)
        {
            var itemIds = index.itemIds.ToArray();
            if (itemIds == null)
            {
                throw new ArgumentException($"Failed to use the provided {nameof(LootTableIndex)}, as it was null.");
            }

            if (index.itemIds.Count == 1)
            {
                return new LootTableIndex(
                    itemId: index.itemIds.First(),
                    npcId: npcId,
                    minimumDropped: index.MinimumDropped,
                    maximumDropped: index.MaximumDropped,
                    chanceDenominator: index.ChanceDenominator,
                    treasureBagId: treasureBagId
                );
            }
            else
            {
                return new LootTableIndex(
                    itemIdOptions: itemIds,
                    npcId: npcId,
                    chanceDenominator: index.ChanceDenominator,
                    treasureBagId: treasureBagId
                );
            }
        }

        public static void RegisterDirectDrop(this LootTableIndex index, NPCLoot npcLoot)
        {
            if (index.itemIds.Count == 0)
            {
                throw new ArgumentException("Cannot register a treasure bag drop without providing an item ID.");
            }

            // If no boss bag is associated, just drop directly from the NPC
            // Otherwise, only drop directly from the NPC if it's not expert mode
            if (index.TreasureBagId == -1)
            {
                if (index.itemIds.Count == 1)
                {
                    var dropRule = ItemDropRule.Common(
                        index.itemIds.First(),
                        index.ChanceDenominator,
                        index.MinimumDropped,
                        index.MaximumDropped);

                    npcLoot.Add(dropRule);
                }
                else
                {
                    var dropRule = ItemDropRule.OneFromOptions(
                        index.ChanceDenominator,
                        index.itemIds.ToArray());

                    npcLoot.Add(dropRule);
                }
            }
            else
            {
                if (index.itemIds.Count == 1)
                {
                    var isNotExpert = new Conditions.NotExpert();
                    LeadingConditionRule leadingConditionRule = new(isNotExpert);

                    var dropRule = ItemDropRule.Common(
                        index.itemIds.First(),
                        index.ChanceDenominator,
                        index.MinimumDropped,
                        index.MaximumDropped);

                    leadingConditionRule.OnSuccess(dropRule);

                    npcLoot.Add(leadingConditionRule);
                }
                else
                {
                    var isNotExpert = new Conditions.NotExpert();
                    LeadingConditionRule leadingConditionRule = new(isNotExpert);

                    var dropRule = ItemDropRule.OneFromOptions(
                        index.ChanceDenominator,
                        index.itemIds.ToArray());

                    leadingConditionRule.OnSuccess(dropRule);

                    npcLoot.Add(leadingConditionRule);
                }
            }
        }

        public static void RegisterBagDrop(this LootTableIndex index, ItemLoot itemLoot)
        {
            if (index.itemIds.Count == 0)
            {
                throw new ArgumentException("Cannot register a treasure bag drop without providing an item ID.");
            }

            if (index.itemIds.Count == 1)
            {
                var dropRule = ItemDropRule.Common(
                    index.itemIds.First(),
                    index.ChanceDenominator,
                    index.MinimumDropped,
                    index.MaximumDropped);

                itemLoot.Add(dropRule);
            }
            else
            {
                var dropRule = ItemDropRule.OneFromOptions(
                    index.ChanceDenominator,
                    index.itemIds.ToArray());

                itemLoot.Add(dropRule);
            }
        }
    }
}
