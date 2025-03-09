using AchiSplatoon2.Content.Players;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class OrderEmblem : BaseAccessory
    {
        public static float OrderWeaponDamageBonus => 0.5f;

        protected override string UsageHintParamA => $"{(int)(OrderWeaponDamageBonus * 100)}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<OrderEmblem>();
        }
    }
}
