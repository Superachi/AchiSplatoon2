using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaWiper : BaseSplatana
    {
        public override float MaxChargeRangeDamageMod { get => 2f; }
        public override float MaxChargeLifetimeMod { get => 1.5f; }

        public override int BaseDamage { get => 12; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 4;

            Item.useTime = 20;
            Item.useAnimation = Item.useTime;

            Item.width = 62;
            Item.height = 52;

            Item.SetValuePostEvilBosses();
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
