using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    [ItemCategory("Accessory", "General")]
    internal class DarkMagnet : BaseAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValuePostEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<DarkMagnet>();
        }
    }
}
