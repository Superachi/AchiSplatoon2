using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    [ItemCategory("Color chips", "ColorChips")]
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

        private void StatIncreaseDisplayString(List<TooltipLine> tooltips, string tooltipName, string stat, string amount)
        {
            stat = ColorHelper.TextWithFunctionalColor(stat);
            amount = ColorHelper.TextWithFunctionalColor(amount);
            var @string = $"Increases {stat} by {amount} per chip";

            var newTooltip = new TooltipLine(Mod, tooltipName, @string);
            tooltips.Add(newTooltip);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (Main.LocalPlayer == null)
            {
                return;
            }

            var modPlayer = player.GetModPlayer<ColorChipPlayer>();

            var statModifierA = "statModifierA";
            var statModifierB = "statModifierB";

            if (!modPlayer.isPaletteEquipped)
            {
                var newTooltip = new TooltipLine(Mod, statModifierB, ColorHelper.TextWithErrorColor("Currently inactive. Requires an equipped Color Palette to activate!"));
                tooltips.Add(newTooltip);

                return;
            }
            else if (modPlayer.DoesPlayerHaveTooManyChips())
            {
                var newTooltip = new TooltipLine(Mod, statModifierB, ColorHelper.TextWithErrorColor("You are carrying too many Color Chips, so the listed effect is disabled."));
                tooltips.Add(newTooltip);
            }

            if (RedValue > 0)
            {
                StatIncreaseDisplayString(tooltips, statModifierA, "sub weapon damage", modPlayer.RedChipBaseSubWeaponDamageBonusDisplay);
                StatIncreaseDisplayString(tooltips, statModifierB, "armor penetration", modPlayer.RedChipBaseArmorPierceBonusDisplay);
            }
            else if (BlueValue > 0)
            {
                StatIncreaseDisplayString(tooltips, statModifierA, "movement speed", modPlayer.BlueChipBaseMoveSpeedBonusDisplay);
                StatIncreaseDisplayString(tooltips, statModifierB, "special charge while moving with roller & brush", modPlayer.BlueChipBaseChargeBonusDisplay);
            }
            else if (YellowValue > 0)
            {
                StatIncreaseDisplayString(tooltips, statModifierA, "explosion radius", modPlayer.YellowChipExplosionRadiusBonusDisplay);
                StatIncreaseDisplayString(tooltips, statModifierB, "shot velocity & accuracy", modPlayer.YellowChipVelocityBonusDisplay);
            }
            else if (PurpleValue > 0)
            {
                StatIncreaseDisplayString(tooltips, statModifierA, "weapon charge up speed", modPlayer.PurpleChipBaseChargeSpeedBonusDisplay);
                StatIncreaseDisplayString(tooltips, statModifierB, "ink recovery on special charge gain", modPlayer.PurpleSplatInkRecoveryBonusDisplay);
            }
            else if (GreenValue > 0)
            {
                StatIncreaseDisplayString(tooltips, statModifierA, "critical strike chance", modPlayer.GreenChipCritChanceBonusDisplay);
                StatIncreaseDisplayString(tooltips, statModifierB, "enemy lucky bomb drop chance", modPlayer.GreenChipLuckyBombChanceDisplay);
            }
            else if (AquaValue > 0)
            {
                StatIncreaseDisplayString(tooltips, statModifierA, "Pearl Drone's attack speed", modPlayer.AquaChipBaseAttackCooldownReductionDisplay);

                var newTooltip = new TooltipLine(Mod, statModifierB, "As you equip more chips, Pearl Drone unlocks new abilities");
                tooltips.Add(newTooltip);
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
