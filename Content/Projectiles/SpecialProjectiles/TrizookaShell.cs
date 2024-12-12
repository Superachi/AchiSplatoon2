using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class TrizookaShell : BaseProjectile
    {
        private int _delayUntilFall;
        private float _fallSpeed;
        private int _remainingBounces;
        private int _lastBounceTimestamp;
        private bool _fading;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        protected override void AfterSpawn()
        {
            _delayUntilFall = 20;
            _fallSpeed = 0.05f;
            _remainingBounces = 3;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.995f;
            Projectile.rotation += Projectile.velocity.X * 0.03f;

            if (timeSpentAlive >= FrameSpeed(_delayUntilFall))
            {
                Projectile.velocity.Y += _fallSpeed;
            }

            if (timeSpentAlive > 300 || _remainingBounces <= 0)
            {
                _fading = true;
            }

            if (_fading)
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0, 0.1f);
            }
            else
            {
                if (timeSpentAlive > 10 && Main.rand.NextBool(1 + (timeSpentAlive / 8)))
                {
                    Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.Smoke,
                        Main.rand.NextVector2Circular(1, 1),
                        0,
                        Color.White,
                        Main.rand.NextFloat(0.5f, 1.5f));
                }
            }

            if (Projectile.scale < 0.05f)
            {
                Projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (timeSpentAlive - _lastBounceTimestamp > 10 && !_fading)
            {
                _lastBounceTimestamp = timeSpentAlive;
                PlayAudio(SoundID.NPCHit17, volume: 0.05f * Projectile.velocity.Length(), pitchVariance: 0.2f, pitch: 1f, maxInstances: 10);
            }

            // Only subtract remaining bounces for vertical bounces
            if (_remainingBounces > 0 && Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                _remainingBounces--;
                ProjectileBounce(oldVelocity, new Vector2(0.8f, 0.5f));
            }
            else
            {
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    ProjectileBounce(oldVelocity, new Vector2(0.8f, 0.95f));
                }
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(Color.White, Projectile.rotation, Projectile.scale);
            return false;
        }
    }
}
