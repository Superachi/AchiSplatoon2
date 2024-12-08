using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ColorChipBase : BaseItem
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
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 8;
        }

        // Order of code -> reseteffects -> update inventory -> update accessory
        public override void UpdateInventory(Player player)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return;
            var modPlayer = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();

            modPlayer.ColorChipAmounts[(int)ChipColor.Red] += RedValue * this.Item.stack;
            modPlayer.ColorChipAmounts[(int)ChipColor.Blue] += BlueValue * this.Item.stack;
            modPlayer.ColorChipAmounts[(int)ChipColor.Yellow] += YellowValue * this.Item.stack;
            modPlayer.ColorChipAmounts[(int)ChipColor.Purple] += PurpleValue * this.Item.stack;
            modPlayer.ColorChipAmounts[(int)ChipColor.Green] += GreenValue * this.Item.stack;
            modPlayer.ColorChipAmounts[(int)ChipColor.Aqua] += AquaValue * this.Item.stack;
        }

        private string StatIncreaseDisplayString(string textColor, string stat, string amount)
        {
            return $"[{textColor}:Increases {stat} by {amount} per chip]";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();
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
                    t.Text += "\n" + StatIncreaseDisplayString(textColor, "shot velocity & accuracy", modPlayer.YellowChipVelocityBonusDisplay);
                }
                else if (PurpleValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "weapon charge up speed", modPlayer.PurpleChipBaseChargeSpeedBonusDisplay);
                    t.Text += "\n" + StatIncreaseDisplayString(textColor, "splat ink recovery", modPlayer.PurpleSplatInkRecoveryBonusDisplay);
                }
                else if (GreenValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "enemy lucky bomb drop chance", modPlayer.GreenChipLuckyBombChanceDisplay);
                    t.Text += $"\n[{textColor}:Per chip, enemies are more likely to drop sub weapons, canned specials and life/mana pickups]";
                }
                else if (AquaValue > 0)
                {
                    t.Text += StatIncreaseDisplayString(textColor, "Pearl Drone's attack speed", modPlayer.AquaChipBaseAttackCooldownReductionDisplay);
                    t.Text += $"\n[{textColor}:Every 2 chips, an additional attack is added (up to 3 new attacks)]";
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

        protected Recipe ColorChipRecipe(int dyeItemId, int gemItemId)
        {
            return CreateRecipe()
                .AddIngredient(ModContent.ItemType<ColorChipEmpty>())
                .AddIngredient(dyeItemId)
                .AddIngredient(gemItemId)
                .AddIngredient(ItemID.MeteoriteBar)
                .AddTile(TileID.Bottles);
        }
    }
}
