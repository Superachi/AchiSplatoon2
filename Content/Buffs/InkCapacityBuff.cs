using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    /// <summary>
    /// This buff increases the player's ink capacity, meaning they can attack longer and recover ink faster.
    /// </summary>
    internal class InkCapacityBuff : ModBuff
    {
        public static int InkCapacityBonus = 100;

        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
        }
    }
}
