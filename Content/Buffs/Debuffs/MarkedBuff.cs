using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.GlobalNPCs;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs.Debuffs
{
    internal class MarkedBuff : ModBuff
    {
        public static float DamageMultiplier = 1.1f;

        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }

        public static void Apply(NPC npc, int duration)
        {
            npc.AddBuff(ModContent.BuffType<MarkedBuff>(), duration);
            npc.GetGlobalNPC<MarkBuffGlobalNPC>().ApplyEffect();
            SoundHelper.PlayAudio(SoundPaths.Marked.ToSoundStyle(), position: npc.Center, volume: 0.3f);
        }

        public static void ApplyToNpcInRadius(Vector2 position, float radius, int duration)
        {
            foreach (var npc in Main.npc)
            {
                if (!npc.active
                    || npc.friendly
                    || npc.type == NPCID.TargetDummy
                    || Main.npcCatchable[npc.type]
                    || NpcHelper.IsTargetAProjectile(npc)
                    || NpcHelper.IsTargetAWormHead(npc))
                {
                    continue;
                }

                if (npc.Distance(position) < radius)
                {
                    Apply(npc, duration);
                }
            }
        }

        public static void Apply(Player player, int duration)
        {
            player.AddBuff(ModContent.BuffType<MarkedBuff>(), duration);
            SoundHelper.PlayAudio(SoundPaths.Marked.ToSoundStyle(), position: player.Center, volume: 0.3f);
        }
    }
}
