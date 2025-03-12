using AchiSplatoon2.StaticData;
using Terraria.ModLoader;

namespace AchiSplatoon2.ModSystems
{
    internal class LootModSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            // First, the game runs the method below, then it sets the NPC loot and then the treasure bag loot
            LootTables.UpdateStaticData();
        }
    }
}
