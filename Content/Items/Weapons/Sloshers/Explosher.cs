using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class Explosher : BaseSlosher
    {
        public override float InkCost { get => 20f; }

        public override float ShotGravity { get => 0.4f; }
        public int ShotGravityDelay => 30;
        public int ShotExtraUpdates => 3;

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<ExplosherProjectile>(),
                singleShotTime: 56,
                shotVelocity: 8f
            );

            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 30;
            Item.damage = 240;
            Item.knockBack = 8;

            Item.SetValuePostPlantera();
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<WeaponPlayer>().CustomWeaponCooldown = Item.useTime;
            return base.UseItem(player);
        }
    }
}
