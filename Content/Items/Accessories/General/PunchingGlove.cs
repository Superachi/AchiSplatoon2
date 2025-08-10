using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    [ItemCategory("Accessory", "General")]
    internal class PunchingGlove : BaseAccessory
    {
        public static int SplashdownDamageBonus => 100;

        protected override string UsageHintParamA => $"{SplashdownDamageBonus}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.SetValuePostEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<PunchingGlove>();
        }
    }
}
