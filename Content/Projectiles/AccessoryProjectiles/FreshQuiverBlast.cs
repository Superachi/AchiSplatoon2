using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class FreshQuiverBlast : BaseProjectile
    {
        private readonly int baseRadius = 150;
        private bool hasExploded = false;

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
            Projectile.CritChance = 100;

            PlayAudio(SoundID.Item38, volume: 0.8f, pitch: 1f, pitchVariance: 0.3f, position: Owner.Center);
            PlayAudio(SoundID.Item38, volume: 0.5f, pitch: 0f, pitchVariance: 0.1f, position: Owner.Center);
            PlayAudio(SoundID.Item14, volume: 0.5f, pitch: 0f, pitchVariance: 0.2f, position: Owner.Center);

            for (int i = 0; i < 3; i++)
            {
                var g = Gore.NewGoreDirect(
                    Terraria.Entity.GetSource_None(),
                    Projectile.Center + new Vector2(-20, -15),
                    Vector2.Zero,
                    GoreID.Smoke3,
                    Main.rand.NextFloat(1f, 2f));

                g.velocity = Main.rand.NextVector2Circular(2, 2);
                g.alpha = 196;
            }

            if (IsThisClientTheProjectileOwner())
            {
                var finalRadius = (int)(baseRadius * explosionRadiusModifier);
                var e = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: finalRadius);
                CreateExplosionVisual(e);

                Projectile.Resize(finalRadius, finalRadius);
                hasExploded = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 2;
            modifiers.HitDirectionOverride = GetOwner().direction;
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}
