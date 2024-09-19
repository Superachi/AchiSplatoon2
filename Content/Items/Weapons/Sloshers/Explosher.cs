using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class Explosher : BaseSlosher
    {
        public override float ShotGravity { get => 0.4f; }
        public int ShotGravityDelay => 30;
        public int ShotExtraUpdates => 3;
        public override float SubWeaponDamageBonus => 1f;

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
            Item.damage = 280;
            Item.knockBack = 8;

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<InkWeaponPlayer>().CustomWeaponCooldown = Item.useTime;
            return base.UseItem(player);
        }
    }
}
