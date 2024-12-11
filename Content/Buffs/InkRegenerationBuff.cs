using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    /// <summary>
    /// This buff should make it so the player always recovers ink as if they were submerged and not moving.
    /// </summary>
    internal class InkRegenerationBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
        }
    }
}
