using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class BombRushBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}
