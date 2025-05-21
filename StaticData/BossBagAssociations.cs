using System.Collections.Generic;
using Terraria.ID;

namespace AchiSplatoon2.StaticData
{
    internal class BossBagAssociations
    {
        public static Dictionary<int, int> AllBosses()
        {
            var dictionary = PreHardmodeBosses();

            foreach (var boss in HardmodeBosses()) dictionary.Add(boss.Key, boss.Value);

            return dictionary;
        }

        public static Dictionary<int, int> PreHardmodeBosses()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.KingSlime, ItemID.KingSlimeBossBag },
                { NPCID.EyeofCthulhu, ItemID.EyeOfCthulhuBossBag },

                { NPCID.EaterofWorldsHead, ItemID.EaterOfWorldsBossBag },
                { NPCID.BrainofCthulhu, ItemID.BrainOfCthulhuBossBag },

                { NPCID.Deerclops, ItemID.DeerclopsBossBag },
                { NPCID.QueenBee, ItemID.QueenBeeBossBag },
                { NPCID.SkeletronHead, ItemID.SkeletronBossBag },

                { NPCID.WallofFlesh, ItemID.WallOfFleshBossBag },
            };

            return dictionary;
        }

        public static Dictionary<int, int> HardmodeBosses()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.QueenSlimeBoss, ItemID.QueenSlimeBossBag },
                { NPCID.Retinazer, ItemID.TwinsBossBag },
                { NPCID.TheDestroyer, ItemID.DestroyerBossBag },
                { NPCID.SkeletronPrime, ItemID.SkeletronPrimeBossBag },
                { NPCID.Plantera, ItemID.PlanteraBossBag },

                { NPCID.Golem, ItemID.GolemBossBag },
                { NPCID.DukeFishron, ItemID.FishronBossBag },
                { NPCID.HallowBoss, ItemID.FairyQueenBossBag },
                { NPCID.CultistBoss, ItemID.CultistBossBag },
                { NPCID.MoonLordCore, ItemID.MoonLordBossBag },
            };

            return dictionary;
        }

        public static Dictionary<int, int> MechBosses()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.Retinazer, ItemID.TwinsBossBag },
                { NPCID.TheDestroyer, ItemID.DestroyerBossBag },
                { NPCID.SkeletronPrime, ItemID.SkeletronPrimeBossBag },
            };

            return dictionary;
        }

        public static Dictionary<int, int> BossesDroppingSilverLicense()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.WallofFlesh, ItemID.WallOfFleshBossBag },
                { NPCID.QueenSlimeBoss, ItemID.QueenSlimeBossBag },
                { NPCID.Retinazer, ItemID.TwinsBossBag },
                { NPCID.TheDestroyer, ItemID.DestroyerBossBag },
                { NPCID.SkeletronPrime, ItemID.SkeletronPrimeBossBag },
                { NPCID.Plantera, ItemID.PlanteraBossBag },
            };

            return dictionary;
        }

        public static Dictionary<int, int> BossesDroppingGoldLicense()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.Golem, ItemID.GolemBossBag },
                { NPCID.DukeFishron, ItemID.FishronBossBag },
                { NPCID.HallowBoss, ItemID.FairyQueenBossBag },
                { NPCID.CultistBoss, ItemID.CultistBossBag },
            };

            return dictionary;
        }

        public static Dictionary<int, int> EvilBosses()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.EaterofWorldsHead, ItemID.EaterOfWorldsBossBag },
                { NPCID.BrainofCthulhu, ItemID.BrainOfCthulhuBossBag },
            };

            return dictionary;
        }

        public static Dictionary<int, int> PostEvilBosses()
        {
            var dictionary = new Dictionary<int, int>()
            {
                { NPCID.EaterofWorldsHead, ItemID.EaterOfWorldsBossBag },
                { NPCID.BrainofCthulhu, ItemID.BrainOfCthulhuBossBag },

                { NPCID.Deerclops, ItemID.DeerclopsBossBag },
                { NPCID.QueenBee, ItemID.QueenBeeBossBag },
                { NPCID.SkeletronHead, ItemID.SkeletronBossBag },

                { NPCID.WallofFlesh, ItemID.WallOfFleshBossBag },
            };

            return dictionary;
        }
    }
}
