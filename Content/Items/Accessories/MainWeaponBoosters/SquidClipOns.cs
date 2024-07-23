using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class SquidClipOns : BaseWeaponBoosterAccessory
    {
        public static float RollCooldownMult = 2f;
        public static float RollDistanceMult = 1.5f;
        protected override string UsageHintParamA => 50.ToString();

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasSquidClipOns = true;
            }
        }
    }
}
