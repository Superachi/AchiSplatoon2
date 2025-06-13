using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Attributes;

namespace AchiSplatoon2.Content.Items.Accessories.RushAttacks
{
    [ItemCategory("Accessory", "RushAttacks")]
    internal class SpinEmblem : BaseAccessory
    {
        public static int DefaultDamage => 20;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 30;
            Item.height = 30;
            Item.SetValuePreEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.TryEquipAccessory<SpinEmblem>();
            }
        }
    }
}
