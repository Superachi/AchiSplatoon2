using AchiSplatoon2.Attributes;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    [ItemCategory("Special", "Specials")]
    internal class BaseSpecial : BaseWeapon
    {
        public override float RechargeCostPenalty { get => 100f; }
        public override bool IsSpecialWeapon { get => true; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(gold: 3);
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
