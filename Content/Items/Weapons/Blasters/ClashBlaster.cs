using AchiSplatoon2.Content.Projectiles.BlasterProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Blasters
{
    internal class ClashBlaster : Blaster
    {
        public override float InkCost { get => 3.5f; }

        // Explosion radius and delay
        public override int ExplosionRadiusAir { get => 280; }
        public override float ExplosionDelayInit { get => 10f; }

        public override Vector2? HoldoutOffset() { return new Vector2(-10, 0); }
        public override Vector2 MuzzleOffset => new Vector2(46f, 0);

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BlasterProjectile>(),
                singleShotTime: 22,
                shotVelocity: 5.5f);

            Item.damage = 24;
            Item.width = 44;
            Item.height = 36;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipeHellstone();
    }
}
