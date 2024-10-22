using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;

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
    }
}
