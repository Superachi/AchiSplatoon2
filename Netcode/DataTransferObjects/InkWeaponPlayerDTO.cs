using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class InkWeaponPlayerDTO
    {
        public bool SpecialReady;
        public Color InkColor;

        public InkWeaponPlayerDTO(bool specialReady, Color inkColor)
        {
            SpecialReady = specialReady;
            InkColor = inkColor;
        }

        public string CreateDTOSummary()
        {
            string desc = "DTO Summary:";
            desc += $"\nSpecialReady={SpecialReady}";
            desc += $"\nInkColor={InkColor}";
            return desc;
        }
    }
}
