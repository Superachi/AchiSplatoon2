using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.Minions.PearlDrone;

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
    }
}
