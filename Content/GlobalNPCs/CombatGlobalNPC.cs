using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class CombatGlobalNPC : BaseGlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            bool isFlat = target.GetModPlayer<Players.SquidPlayer>().IsFlat();
            if (isFlat)
            {
                var hitbox = new Rectangle((int)target.Left.X, (int)target.Bottom.Y - 20, target.width, 20);
                if (hitbox.Contains((int)npc.Center.X, (int)npc.Center.Y))
                {
                    return true;
                }

                return false;
            }

            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
    }
}
