using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles.CorruptionBrellaProjectiles
{
    internal class CorruptionBrellaPelletProjectile : BrellaPelletProjectile
    {
        private int _initialDirection = 0;
        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            dissolvable = false;

            Projectile.position = GetOwner().Center;
            _initialDirection = GetOwner().direction;

            Projectile.knockBack = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, 1f * _initialDirection);
            Projectile.velocity *= 0.99f;

            if (Projectile.velocity.Length() < 0.05f)
            {
                Projectile.Kill();
                return;
            }

            if (timeSpentAlive > 6 && Main.rand.NextBool(3))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.Venom,
                    Velocity: Projectile.velocity / 2,
                    newColor: Color.White,
                    Scale: 1.2f);

                d.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(BuffID.Venom, 30);
        }

        protected override void CreateDustOnDespawn()
        {
            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustDirect(
                    Projectile.Center,
                    1,
                    1,
                    DustID.Venom,
                    newColor: Color.White,
                    Scale: Main.rand.NextFloat(1f, 2f));
                d.velocity = Main.rand.NextVector2CircularEdge(3, 3);
                d.noGravity = true;
            }
        }
    }
}
