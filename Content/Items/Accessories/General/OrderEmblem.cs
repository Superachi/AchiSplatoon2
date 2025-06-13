using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    [ItemCategory("Accessory", "General")]
    internal class OrderEmblem : BaseAccessory
    {
        public static float OrderWeaponDamageBonus => 0.5f;

        protected override string UsageHintParamA => $"{(int)(OrderWeaponDamageBonus * 100)}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValuePreEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<OrderEmblem>();
        }
    }
}
