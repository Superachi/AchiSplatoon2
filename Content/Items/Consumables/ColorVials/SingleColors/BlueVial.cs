using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials.SingleColors
{
    internal class BlueVial : BaseSingleColorVial
    {
        protected override Color ColorToSet => new Color(r: 40, g: 60, b: 255);
    }
}
