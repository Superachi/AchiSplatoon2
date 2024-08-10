using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.ChargerProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class StarfishedCharger : BaseCharger
    {
        public override string ShootSample { get => "BambooChargerShoot"; }
        public override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        public override bool ScreenShake => false;
        public override Vector2? HoldoutOffset() { return new Vector2(-20, 2); }
        public override float MuzzleOffsetPx { get; set; } = 60f;
        public override float[] ChargeTimeThresholds { get => [36f]; }
        public override int MaxPenetrate => 5;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<StarfishedChargerProjectile>(),
                singleShotTime: 15,
                shotVelocity: 18f);
            Item.damage = 180;
            Item.width = 86;
            Item.height = 24;
            Item.knockBack = 0;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
