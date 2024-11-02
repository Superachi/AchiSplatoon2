using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneBurstBomb : BurstBombProjectile
    {
        private readonly int baseExplosionRadius = new BurstBomb().ExplosionRadius;

        public override void AfterSpawn()
        {
            Initialize();
            finalExplosionRadius = (int)(baseExplosionRadius * explosionRadiusModifier);
            enablePierceDamagefalloff = false;

            PlayAudio("Throwables/SplatBombThrow");
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            new OnHitEffect().ApplyEffect(this, target, damageDone);
        }
    }
}
