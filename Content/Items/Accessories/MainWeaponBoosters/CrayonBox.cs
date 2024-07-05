using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class CrayonBox : BaseAccessory
    {
        public static float BarrageLengthMod = 2f;
        public static float SpreadOffsetMod = 0.5f;
        public static int ArmorPenetration = 8;
        protected override string UsageHintParamA => ArmorPenetration.ToString();

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasCrayonBox = true;
            }
        }
    }
}
