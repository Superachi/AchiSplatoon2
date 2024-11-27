using AchiSplatoon2.Content.Projectiles;
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
    }
}
