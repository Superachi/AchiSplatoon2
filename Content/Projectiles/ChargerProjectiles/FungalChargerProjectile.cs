using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class FungalChargerProjectile : ChargerProjectile
    {
        private int npcHits = 0;

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            Projectile.velocity *= Main.rand.NextFloat(0.8f, 1f);
            Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, Main.rand.NextFloat(-2, 2));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            for (var i = 0; i < 5; i++)
            {
                var dust = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.Smoke,
                    Velocity: Main.rand.NextVector2Circular(10, 10),
                    newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                    Scale: Main.rand.NextFloat(2f, 3f));
                dust.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (var i = 0; i < 3; i++)
            {
                var dust = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.Smoke,
                    Velocity: Main.rand.NextVector2Circular(5, 5),
                    newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                    Scale: Main.rand.NextFloat(1f, 2f));
                dust.noGravity = true;
            }

            ProjectileBounce(oldVelocity, new Vector2(0.3f, 0.3f));

            return false;
        }

        protected override void DustTrail()
        {
            if (Main.rand.NextBool(10))
            {
                var dust = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.Smoke,
                    Velocity: Projectile.velocity * 4 + Main.rand.NextVector2Circular(2f, 2f),
                    newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                    Scale: Main.rand.NextFloat(1f, 1.4f));
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(100))
            {
                var dust = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.FireworksRGB,
                    Velocity: Projectile.velocity * 4 + Main.rand.NextVector2Circular(2f, 2f),
                    newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                    Scale: Main.rand.NextFloat(0.5f, 1f));
                dust.noGravity = true;
            }
        }

        protected override void PlayShootSound()
        {
            if (wasParentChargeMaxed)
            {
                PlayAudio(SoundID.Item38, volume: 0.2f, maxInstances: 1, pitch: -0.3f, pitchVariance: 0.3f);
                PlayAudio(SoundID.Item39, volume: 0.2f, maxInstances: 1, pitch: 0.2f, pitchVariance: 0.3f);
                PlayAudio(SoundID.Item41, volume: 0.7f, maxInstances: 1, pitch: -0.2f, pitchVariance: 0f);
                PlayAudio(SoundID.Item102, volume: 0.1f, maxInstances: 5, pitch: 0.2f, pitchVariance: 0.3f);
                PlayAudio(SoundID.Item109, volume: 0.2f, maxInstances: 1, pitch: 0.2f, pitchVariance: 0.3f);
            }
            else
            {
                PlayAudio(SoundID.Item39, volume: 0.2f, maxInstances: 1, pitch: 0.2f, pitchVariance: 0.3f);
                PlayAudio(SoundID.Item41, volume: 0.4f, maxInstances: 1, pitch: 0.3f, pitchVariance: 0f);
                PlayAudio(SoundID.Item102, volume: 0.1f, maxInstances: 5, pitch: 0.2f, pitchVariance: 0.3f);
            }
        }

        protected override void CreateDustOnSpawn()
        {
            int dustAmount = wasParentChargeMaxed ? 6 : 3;

            for (int i = 1; i < dustAmount; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                var d = Dust.NewDustDirect(
                    Projectile.Center,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke,
                    newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                    Scale: Main.rand.NextFloat(1.4f, 2f));
                d.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, Main.rand.NextFloat(-20, 20)) * 1f * Main.rand.NextFloat(1, 3) * i;
                d.noGravity = true;
            }
        }
    }
}
