using AchiSplatoon2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkWeaponPlayer : ModPlayer
    {
        public bool isPaletteEquipped;
        public int[] ColorChipAmounts;
        public int ColorChipTotal;
        public float RedChipBaseDamageBonus { get => 0.03f; }
        public float RedChipBaseAttackSpeedBonus { get => 0.03f; }
        public float PurpleChipBaseKnockbackBonus { get => 2f; }
        public float GreenChipBaseCritBonus { get => 5f; }
        public float BlueChipBaseMoveSpeedBonus { get => 0.2f; }

        public enum ChipColor
        {
            Red,
            Blue,
            Yellow,
            Purple,
            Green,
            Aqua,
        }

        public override void ResetEffects()
        {
            isPaletteEquipped = false;
            ColorChipAmounts = [0, 0, 0, 0, 0, 0];
            ColorChipTotal = 0;
        }

        public void CalculateColorChipTotal()
        {
            ColorChipTotal = 0;
            for (int i = 0; i < ColorChipAmounts.Length; i++)
            {
                ColorChipTotal += ColorChipAmounts[i];
            }
        }
    }
}
