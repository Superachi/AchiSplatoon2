using System.Collections.Generic;
using System.Linq;

namespace AchiSplatoon2.StaticData.LootTables
{
    internal class LootTables
    {
        public static List<LootTableIndex> List { get; private set; }
        public static void UpdateStaticData()
        {
            List = AllLootIndices();
        }

        public static List<LootTableIndex> AllLootIndices()
        {
            var list = new List<LootTableIndex>();

            list = list
                .Concat(new GeneralLootTable().Indices())
                .Concat(new BossLootTable().Indices())
                .Concat(new MimicLootTable().Indices())
                .Concat(new InvasionLootTable().Indices())
                .ToList();

            return list;
        }

        public static List<LootTableIndex> FindIndicesById(int modItemId)
        {
            return List.Where(index => index.itemIds.Contains(modItemId)).ToList();
        }
    }
}
