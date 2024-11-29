using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class SpawnGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Water)
            {
                pool.Add(NPCID.Squid, 0.2f);
            }
        }
    }
}
