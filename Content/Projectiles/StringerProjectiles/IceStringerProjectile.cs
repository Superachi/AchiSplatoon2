using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class IceStringerProjectile : TriStringerProjectile
    {
        private int hitShrapnelCount = 8;
        private int hitShrapnelCooldown = 0;
        private int hitShrapnelCooldownMax = 6;

        private bool hasLanded = false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            DrawOriginOffsetX = -10;
            DrawOriginOffsetY = 0;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();

            SoundHelper.PlayAudio(SoundID.Item28, 0.3f, maxInstances: 10, pitch: 0.5f, position: Projectile.Center);
            colorOverride = new Color(r: 106, g: 218, b: 255);
            dissolvable = false;
            enablePierceDamagefalloff = true;

            if (parentFullyCharged)
            {
                Projectile.penetrate += 5;
            }
        }

        public override void AI()
        {
            base.AI();

            if (hitShrapnelCooldown > 0)
            {
                hitShrapnelCooldown--;
            }

            var dustChance = sticking == true ? 75 : 20;
            if (Main.rand.NextBool(dustChance))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.RainbowTorch,
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(0.5f, 1f)
                );
                dust.noGravity = true;
            }

            if (sticking && !hasLanded)
            {
                hasLanded = true;
                SoundHelper.PlayAudio(SoundID.DD2_CrystalCartImpact, 1f, maxInstances: 10, position: Projectile.Center);
            }
        }

        protected override void Explode()
        {
            SoundHelper.PlayAudio(SoundID.DD2_WitherBeastCrystalImpact, 1f, maxInstances: 10, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.DD2_CrystalCartImpact, maxInstances: 10, position: Projectile.Center);
            CreateShrapnel(10, 5f, 15f);
            Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (parentFullyCharged && hitShrapnelCooldown <= 0)
            {
                SoundHelper.PlayAudio(SoundID.Shatter, 0.5f, maxInstances: 10, pitchVariance: 0.2f, pitch: 0.5f, position: Projectile.Center);
                CreateShrapnel(hitShrapnelCount, 3f, 6f);
                hitShrapnelCooldown = hitShrapnelCooldownMax;

                if (hitShrapnelCount > 3) hitShrapnelCount--;
            }
        }

        private void CreateShrapnel(int projectileCount, float minVelocity, float maxVelocity)
        {
            float randomDegree = Main.rand.NextFloat(360);
            for (int i = 0; i < projectileCount; i++)
            {
                var degreeIncrement = (float)(360f / projectileCount) * i + randomDegree;
                var radians = MathHelper.ToRadians(degreeIncrement);
                var angleVector = radians.ToRotationVector2();
                var velocity = Main.rand.NextFloat(minVelocity, maxVelocity);

                CreateChildProjectile<IceStringerShrapnel>(Projectile.Center, angleVector * velocity, Projectile.damage / 2);
            }
        }
    }
}
