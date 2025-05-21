using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories;

internal class ChargedBattery : BaseAccessory
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
        WoomyMathHelper.FloatToPercentage(ChargeSpeedFlatBonus)
        );

    public static float ChargeSpeedFlatBonus => 0.1f;
    public static float AerialChargeSpeedModOverride => 1f;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 32;
        Item.height = 32;

        Item.SetValueHighHardmodeOre();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (NetHelper.IsPlayerSameAsLocalPlayer(player))
        {
            var modPlayer = player.GetModPlayer<AccessoryPlayer>();
            modPlayer.hasChargedBattery = true;
        }
    }
}
