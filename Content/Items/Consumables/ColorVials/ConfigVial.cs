using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials
{
    internal class ConfigVial : BaseVial
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 32;
        }

        protected override void ApplyVialEffect(Player player)
        {
            var modPlayer = player.GetModPlayer<InkColorPlayer>();
            modPlayer.ApplyConfigColors();
        }
    }
}
