using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class KensaReefluxStringer : ReefluxStringer
    {
        public override float[] ChargeTimeThresholds { get => [15f, 30f]; }
        public override float ShotgunArc { get => 4.5f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 98;
            Item.crit = 15;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
