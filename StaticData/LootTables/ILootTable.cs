using System.Collections.Generic;

namespace AchiSplatoon2.StaticData.LootTables
{
    interface ILootTable
    {
        List<LootTableIndex> Indices();
    }
}
