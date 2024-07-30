using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KrakonSplatRoller : BaseRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 100;
            Item.knockBack = 5;

            Item.shoot = ModContent.ProjectileType<KrakonRollerSwingProjectile>();
        }
    }
}
