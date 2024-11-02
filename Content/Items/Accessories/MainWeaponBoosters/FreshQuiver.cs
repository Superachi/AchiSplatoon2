using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class FreshQuiver : BaseWeaponBoosterAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasFreshQuiver = true;
            }
        }
    }
}
