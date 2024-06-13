using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Players;
using System.Collections.Generic;

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
                if (modPlayer.isPaletteEquipped)
                {
                    if (modPlayer.DoesPlayerHaveTooManyChips())
                    {
                        t.Text += $"\n[c/a8a8a8:Currently inactive. You have too many Color Chips equipped.]";
                        return;
                    }

                    var textColor = "c/ffffff";
                    t.Text += $"\n[c/ff8e2c:Palette effect: ]";
                    if (RedValue > 0)
                    {
                        t.Text += StatIncreaseDisplayString(textColor, "attack speed", modPlayer.RedChipBaseAttackSpeedBonusDisplay);
                    }
                    else if (BlueValue > 0)
                    {
                        t.Text += StatIncreaseDisplayString(textColor, "movement speed", modPlayer.BlueChipBaseMoveSpeedBonusDisplay);
                    }
                    else if (YellowValue > 0)
                    {
                        t.Text += StatIncreaseDisplayString(textColor, "explosion radius", modPlayer.YellowChipExplosionRadiusBonusDisplay);
                    }
                    else if (PurpleValue > 0)
                    {
                        t.Text += StatIncreaseDisplayString(textColor, "weapon charge up speed", modPlayer.PurpleChipBaseChargeSpeedBonusDisplay);
                        t.Text += "\n"+StatIncreaseDisplayString(textColor, "knockback", modPlayer.PurpleChipBaseKnockbackBonusDisplay);
                    }
                    else if (GreenValue > 0)
                    {
                        t.Text += StatIncreaseDisplayString(textColor, "critical strike chance", modPlayer.GreenChipBaseCritBonusDisplay);
                    }
                    else if (AquaValue > 0)
                    {
                        t.Text += StatIncreaseDisplayString(textColor, "vibes", modPlayer.RedChipBaseAttackSpeedBonusDisplay);
                    }
                }
                else
                {
                    t.Text += $"\n[c/a8a8a8:Currently inactive. Requires an Order Palette to activate!]";
                }
            }
        }
    }
}
