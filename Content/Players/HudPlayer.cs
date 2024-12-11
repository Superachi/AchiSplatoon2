using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class HudPlayer : ModPlayer
    {
        private HudProjectile? _hudProjectile;

        public override void OnEnterWorld()
        {
            _hudProjectile = (HudProjectile)ProjectileHelper.CreateProjectile(Player, ModContent.ProjectileType<HudProjectile>());
        }

        public override void PreUpdate()
        {
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<HudProjectile>()] == 0)
            {
                _hudProjectile = (HudProjectile)ProjectileHelper.CreateProjectile(Player, ModContent.ProjectileType<HudProjectile>());
            }
        }

        /// <summary>
        /// Sets the text that's briefly displayed above the player.
        /// </summary>
        public void SetOverheadText(string text, int displayTime = 60, Color? color = null)
        {
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.type == ModContent.ProjectileType<HudProjectile>()
                    && proj.owner == Player.whoAmI
                    && proj.ModProjectile is HudProjectile hudProj)
                {
                    hudProj.SetOverheadText(text, displayTime, color);
                }
            }
        }

        public bool IsTextActive()
        {
            if (_hudProjectile == null) return true;
            return _hudProjectile.IsTextActive();
        }
    }
}
