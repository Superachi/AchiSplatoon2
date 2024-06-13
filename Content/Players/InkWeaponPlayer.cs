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
        public int ColorChipRedAmount;
        public int ColorChipBlueAmount;
        public int ColorChipYellowAmount;
        public int ColorChipPurpleAmount;
        public int ColorChipGreenAmount;
        public int ColorChipAquaAmount;

        public override void ResetEffects()
        {
            ColorChipRedAmount = 0;
            ColorChipBlueAmount = 0;
            ColorChipYellowAmount = 0;
            ColorChipPurpleAmount = 0;
            ColorChipGreenAmount = 0;
            ColorChipAquaAmount = 0;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.myPlayer == proj.owner)
            {
                modifiers.SourceDamage *= 1 + (ColorChipRedAmount / 10);
            }
        }
    }
}
