using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class SubSaverEmblem : MainSaverEmblem
    {
        public override float InkSaverAmount()
        {
            return 0.3f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<SubSaverEmblem>();
        }
    }
}
