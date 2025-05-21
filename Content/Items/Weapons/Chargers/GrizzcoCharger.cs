using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ChargerProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    [ItemCategory("Charger", "Chargers")]
    internal class GrizzcoCharger : BaseWeapon
    {
        public override float InkCost { get => 5f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Charger;

        public override SoundStyle ShootSample { get => SoundPaths.BambooChargerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.BambooChargerShootWeak.ToSoundStyle(); }
        public override Vector2 MuzzleOffset => new Vector2(80f, 0);
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.channel = false;   // It instantly charges, so as a QoL it can just automatically fire without channeling

            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<GrizzcoChargerProjectile>(),
                singleShotTime: 15,
                shotVelocity: 2f);

            Item.width = 82;
            Item.height = 30;
            Item.damage = 140;
            Item.knockBack = 0;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeGrizzco(ModContent.ItemType<BambooMk1Charger>());

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
