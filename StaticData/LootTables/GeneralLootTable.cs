using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Unclassed;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.StaticData.LootTables
{
    internal class GeneralLootTable : ILootTable
    {
        public List<LootTableIndex> Indices()
        {
            var list = new List<LootTableIndex>()
            {
                new LootTableIndex(
                    itemId: ModContent.ItemType<SquidBoomerang>(),
                    npcId: NPCID.Squid,
                    chanceDenominator: 10)
            };

            var hornetEnemies = new Dictionary<int, int>()
                {
                    { NPCID.Hornet, -1 },
                    { NPCID.HornetFatty, -1 },
                    { NPCID.HornetHoney, -1 },
                    { NPCID.HornetLeafy, -1 },
                    { NPCID.HornetSpikey, -1 },
                };

            var hornetDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<HoneyScepter>(),
                npcIdAndBagId: hornetEnemies,
                chanceDenominator: 200);

            list = list.Concat(hornetDrops).ToList();

            return list;
        }
    }
}
