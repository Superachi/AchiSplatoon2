using AchiSplatoon2.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class SplooshOMatic : BaseSplattershot
    {
        public override string ShootSample => "SplatlingShoot";
        public override float MuzzleOffsetPx { get; set; } = 62f;
        public override Vector2? HoldoutOffset() { return new Vector2(-10, 0); }
        public override SubWeaponType BonusSub { get => SubWeaponType.AngleShooter; }
        public override SubWeaponBonusType BonusType { get => SubWeaponBonusType.Damage; }
        public override float ShotGravity => 0.03f;
        public override float AimDeviation { get => 8f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<SplattershotProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 5,
                shotVelocity: 5f);

            Item.damage = 40;
            Item.width = 60;
            Item.height = 32;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
        }
    }
}
