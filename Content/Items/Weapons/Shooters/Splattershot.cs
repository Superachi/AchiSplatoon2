using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class Splattershot : BaseSplattershot
    {
        public override float ShotGravity { get => 0.35f; }
        public override int ShotGravityDelay => 15;
        public override int ShotExtraUpdates { get => 4; }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.useTime = 7;
            Item.useAnimation = Item.useTime;

            Item.damage = 10;
            Item.width = 42;
            Item.height = 26;
            Item.knockBack = 1;
            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
