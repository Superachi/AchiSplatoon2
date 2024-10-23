using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.Minions.PearlDrone;
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
            var weaponPlayer = player.GetModPlayer<InkWeaponPlayer>();
            var summonDamageBonus = (int)((player.GetDamage(DamageClass.Summon).ApplyTo(1) - 1) * 100);

            string tooltip = $"Power level: { dronePlayer.PowerLevel}\n";

            if (weaponPlayer.IsPaletteValid())
            {
                tooltip += $"Attack speed bonus: {(int)((weaponPlayer.CalculateDroneAttackCooldownReduction()) * 100)}%\n";
            }

            if (summonDamageBonus > 0)
            {
                tooltip += $"Summoning damage bonus: {summonDamageBonus}%";
            }

            tip = tooltip;
        }
    }
}
