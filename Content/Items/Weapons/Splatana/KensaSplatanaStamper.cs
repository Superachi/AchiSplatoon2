using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class KensaSplatanaStamper : SplatanaStamper
    {
        public override int BaseDamage { get => 70; }
        public override float[] ChargeTimeThresholds { get => [18f]; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 6;

            Item.SetValuePostPlantera();
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
