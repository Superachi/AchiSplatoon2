using AchiSplatoon2.Content.Players;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class MainSaverEmblem : BaseAccessory
    {
        public virtual float InkSaverAmount()
        {
            return 0.3f;
        }

        public float InkSaverMult()
        {
            return 1f - InkSaverAmount();
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            (int)Math.Round(InkSaverAmount() * 100));

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<MainSaverEmblem>();
        }
    }
}
