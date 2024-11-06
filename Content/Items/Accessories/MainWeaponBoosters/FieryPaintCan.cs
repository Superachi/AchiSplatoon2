using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class FieryPaintCan : BaseWeaponBoosterAccessory
    {
        public static float MissDamageModifier = 1.3f;
        public static float MissRadiusModifier = 1.5f;
        protected override string UsageHintParamA => ((int)((MissDamageModifier - 1) * 100)).ToString();

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 24;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.hasFieryPaintCan = true;
            }
        }
    }
}
