using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.StaticData.LootTables;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalItems
{
    internal class GlobalTreasureBags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            foreach (var loot in LootTables.List)
            {
                if (item.type == loot.TreasureBagId)
                {
                    loot.RegisterBagDrop(itemLoot);
                }
            }
        }
    }
}
