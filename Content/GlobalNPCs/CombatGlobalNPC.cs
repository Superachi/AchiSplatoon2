using Terraria;
using Terraria.DataStructures;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class CombatGlobalNPC : BaseGlobalNPC
    {
        public float initialKnockbackResist = 0f;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            initialKnockbackResist = npc.knockBackResist;
        }
    }
}
