using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class AdamantiteCoil : BaseWeaponBoosterAccessory
    {
        public static float BaseDamageMod => 2f;
        public static float PostFirstHitDamageMod => 0.8f;
        protected override string UsageHintParamA => $"{Math.Round((BaseDamageMod - 1) * 100)}";
        protected override string UsageHintParamB => $"{Math.Round((1 - PostFirstHitDamageMod) * 100)}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 20;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.hasSteelCoil = true;
            }
        }
    }
}
