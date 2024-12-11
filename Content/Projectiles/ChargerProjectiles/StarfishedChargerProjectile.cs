using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class StarfishedChargerProjectile : ChargerProjectile
    {
        private int npcHits = 0;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (wasParentChargeMaxed && target.life > 0 && npcHits < 5)
            {
                var p = CreateChildProjectile<StarfishedChargerBlast>(Projectile.Center, Vector2.Zero, Projectile.damage * 2, false);
                p.delayUntilBlast += npcHits * 5;
                p.pitchAdd += npcHits * 0.2f;
                p.npcTarget = target.whoAmI;
                p.RunSpawnMethods();

                npcHits++;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (var i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.AncientLight,
                    Velocity: Main.rand.NextVector2Circular(10, 10),
                    newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                    Scale: Main.rand.NextFloat(1f, 2f));
                dust.noGravity = true;
            }

            return true;
        }

        protected override void DustTrail()
        {
            var dust = Dust.NewDustPerfect(
                Position: Projectile.Center,
                Type: DustID.AncientLight,
                Velocity: Vector2.Zero,
                newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                Scale: Main.rand.NextFloat(0.4f, 1.0f));
            dust.noGravity = true;

            if (timeSpentAlive > FrameSpeedMultiply(2))
            {
                if (Main.rand.NextBool(50))
                {
                    dust = Dust.NewDustPerfect(
                        Position: Projectile.Center,
                        Type: DustID.AncientLight,
                        Velocity: Main.rand.NextVector2CircularEdge(5, 5),
                        newColor: ColorHelper.ColorWithAlphaZero(CurrentColor),
                        Scale: Main.rand.NextFloat(0.4f, 2.0f));
                    dust.noGravity = true;
                }

                if (wasParentChargeMaxed)
                {
                    if (Main.rand.NextBool(50))
                    {
                        dust = Dust.NewDustPerfect(
                            Position: Projectile.Center,
                            Type: DustID.FireworksRGB,
                            Velocity: WoomyMathHelper.AddRotationToVector2(-Projectile.velocity, -20, 20) * Main.rand.Next(5, 10),
                            newColor: ColorHelper.IncreaseHueBy(Main.rand.Next(-30, 30), ColorHelper.ColorWithAlpha255(CurrentColor)),
                            Scale: Main.rand.NextFloat(0.4f, 1.0f));
                        dust.noGravity = true;
                    }
                }
            }
        }

        protected override void PlayShootSound()
        {
            if (wasParentChargeMaxed)
            {
                PlayAudio(SoundID.Item158, volume: 0.7f, maxInstances: 5, pitch: 0.5f, pitchVariance: 0.2f);
                PlayAudio(SoundID.DD2_SkyDragonsFuryShot, volume: 0.5f, maxInstances: 5, pitch: 0.3f, pitchVariance: 0.2f);
                PlayAudio(SoundID.Item25, volume: 0.5f, maxInstances: 5, pitch: 0.5f, pitchVariance: 0.2f);
                PlayAudio(SoundID.Item67, volume: 0.3f, maxInstances: 5, pitch: 0.2f, pitchVariance: 0.2f);
            }
            else
            {
                PlayAudio(SoundID.DD2_SkyDragonsFuryShot, volume: 0.4f, maxInstances: 5, pitch: 0.8f, pitchVariance: 0.2f);
                PlayAudio(SoundID.Item67, volume: 0.3f, maxInstances: 5, pitch: 0.8f, pitchVariance: 0.2f);
            }
        }

        protected override void CreateDustOnSpawn()
        {
            int dustAmount = wasParentChargeMaxed ? 12 : 6;

            for (int i = 1; i < dustAmount; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                var d = Dust.NewDustDirect(
                    Projectile.Center,
                    Projectile.width,
                    Projectile.height,
                    DustID.AncientLight,
                    newColor: ColorHelper.IncreaseHueBy(Main.rand.Next(-30, 30), ColorHelper.ColorWithAlphaZero(CurrentColor)),
                    Scale: Main.rand.NextFloat(1f, 1.4f));
                d.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, Main.rand.NextFloat(-20, 20)) * 1.5f * Main.rand.NextFloat(1, 4) * i;
                d.noGravity = true;
            }
        }
    }
}
