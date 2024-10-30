using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class SpookyBrush : BaseBrush
    {
        public override float AimDeviation { get => 0f; }
        protected override int ArmorPierce => 20;


        // Brush-specific properties
        public override float BaseWeaponUseTime => 18f;
        public override int SwingArc => 120;
        public override float RollMoveSpeedBonus => 1.7f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.damage = 80;
            Item.crit = 5;
            Item.knockBack = 6;

            Item.width = 60;
            Item.height = 58;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Lime;
        }
    }
}
