using AchiSplatoon2.Content.Players;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class ColorChipPlayerDTO : BaseDTO
    {
        [JsonProperty]
        public int[]? ColorChipAmounts = null;

        public ColorChipPlayerDTO(
            int[]? colorChipAmounts = null)
        {
            ColorChipAmounts = colorChipAmounts;
        }

        public override void ApplyToModPlayer(ModPlayer modPlayer)
        {
            var colorChipPlayer = (ColorChipPlayer)modPlayer;
            if (ColorChipAmounts != null)
            {
                colorChipPlayer.ColorChipAmounts = ColorChipAmounts;
            }

            colorChipPlayer.UpdateInkColor();
        }
    }
}
