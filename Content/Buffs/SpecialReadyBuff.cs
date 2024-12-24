using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class SpecialReadyBuff : ModBuff
    {
        public override LocalizedText Description => base.Description.WithFormatArgs();

        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
            Main.buffNoTimeDisplay[Type] = true;

            player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 2);
        }

        public override bool RightClick(int buffIndex)
        {
            Player player = Main.LocalPlayer;

            if (player.GetModPlayer<SpecialPlayer>().SpecialActivated)
            {
                return false;
            }

            player.GetModPlayer<HudPlayer>().SetOverheadText("Cancelled special!", 90);
            return true;
        }
    }
}
