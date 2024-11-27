using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Content.Projectiles.TransformProjectiles;
using AchiSplatoon2.Helpers;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class HudPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            ProjectileHelper.CreateProjectile(Player, ModContent.ProjectileType<HudProjectile>());
        }

        public override void PreUpdate()
        {
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<HudProjectile>()] == 0)
            {
                ProjectileHelper.CreateProjectile(Player, ModContent.ProjectileType<HudProjectile>());
            }
        }
    }
}
