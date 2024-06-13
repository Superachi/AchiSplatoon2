using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipBase : ModItem
    {
        // Damage
        protected virtual int RedValue { get => 0; }

        // Move speed
        protected virtual int BlueValue { get => 0; }

        // Pierce? or proj velocity
        protected virtual int YellowValue { get => 0; }

        // Knockback
        protected virtual int PurpleValue { get => 0; }

        // Crit and lucky bomb?
        protected virtual int GreenValue { get => 0; }
        protected virtual int AquaValue { get => 0; }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ItemRarityID.Blue;
            Item.stack = 1;
        }

        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<InkWeaponPlayer>().ColorChipRedAmount += RedValue;
            player.GetModPlayer<InkWeaponPlayer>().ColorChipBlueAmount += BlueValue;
            player.GetModPlayer<InkWeaponPlayer>().ColorChipYellowAmount += YellowValue;
            player.GetModPlayer<InkWeaponPlayer>().ColorChipPurpleAmount += PurpleValue;
            player.GetModPlayer<InkWeaponPlayer>().ColorChipGreenAmount += GreenValue;
            player.GetModPlayer<InkWeaponPlayer>().ColorChipAquaAmount += AquaValue;

            player.GetDamage(DamageClass.Generic) += (float)RedValue / 20f;
            player.GetKnockback(DamageClass.Generic) += (float)PurpleValue / 5f;
            player.GetCritChance(DamageClass.Generic) += (float)GreenValue * 3;
            player.moveSpeed += (float)BlueValue / 5f;
        }
    }
}
