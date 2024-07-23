using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class BigBlastBuff : ModBuff
    {
        public override LocalizedText Description => base.Description.WithFormatArgs(
            WoomyMathHelper.FloatToPercentage(FieryPaintCan.MissDamageModifier - 1f),
            WoomyMathHelper.FloatToPercentage(FieryPaintCan.MissRadiusModifier - 1f));

        public override void Update(Player player, ref int buffIndex)
        {
            player.AddBuff(ModContent.BuffType<BigBlastBuff>(), 2);
        }
    }
}
