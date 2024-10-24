using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneSprinklerProjectile : SprinklerProjectile
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            new OnHitEffect().ApplyEffect(this, target, damageDone);
        }
    }
}
