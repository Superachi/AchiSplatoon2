using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class UltraStampBuff : ModBuff
    {
        public static readonly int DamageReductionPercentage = 80;
        public override LocalizedText Description => base.Description.WithFormatArgs(DamageReductionPercentage);
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance = (float)DamageReductionPercentage / 100f;
        }
    }
}
