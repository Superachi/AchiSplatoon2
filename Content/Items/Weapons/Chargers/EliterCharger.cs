using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class EliterCharger : BaseCharger
    {
        public override string ShootSample { get => "EliterChargerShoot"; }
        public override string ShootWeakSample { get => "EliterChargerShootWeak"; }
        public override float[] ChargeTimeThresholds { get => [75f]; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DefaultToRangedWeapon(ModContent.ProjectileType<SplatChargerProjectile>(), AmmoID.None, 20, 12f);
            Item.width = 92;
            Item.height = 32;
            Item.damage = 480;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(gold: 25);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SplatCharger>());
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
