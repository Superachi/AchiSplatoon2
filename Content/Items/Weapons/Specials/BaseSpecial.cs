using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var accMP = Main.LocalPlayer.GetModPlayer<InkAccessoryPlayer>();
                damage *= accMP.specialPowerMultiplier;
            }
        }
    }
}
