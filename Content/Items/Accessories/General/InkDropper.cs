using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    internal class InkDropper : BaseAccessory
    {
        public static int DropletChanceDenominatorOverride => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 14;
            Item.height = 32;
            Item.SetValuePostEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<InkDropper>();
        }
    }
}
