using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials.Gradients
{
    internal class FireGradientVial : BaseVial
    {
        protected override void ApplyVialEffect(Player player)
        {
            var modPlayer = player.GetModPlayer<InkColorPlayer>();
            modPlayer.SetDualColor(
                InkColorPlayer.IncrementType.AttackBased,
                new Color(255, 50, 0),
                new Color(255, 180, 0));
        }
    }
}
