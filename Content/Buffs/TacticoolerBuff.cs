using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class TacticoolerBuff : ModBuff
    {
        public static int BaseDuration => 1800;
        public static float MoveSpeedBonus => 0.5f;
        public static float MoveAccelBonus => 2f;
        public static float MoveFrictionBonus => 2f;

        public static int CritBonus => 10;
        public static int DefenseBonus => 8;
        public static int LifeRegenBonus => 5;

        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
            player.statDefense += DefenseBonus;
            player.GetCritChance(DamageClass.Generic) += CritBonus;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            // Stat bonus section
            string tooltip = ColorHelper.TextWithNeutralColor("The Tacticooler's drink improves your mobility and other stats!") + "\n";
            tooltip += ColorHelper.TextWithNeutralColor("Life regeneration bonus: ") + ColorHelper.TextWithBonusColor($"+{LifeRegenBonus} life per second") + "\n";
            tooltip += ColorHelper.TextWithNeutralColor("Defense bonus: ") + ColorHelper.TextWithBonusColor($"+{DefenseBonus}") + "\n";
            tooltip += ColorHelper.TextWithNeutralColor("Critical strike chance bonus: ") + ColorHelper.TextWithBonusColor($"+{CritBonus}%") + "\n";

            tip = tooltip;
        }
    }
}
