using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories;

internal class ChargedBattery : BaseAccessory
{
    public static float ChargeSpeedFlatBonus => 0.2f;
    public static float AerialChargeSpeedModOverride => 0.8f;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 32;
        Item.height = 32;

        Item.value = Item.buyPrice(gold: 10);
        Item.rare = ItemRarityID.LightPurple;
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
