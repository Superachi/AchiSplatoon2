using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    internal class FestivePopper : BaseAccessory
    {
        public static int BaseBlastRadius => 250;
        public static int PreHMDamage => 40;
        public static int PostHMDamage => 80;
        public static int PostPlanteraDamage => 120;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 18;
            Item.SetValuePostEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<FestivePopper>();
        }
    }
}
