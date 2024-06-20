namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class BaseSpecial : BaseWeapon
    {
        public override bool IsSpecialWeapon { get => true; }
        public override bool AllowSubWeaponUsage { get => false; }
    }
}
