using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class UltraStampBuff : ModBuff
    {
        public static readonly float DamageReductionPercentage = 75f;
        public override LocalizedText Description => base.Description.WithFormatArgs(DamageReductionPercentage);
        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoTimeDisplay[buffIndex] = true;
            player.endurance = DamageReductionPercentage / 100f;
        }
    }
}
