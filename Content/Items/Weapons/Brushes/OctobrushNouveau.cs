using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Brushes
{
    internal class OctobrushNouveau : Octobrush
    {
        protected override int ArmorPierce => 15;
        public override float BaseWeaponUseTime => 15f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            SetItemUseTime();

            Item.DamageType = DamageClass.Melee;
            Item.damage = 26;
            Item.knockBack = 6;
            Item.scale = 1.5f;

            Item.SetValueMidHardmodeOre();
        }

        public override void AddRecipes() => AddRecipeMythril();
    }
}
