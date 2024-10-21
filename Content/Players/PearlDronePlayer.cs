using AchiSplatoon2.Content.Projectiles.Minions.PearlDrone;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class PearlDronePlayer : BaseModPlayer
    {
        public bool DoesPlayerHavePearlDrone()
        {
            return Player.ownedProjectileCounts[ModContent.ProjectileType<PearlDroneMinion>()] > 0;
        }

        public PearlDroneMinion? GetPlayerDrone()
        {
            if (!DoesPlayerHavePearlDrone()) return null;

            foreach(Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.type == ModContent.ProjectileType<PearlDroneMinion>() && projectile.owner == Player.whoAmI)
                {
                    return (PearlDroneMinion)projectile.ModProjectile;
                }
            }

            return null;
        }

        public void TriggerDialoguePlayerKillsNpc(NPC npc)
        {
            var drone = GetPlayerDrone();
            if (drone == null) return;

            drone.TriggerDialoguePlayerKillsNpc(npc);
        }

        public void TriggerDialoguePearlKillsNpc(NPC npc)
        {
            var drone = GetPlayerDrone();
            if (drone == null) return;

            drone.TriggerDialoguePearlKillsNpc(npc);
        }
    }
}
