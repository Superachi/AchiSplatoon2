using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class SwimFormBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoTimeDisplay[buffIndex] = true;
        }
    }
}
