using AchiSplatoon2.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Projectiles.StringerProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class GrizzcoCharger : BaseWeapon
    {
        public override string ShootSample { get => "BambooChargerShoot"; }
        public override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        public override float MuzzleOffsetPx { get; set; } = 80f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.channel = false;   // It instantly charges, so as a QoL it can just automatically fire without channeling

            Item.DefaultToRangedWeapon(
                baseProjType: ModContent.ProjectileType<GrizzcoChargerProjectile>(),
                ammoID: AmmoID.None,
                singleShotTime: 15,
                shotVelocity: 2f);

            Item.width = 82;
            Item.height = 30;
            Item.damage = 180;
            Item.knockBack = 0;
            Item.value = Item.buyPrice(gold: 40);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BambooMk1Charger>(), 1);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
