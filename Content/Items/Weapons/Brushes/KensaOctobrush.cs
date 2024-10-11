using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class KensaOctobrush : Octobrush
    {
        public override float AimDeviation { get => 3f; }
        public override float DelayUntilFall => 15f;
        protected override int ArmorPierce => 20;


        // Brush-specific properties
        public override float ShotVelocity => 10f;
        public override float BaseWeaponUseTime => 12f;
        public override int SwingArc => 150;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.damage = 66;
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
