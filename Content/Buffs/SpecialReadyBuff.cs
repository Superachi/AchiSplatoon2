using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Buffs
{
    internal class SpecialReadyBuff : ModBuff
    {
        public override LocalizedText Description => base.Description.WithFormatArgs();
        
        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
            player.AddBuff(ModContent.BuffType<SpecialReadyBuff>(), 2);
            Main.buffNoSave[buffIndex] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
            modPlayer.ResetSpecialStats();
            CombatTextHelper.DisplayText("Cancelled special!", player.Center);
            return true;
        }
    }
}
