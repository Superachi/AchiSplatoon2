using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class SorellaSplatBrella : SplatBrella
    {
        public override int ShieldLife => 400;
        public override int ShieldCooldown => 450;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 28;
            Item.knockBack = 3;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.Register();
        }
    }
}
