using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class CrayonBox : BaseWeaponBoosterAccessory
    {
        public static float ShotVelocityMod = 1.1f;
        public static float SpreadOffsetMod = 0.75f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.hasCrayonBox = true;
            }
        }
    }
}
