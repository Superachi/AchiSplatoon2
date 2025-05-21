using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials.Gradients
{
    internal class WaterGradientVial : BaseVial
    {
        protected override void ApplyVialEffect(Player player)
        {
            var modPlayer = player.GetModPlayer<InkColorPlayer>();
            modPlayer.SetDualColor(
                InkColorPlayer.IncrementType.AttackBased,
                new Color(0, 255, 240),
                new Color(0, 90, 255));
        }
    }
}
