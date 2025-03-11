using System.Collections.Generic;
using Terraria.ID;

namespace AchiSplatoon2.StaticData
{
    internal class BossBagAssociations
    {
        public static Dictionary<int, int> Link => new()
        {
            { ItemID.KingSlimeBossBag, NPCID.KingSlime },
            { ItemID.EyeOfCthulhuBossBag, NPCID.EyeofCthulhu },
            { ItemID.EaterOfWorldsBossBag, NPCID.EaterofWorldsHead },
            { ItemID.BrainOfCthulhuBossBag, NPCID.BrainofCthulhu },
            { ItemID.DeerclopsBossBag, NPCID.Deerclops },
            { ItemID.QueenBeeBossBag, NPCID.QueenBee },
            { ItemID.SkeletronBossBag, NPCID.SkeletronHead },

            { ItemID.WallOfFleshBossBag, NPCID.WallofFlesh },
            { ItemID.TwinsBossBag, NPCID.Retinazer },
            { ItemID.DestroyerBossBag, NPCID.TheDestroyer },
            { ItemID.SkeletronPrimeBossBag, NPCID.SkeletronPrime },

            { ItemID.PlanteraBossBag, NPCID.Plantera },
            { ItemID.GolemBossBag, NPCID.Golem },
            { ItemID.FishronBossBag, NPCID.DukeFishron },
            { ItemID.CultistBossBag, NPCID.CultistBoss },
            { ItemID.MoonLordBossBag, NPCID.MoonLordCore },
        };
    }
}
