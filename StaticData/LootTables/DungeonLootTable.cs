
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.StaticData.LootTables
{
    internal class DungeonLootTable
    {
        public List<LootTableIndex> Indices()
        {
            var list = new List<LootTableIndex>()
            {
                new LootTableIndex(
                    itemId: ModContent.ItemType<Snipewriter5B>(),
                    npcId: NPCID.SkeletonSniper,
                    chanceDenominator: 8),

                new LootTableIndex(
                    itemId: ModContent.ItemType<RecycleBrellaMk2>(),
                    npcId: NPCID.TacticalSkeleton,
                    chanceDenominator: 8),

                new LootTableIndex(
                    itemId: ModContent.ItemType<SBlast91>(),
                    npcId: NPCID.SkeletonCommando,
                    chanceDenominator: 8),

                new LootTableIndex(
                    itemId: ModContent.ItemType<PainBrushNouveau>(),
                    npcId: NPCID.Paladin,
                    chanceDenominator: 8),
            };

            // Splash/Sploosh drops
            var rustyArmoredEnemies = new Dictionary<int, int>()
            {
                { NPCID.RustyArmoredBonesAxe, -1 },
                { NPCID.RustyArmoredBonesFlail, -1 },
                { NPCID.RustyArmoredBonesSword, -1 },
                { NPCID.RustyArmoredBonesSwordNoArmor, -1 },
            };

            var rustyArmoredDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<SplashOMaticNeo>(),
                npcIdAndBagId: rustyArmoredEnemies,
                chanceDenominator: 80);

            var hellArmoredEnemies = new Dictionary<int, int>()
            {
                { NPCID.HellArmoredBones, -1 },
                { NPCID.HellArmoredBonesMace, -1 },
                { NPCID.HellArmoredBonesSpikeShield, -1 },
                { NPCID.HellArmoredBonesSword, -1 },
            };

            var hellArmoredDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<SplooshOMaticNeo>(),
                npcIdAndBagId: hellArmoredEnemies,
                chanceDenominator: 80);

            return list
                .Concat(rustyArmoredDrops)
                .Concat(hellArmoredDrops)
                .ToList();
        }
    }
}
