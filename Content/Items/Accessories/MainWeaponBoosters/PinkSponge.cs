using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class PinkSponge : BaseWeaponBoosterAccessory
    {
        public static float DeflectDamageModifier => 8f;
        public static float DeflectVelocityModifier => 20f;
        public static float ChargeSlashLifetimeModifier => 0.3f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasPinkSponge = true;
            }
        }
    }
}
