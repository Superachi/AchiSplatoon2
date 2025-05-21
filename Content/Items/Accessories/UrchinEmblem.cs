using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using AchiSplatoon2.ExtensionMethods;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class UrchinEmblem : BaseAccessory
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(AttackDamage.ToString());

        public static int AttackDamage => 15;
        public static int AttackCooldown => 30;

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
                modPlayer.TryEquipAccessory<UrchinEmblem>();
            }
        }
    }
}
