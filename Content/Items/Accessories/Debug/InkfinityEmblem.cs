using AchiSplatoon2.Content.Players;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.Debug
{
    internal class InkfinityEmblem : BaseAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Expert;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<InkfinityEmblem>();
        }
    }
}
