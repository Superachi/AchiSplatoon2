using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.ExtensionMethods
{
    public static class WoomyExtensions
    {
        public static float NormalizePrefixMod(this float modifier)
        {
            return modifier + 1;
        }

        public static bool HasAccessory<T>(this Player player) where T : ModItem
        {
            return player.GetModPlayer<AccessoryPlayer>().HasAccessory<T>();
        }
    }
}
