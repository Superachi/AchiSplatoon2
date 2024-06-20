using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class BaseSpecial : BaseWeapon
    {
        public override bool IsSpecialWeapon { get => true; }
        public override bool AllowSubWeaponUsage { get => false; }

        public override void SetDefaults()
        {
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
    }
}
