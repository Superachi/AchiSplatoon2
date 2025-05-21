using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class InkbrushNouveau : Inkbrush
    {
        protected override int ArmorPierce => 30;
        public override float BaseWeaponUseTime => 10f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.DamageType = DamageClass.Melee;
            Item.damage = 16;
            Item.knockBack = 5;
            Item.scale = 1.2f;

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipePalladium();
    }
}
