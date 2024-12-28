using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class BaseSpecial : BaseWeapon
    {
        public override float RechargeCostPenalty { get => 100f; }
        public override bool IsSpecialWeapon { get => true; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(gold: 5);
        }

        public override bool CanReforge()
        {
            return false;
        }

        public override bool AllowPrefix(int pre)
        {
            return false;
        }

        public override void AddRecipes() { }
    }
}
