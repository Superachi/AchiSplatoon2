using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaWiper : BaseSplatana
    {
        public override float MaxChargeMeleeDamageMod { get => 3f; }
        public override float MaxChargeRangeDamageMod { get => 2f; }
        public override float MaxChargeLifetimeMod { get => 1.5f; }

        public override int BaseDamage { get => 10; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 4;

            Item.useTime = 15;
            Item.useAnimation = Item.useTime;

            Item.width = 62;
            Item.height = 52;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
