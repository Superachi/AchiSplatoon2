using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.Minions.PearlDrone;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Buffs
{
    internal class PearlDroneBuff : ModBuff
    {
        // See also https://github.com/tModLoader/tModLoader/blob/stable/ExampleMod/Content/Projectiles/Minions/ExampleSimpleMinion.cs#L21
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PearlDroneMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            var player = Main.LocalPlayer;
            var dronePlayer = player.GetModPlayer<PearlDronePlayer>();
            var colorChipPlayer = player.GetModPlayer<ColorChipPlayer>();
            var accPlayer = player.GetModPlayer<AccessoryPlayer>();
            var summonDamageBonus = (int)((dronePlayer.GetSummonDamageModifier() - 1) * 100);
            var summonDamageFromMinionSlots = (int)(dronePlayer.CountedMinionSlots() * dronePlayer.DamageBonusPerMinionSlot * 100);

            if (!colorChipPlayer.IsPaletteValid()) return;

            // Stat bonus section
            string tooltip = ColorHelper.TextWithPearlColor("Pearl is supporting you!") + "\n";
            tooltip += "Power level:" + ColorHelper.TextWithBonusColor($" {dronePlayer.PowerLevel}") + ColorHelper.TextWithFlavorColor(" (boosts attack damage and flight speed)") + "\n";
            tooltip += "Drone Color Chip attack speed bonus:" + ColorHelper.TextWithBonusColor($" {(int)(colorChipPlayer.CalculateDroneAttackCooldownReduction() * 100)}%") + "\n";
            if (summonDamageBonus > 0)
            {
                tooltip += "Summon damage bonus from gear:" + ColorHelper.TextWithBonusColor($" {summonDamageBonus}%");
                if (summonDamageFromMinionSlots > 0)
                {
                    tooltip += ColorHelper.TextWithBonusColor($" (+{summonDamageFromMinionSlots}% from added minion slots)");
                }
            }
            tooltip += "\n\n";

            // Enabled attacks section
            tooltip += ColorHelper.TextWithPearlColor($"Your {dronePlayer.GetDroneChipCount()} Drone Color Chip(s) enable the following abilities:") + "\n";

            if (accPlayer.HasAccessory<LaserAddon>())
            {
                tooltip += ColorHelper.TextWithFunctionalColor("Laser beams!") + "\n";
            }
            else
            {
                tooltip += ColorHelper.TextWithSubWeaponColor("Sprinkler") + "\n";
            }
            tooltip += ColorHelper.TextWithSubWeaponColor("Life drops") + "\n";

            if (dronePlayer.IsBurstBombEnabled)
            {
                tooltip += ColorHelper.TextWithSubWeaponColor("Burst Bomb") + $" ({dronePlayer.MinimumChipsForBurstBomb}+ chips)" + "\n";
            }

            if (dronePlayer.IsKillerWailEnabled)
            {
                tooltip += ColorHelper.TextWithSpecialWeaponColor("Killer Wail 5.1") + $" ({dronePlayer.MinimumChipsForKillerWail}+ chips)" + "\n";
            }

            tooltip += "\n";

            tooltip += ColorHelper.TextWithPearlColor("Damage dealt by this summon:") + $" {dronePlayer.DamageDealt}" + "\n";

            tip = tooltip;
        }
    }
}
