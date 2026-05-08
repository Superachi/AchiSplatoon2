using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials.Conditional
{
    internal class BiomeVial : BaseVial
    {
        protected override void ApplyVialEffect(Player player)
        {
            var modPlayer = player.GetModPlayer<InkColorPlayer>();
            modPlayer.ApplyBiomeColors();
        }
    }
}
