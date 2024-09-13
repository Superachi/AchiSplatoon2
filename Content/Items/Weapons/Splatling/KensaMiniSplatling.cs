using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class KensaMiniSplatling : MiniSplatling
    {
        public override float BarrageVelocity { get; set; } = 9f;
        public override int BarrageShotTime { get; set; } = 3;
        public override int BarrageMaxAmmo { get; set; } = 24;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 46;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
