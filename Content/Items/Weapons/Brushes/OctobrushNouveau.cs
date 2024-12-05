using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class OctobrushNouveau : Octobrush
    {
        protected override int ArmorPierce => 15;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.knockBack = 6;
            Item.scale = 1.5f;

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeMythril();
    }
}
