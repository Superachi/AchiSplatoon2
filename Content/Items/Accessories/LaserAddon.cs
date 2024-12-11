using AchiSplatoon2.Content.Players;
using Terraria.ID;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class LaserAddon : BaseAccessory
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(_displayValue);

        public static float InkCapacityMult => 0.7f;
        private static int _displayValue = (int)((1f - InkCapacityMult) * 100);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 30;
            Item.height = 24;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<LaserAddon>();
        }
    }
}
