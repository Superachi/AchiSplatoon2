using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class KensaSplatanaStamper : SplatanaStamper
    {
        public override int BaseDamage { get => 60; }
        public override float[] ChargeTimeThresholds { get => [12f]; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.crit = 10;
            Item.knockBack = 6;

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
