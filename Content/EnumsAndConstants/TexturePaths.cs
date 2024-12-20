using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.EnumsAndConstants
{
    internal static class TexturePaths
    {
        private static readonly string _textureRootPath = $"AchiSplatoon2/Content/Assets/Textures/";

        public static Texture2D ToTexture2D(this string texturePath)
        {
            return ModContent.Request<Texture2D>(texturePath).Value;
        }

        private static string FormatPath(string filename, string directory = "")
        {
            var path = Path.Combine(_textureRootPath, directory, filename);
            return path;
        }

        // Root directory
        public static string VortexDualieShot => FormatPath(nameof(VortexDualieShot));
        public static string NebulaStringerShot => FormatPath(nameof(NebulaStringerShot));

        // Specials
        private static readonly string _directorySpecials = "Specials";
        public static string ZookaStages => FormatPath(nameof(ZookaStages), _directorySpecials);

        // UI
        private static readonly string _directoryUI = "UI";

        // UI - InkTank
        private static readonly string _directoryInkTank = Path.Combine(_directoryUI, "InkTank");
        public static string InkTankBack => FormatPath(nameof(InkTankBack), _directoryInkTank);
        public static string InkTankFront => FormatPath(nameof(InkTankFront), _directoryInkTank);

        // UI - SpecialCharge
        private static readonly string _directorySpecialCharge = Path.Combine(_directoryUI, "SpecialCharge");
        public static string SpecialFill => FormatPath(nameof(SpecialFill), _directorySpecialCharge);
        public static string SpecialFillEmpty => FormatPath(nameof(SpecialFillEmpty), _directorySpecialCharge);
        public static string SpecialFrame => FormatPath(nameof(SpecialFrame), _directorySpecialCharge);
        public static string SpecialFrameInvisible => FormatPath(nameof(SpecialFrameInvisible), _directorySpecialCharge);

        // UI - WeaponCharge
        private static readonly string _directoryWeaponCharge = Path.Combine(_directoryUI, "WeaponCharge");
        public static string ChargeUpBar => FormatPath(nameof(ChargeUpBar), _directoryWeaponCharge);
    }
}
