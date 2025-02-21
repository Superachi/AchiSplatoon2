using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class EmpressInkTankPlayer : ModPlayer
    {
        public int procCooldown = 0;

        public override void PreUpdate()
        {
            if (procCooldown > 0) procCooldown--;
        }

        public bool CanSpawnProjectile()
        {
            return procCooldown == 0;
        }

        public void ActivateCooldown()
        {
            procCooldown = EmpressInkTank.ProcCooldown;
        }
    }
}
