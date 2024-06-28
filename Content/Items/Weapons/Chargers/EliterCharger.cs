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
    internal class EliterCharger : SplatCharger
    {
        public override string ShootSample { get => "EliterChargerShoot"; }
        public override string ShootWeakSample { get => "EliterChargerShootWeak"; }
        public override float[] ChargeTimeThresholds { get => [75f]; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 92;
            Item.height = 32;
            Item.damage = 500;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(gold: 25);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
