using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneBurstBomb : BurstBombProjectile
    {
        private int baseExplosionRadius = 200;

        public override void AfterSpawn()
        {
            Initialize();
            finalExplosionRadius = (int)(baseExplosionRadius * explosionRadiusModifier);
            enablePierceDamagefalloff = false;

            PlayAudio("Throwables/SplatBombThrow");
        }
    }
}
