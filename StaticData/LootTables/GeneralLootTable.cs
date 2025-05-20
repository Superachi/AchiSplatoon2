using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Consumables.ColorVials.Gradients;
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
                    chanceDenominator: 5)
            };

            // Honey scepter
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

            // Urchin emblem
            var waterEnemies = new Dictionary<int, int>()
            {
                { NPCID.BlueJellyfish, -1 },
                { NPCID.PinkJellyfish, -1 },
                { NPCID.Piranha, -1 },
                { NPCID.CorruptGoldfish, -1 },
                { NPCID.CrimsonGoldfish, -1 },
            };

            var urchinDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<UrchinEmblem>(),
                npcIdAndBagId: waterEnemies,
                chanceDenominator: 40);

            list = list.Concat(urchinDrops).ToList();

            // Color vials
            list.Add(
                new LootTableIndex(
                    itemId: ModContent.ItemType<FireGradientVial>(),
                    npcId: NPCID.LavaSlime,
                    chanceDenominator: 20)
                );

            list.Add(
                new LootTableIndex(
                    itemId: ModContent.ItemType<RainbowGradientVial>(),
                    npcId: NPCID.RainbowSlime,
                    chanceDenominator: 10)
                );

            list.Add(
                new LootTableIndex(
                    itemId: ModContent.ItemType<WaterGradientVial>(),
                    npcId: NPCID.IceSlime,
                    chanceDenominator: 20)
                );

            return list;
        }
    }
}
