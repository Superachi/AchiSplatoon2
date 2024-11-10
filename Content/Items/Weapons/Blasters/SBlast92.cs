using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class SBlast92 : Blaster
    {
        public override string ShootAltSample { get => "SBlastShoot"; }
        public override int ExplosionRadiusAir { get => 240; }
        public override float ExplosionDelayInit { get => 12f; }

        // Sprite offset
        public override Vector2? HoldoutOffset() { return new Vector2(-12, -2); }
        public override float MuzzleOffsetPx { get; set; } = 64f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SBlastProjectile>(),
                singleShotTime: 52,
                shotVelocity: 7f);

            Item.damage = 170;
            Item.width = 60;
            Item.height = 32;
            Item.knockBack = 7;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofSight);
    }
}
