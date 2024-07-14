using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class JetSquelcher : BaseSplattershot
    {
        public override string ShootSample { get => "JetSquelcherShoot"; }
        public override int ShotGravityDelay => 20;
        public override int ShotExtraUpdates { get => 8; }
        public override float MuzzleOffsetPx { get; set; } = 56f;
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 4); }
        public override float AimDeviation { get => 2f; }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 30;
            Item.width = 64;
            Item.height = 36;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(registerNow: false);
            recipe.AddIngredient(ItemID.TitaniumBar, 5);
            recipe.Register();
        }
    }
}
