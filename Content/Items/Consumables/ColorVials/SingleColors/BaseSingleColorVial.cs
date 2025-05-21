using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials.SingleColors
{
    internal class BaseSingleColorVial : BaseVial
    {
        public virtual Color ColorToSet => Color.White;

        protected override void ApplyVialEffect(Player player)
        {
            var inkColorPlayer = player.GetModPlayer<InkColorPlayer>();
            inkColorPlayer.SetSingleColor(ColorToSet);
        }
    }
}
