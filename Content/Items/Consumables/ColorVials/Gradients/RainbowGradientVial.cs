using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials.Gradients
{
    internal class RainbowGradientVial : BaseVial
    {
        protected override void ApplyVialEffect(Player player)
        {
            var modPlayer = player.GetModPlayer<InkColorPlayer>();
            modPlayer.ApplyRainbowColors();
        }
    }
}
