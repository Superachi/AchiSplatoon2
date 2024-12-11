using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Chat;

namespace AchiSplatoon2.ExtensionMethods
{
    public static class WoomyExtensions
    {
        public static float NormalizePrefixMod(this float modifier)
        {
            return modifier + 1;
        }
    }
}
