using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Test
{
    internal class PearlDroneStaff : BaseWeapon
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var tip = "Summons Pearl Drone to your side!";
            tooltips.Add(new TooltipLine(Mod, "PearlSummonA", tip) { OverrideColor = null });

            tip = $"Tip: entering the world with a {TextHelper.ItemEmoji<ColorChipAqua>(true)} in your inventory summons " + ColorHelper.TextWithPearlColor("Pearl") + " automatically";
            tooltips.Add(new TooltipLine(Mod, "PearlSummonB", tip) { OverrideColor = null });
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;

            Item.useTime = 20;
            Item.useAnimation = Item.useTime;

            Item.useStyle = ItemUseStyleID.Swing;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.SetValuePreEvilBosses();
            Item.rare = ItemRarityID.Expert;
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<PearlDronePlayer>().SpawnPearlDroneViaStaff();
            return true;
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
