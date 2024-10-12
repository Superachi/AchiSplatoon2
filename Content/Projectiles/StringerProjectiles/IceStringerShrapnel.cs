using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class IceStringerShrapnel : BaseProjectile
    {
        protected override bool CountDamageForSpecialCharge => false;
        float drawScale = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            drawScale = Main.rand.NextFloat(0.5f, 1f);
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (timeSpentAlive > 20 && drawScale > 0)
            {
                drawScale -= 0.05f;
            }

            if (drawScale < 0.3f)
            {
                Projectile.friendly = false;
            }

            if (drawScale <= 0)
            {
                Projectile.Kill();
            }

            Projectile.rotation += Projectile.velocity.X / 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(
                Color.White,
                Projectile.rotation,
                drawScale,
                1f,
                false,
                additiveAmount: 0.3f);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DisableCrit();
            modifiers.DisableKnockback();
        }
    }
}
