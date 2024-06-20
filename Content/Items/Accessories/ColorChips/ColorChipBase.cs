using AchiSplatoon2.Content.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipBase : ModItem
    {
        protected virtual int RedValue { get => 0; }
        protected virtual int BlueValue { get => 0; }
        protected virtual int YellowValue { get => 0; }
        protected virtual int PurpleValue { get => 0; }
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

        // Order of code -> reseteffects -> update inventory -> update accessory
        public override void UpdateInventory(Player player)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

            modPlayer.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Red] += RedValue;
            modPlayer.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Blue] += BlueValue;
            modPlayer.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Yellow] += YellowValue;
            modPlayer.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Purple] += PurpleValue;
            modPlayer.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Green] += GreenValue;
            modPlayer.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Aqua] += AquaValue;
        }

        private string StatIncreaseDisplayString(string textColor, string stat, string amount)
        {
            return $"[{textColor}:Increases {stat} by {amount} per chip]";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            int index = tooltips.FindIndex(l => l.Name == "ItemName");
            if (index != -1)
            {
                var t = tooltips[index];
                t.Text = $"{Item.Name}";

                var textColor = "c/ffffff";

                t.Text += $"\n[c/ff8e2c:Effect when active:]\n";
                if (RedValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "weapon damage", modPlayer.RedChipBaseAttackDamageBonusDisplay);
                    t.Text += "\n" + StatIncreaseDisplayString(textColor, "armor penetration", modPlayer.RedChipBaseArmorPierceBonusDisplay);
                }
                else if (BlueValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "movement speed", modPlayer.BlueChipBaseMoveSpeedBonusDisplay);
                    t.Text += "\n" + StatIncreaseDisplayString(textColor, "special charge up while moving", modPlayer.BlueChipBaseChargeBonusDisplay);
                }
                else if (YellowValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "explosion radius", modPlayer.YellowChipExplosionRadiusBonusDisplay);
                    t.Text += "\n" + StatIncreaseDisplayString(textColor, "projectile piercing", modPlayer.YellowChipPiercingBonusDisplay);
                }
                else if (PurpleValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "weapon charge up speed", modPlayer.PurpleChipBaseChargeSpeedBonusDisplay);
                    t.Text += "\n" + StatIncreaseDisplayString(textColor, "knockback", modPlayer.PurpleChipBaseKnockbackBonusDisplay);
                }
                else if (GreenValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "critical strike chance", modPlayer.GreenChipBaseCritBonusDisplay);
                }
                else if (AquaValue > 0)
                {
                    //
                }

                if (!modPlayer.isPaletteEquipped)
                {
                    t.Text += $"\n[c/ed3a4a:Currently inactive. Requires a Color Palette accessory to activate!]";
                }
                else if (modPlayer.DoesPlayerHaveTooManyChips())
                {
                    t.Text += $"\n[c/ed3a4a:You are carrying too many Color Chips, so the listed effect is disabled.]";
                }
            }
        }
    }
}
