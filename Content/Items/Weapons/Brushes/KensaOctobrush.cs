using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class KensaOctobrush : Octobrush
    {
        public override float AimDeviation { get => 3f; }
        protected override int ArmorPierce => 20;


        // Brush-specific properties
        public override float BaseWeaponUseTime => 12f;
        public override int SwingArc => 150;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.DamageType = DamageClass.Melee;
            Item.damage = 40;
            Item.knockBack = 7;
            Item.scale = 2.0f;

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
