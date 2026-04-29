using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    [ItemCategory("Accessory", "General")]
    internal class SanitizedSample : BaseAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 24;
            Item.height = 24;
            Item.SetValuePostEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<SanitizedSample>();
        }
    }
}
