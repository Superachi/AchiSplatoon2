using AchiSplatoon2.Netcode.DataModels;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class GrizzcoDualieBlastProjectile : BaseProjectile
    {
        protected string explosionSample = "BlasterExplosion";
        private int baseRadius = 220;
        private bool hasExploded = false;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 6;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void AfterSpawn()
        {
            Initialize();
            enablePierceDamagefalloff = false;

            if (IsThisClientTheProjectileOwner())
            {
                var finalRadius = (int)(baseRadius * explosionRadiusModifier);
                var e = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                var a = new PlayAudioModel(_soundPath: explosionSample, _volume: 0.3f, _pitchVariance: 0.1f, _maxInstances: 3, _pitch: 0);
                CreateExplosionVisual(e, a);

                Projectile.Resize(finalRadius, finalRadius);
                hasExploded = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
            modifiers.Knockback *= 2;
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}
