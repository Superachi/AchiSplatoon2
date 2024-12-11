using AchiSplatoon2.Content.Players;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class OnHitEffect
    {
        public void ApplyEffect(BaseProjectile callingProjectile, NPC target, int damageDone)
        {
            var pdMP = Main.LocalPlayer.GetModPlayer<PearlDronePlayer>();
            var parent = callingProjectile.parentProjectile;

            if (parent != null && parent.ModProjectile is PearlDroneMinion)
            {
                if (target.life <= 0)
                {
                    pdMP.TriggerDialoguePearlKillsNpc(target);
                }

                pdMP.AddDamageDealtStatistic(damageDone);
            }
            else
            {
                pdMP.TriggerDialoguePlayerKillsNpc(target);
            }
        }
    }
}
