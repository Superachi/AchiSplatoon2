using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class TriStringerInkline : TriStringer
    {
        public override float ShotgunArc { get => 8f; }
        public override int ProjectileCount { get => 3; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 80;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeMythril();
    }
}
