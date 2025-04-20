using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class StarfishedChargerBlast : BaseProjectile
    {
        private readonly int baseRadius = 100;
        private bool hasExploded = false;

        public float pitchAdd = 0f;
        public int delayUntilBlast = 40;
        public int npcTarget = -1;
        private int blastTimer = 0;

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.knockBack = 5;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DisableCrit();
        }

        public override void AI()
        {
            NPC target = Main.npc[npcTarget];
            if (target.life <= 0)
            {
                Projectile.Kill();
                return;
            }

            blastTimer++;
            if (blastTimer >= delayUntilBlast && !hasExploded)
            {
                if (IsThisClientTheProjectileOwner())
                {
                    var finalRadius = (int)(baseRadius * explosionRadiusModifier);

                    var a = new PlayAudioModel(SoundPaths.BlasterExplosion, _volume: 0.3f, _pitchVariance: 0.1f, _maxInstances: 3, _pitch: -0.4f + pitchAdd, _position: Projectile.Center);
                    var p = CreateChildProjectile<BlastProjectile>(target.Center, Vector2.Zero, Projectile.damage, false);
                    p.SetProperties(finalRadius, a);
                    p.RunSpawnMethods();

                    var sparkle = CreateChildProjectile<StillSparkleVisual>(target.Center, Vector2.Zero, 0, true);
                    sparkle.AdjustRotation(MathHelper.ToRadians(45));
                    sparkle.AdjustColor(CurrentColor);
                    sparkle.AdjustScale(finalRadius / 80f);

                    hasExploded = true;
                    Projectile.timeLeft = 6;
                }
            }
            else
            {
                if (Main.rand.NextBool(4))
                {
                    var dust = Dust.NewDustPerfect(
                        Position: target.Center,
                        Type: 264,
                        Velocity: Main.rand.NextVector2Circular(3, 3),
                        newColor: GenerateInkColor(),
                        Scale: Main.rand.NextFloat(0.8f, 1.6f));
                    dust.noGravity = true;
                }
            }
        }
    }
}
