using AchiSplatoon2.Netcode.DataModels;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class FreshQuiverBlast : BaseProjectile
    {
        protected override bool EnablePierceDamageFalloff { get => false; }

        protected string explosionSample = "BlasterExplosion";
        private int baseRadius = 150;
        private bool hasExploded = false;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 6;
            Projectile.penetrate = -1;
        }

        public override void AfterSpawn()
        {
            Initialize();
            Projectile.CritChance = 100;

            if (IsThisClientTheProjectileOwner())
            {
                var finalRadius = (int)(baseRadius * explosionRadiusModifier);
                var e = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                var a = new PlayAudioModel(_soundPath: explosionSample, _volume: 0.3f, _pitchVariance: 0.1f, _maxInstances: 3);
                CreateExplosionVisual(e, a);

                Projectile.Resize(finalRadius, finalRadius);
                hasExploded = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 2;
        }
    }
}
