using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
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

        public static bool OwnsModProjectileWithType(this Player player, int projectileType)
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                var modProj = projectile.ModProjectile;
                if (modProj == null)
                {
                    continue;
                }

                var projectileTypeToCheck = ModContent.GetModProjectile(projectileType);
                if (projectileTypeToCheck.GetType().IsAssignableFrom(modProj.GetType())
                    && projectile.owner == player.whoAmI)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
