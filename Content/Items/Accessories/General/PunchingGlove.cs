using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    internal class PunchingGlove : BaseAccessory
    {
        public static float SplashdownDamageBonus => 0.2f;

        protected override string UsageHintParamA => $"{(int)(SplashdownDamageBonus * 100)}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValueHighHardmodeOre();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<PunchingGlove>();
        }
    }
}
