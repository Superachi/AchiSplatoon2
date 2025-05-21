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
        public static string GolemSlashSegment => FormatPath(nameof(GolemSlashSegment));
        public static string LargeCrescentSlash => FormatPath(nameof(LargeCrescentSlash));
        public static string MarkedDot => FormatPath(nameof(MarkedDot));

        // Specials
        private static readonly string _directorySpecials = "Specials";
        public static string ZookaStages => FormatPath(nameof(ZookaStages), _directorySpecials);
        public static string BombLauncherBackpack => FormatPath(nameof(BombLauncherBackpack), _directorySpecials);

        // Glows
        private static readonly string _directoryGlows = "Glows";
        public static string Glow100x => FormatPath(nameof(Glow100x), _directoryGlows);

        // Sparkles
        private static readonly string _directorySparkles = "Sparkles";
        public static string Medium4pSparkle => FormatPath(nameof(Medium4pSparkle), _directorySparkles);
        public static string Medium8pSparkle => FormatPath(nameof(Medium8pSparkle), _directorySparkles);
        public static string MediumCircle => FormatPath(nameof(MediumCircle), _directorySparkles);

        //
        private static readonly string _directorySlots = "Slots";
        public static string InkTankSlot => FormatPath(nameof(InkTankSlot), _directorySlots);
        public static string PaletteSlot => FormatPath(nameof(PaletteSlot), _directorySlots);

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
