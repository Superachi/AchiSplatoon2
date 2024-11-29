using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    /// <summary>
    /// This buff increases the player's ink capacity, meaning they can attack longer and recover ink faster.
    /// </summary>
    internal class InkCapacityBuff : ModBuff
    {
        public static int InkCapacityBonus = 100;
        public override LocalizedText Description => base.Description.WithFormatArgs(InkCapacityBonus);

        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoSave[buffIndex] = true;
        }
    }
}
