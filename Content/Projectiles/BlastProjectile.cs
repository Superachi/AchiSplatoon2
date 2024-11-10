using AchiSplatoon2.Netcode.DataModels;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BlastProjectile : BaseProjectile
    {
        private int blastRadius;
        private PlayAudioModel? playAudioModel;

        public void SetProperties(int radius, PlayAudioModel? audioModel = null)
        {
            blastRadius = radius;
            playAudioModel = audioModel ?? new PlayAudioModel(_soundPath: "BlasterExplosion", _volume: 0.3f, _pitchVariance: 0.1f, _maxInstances: 3);
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 6;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            enablePierceDamagefalloff = false;

            if (IsThisClientTheProjectileOwner())
            {
                var finalRadius = (int)(blastRadius * explosionRadiusModifier);
                var e = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                CreateExplosionVisual(e, playAudioModel);

                Projectile.Resize(finalRadius, finalRadius);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = Math.Sign(target.position.X - GetOwner().position.X);
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}
