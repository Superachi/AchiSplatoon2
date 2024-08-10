using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class KensaOctobrush : Octobrush
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.damage = 72;
            Item.knockBack = 6;
            Item.scale = 2.0f;
            Item.useTime = 15;
            Item.useAnimation = Item.useTime;

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
