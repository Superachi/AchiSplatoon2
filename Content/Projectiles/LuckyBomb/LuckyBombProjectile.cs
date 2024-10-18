using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.LuckyBomb
{
    internal class LuckyBombProjectile : BaseProjectile
    {
        public int spawnOrder = 0;

        private const int stateSpawn = 0;
        private const int stateChase = 1;
        private const int stateExplode = 2;
        private const int stateSequenceExplode = 3;

        private NPC npcTarget = null;
        private float chaseSpeed = 0f;
        private int remainingTargetAttempts = 10;
        private float maxTargetDistance = 800f;

        private float drawScale = 0f;
        private float sizeMod = 1f;
        private int frameCount = 8;
        private float frameTimer = 0f;
        private int frameDelay = 4;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = frameCount;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            SetState(stateSpawn);
            Projectile.frame = Main.rand.Next(frameCount);

            if (Projectile.damage < 75) sizeMod = 1f;
            else if (Projectile.damage < 125) sizeMod = 1.25f;
            else if (Projectile.damage < 200) sizeMod = 1.5f;
            else sizeMod = 2f;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == NPCID.TargetDummy) return false;
            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (state != stateExplode) SetState(stateExplode);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DisableCrit();
            modifiers.Defense *= 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (state != stateExplode)
            {
                DrawProjectile(inkColor: initialColor, rotation: 0, scale: drawScale, considerWorldLight: false);
            }
            return false;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            frameTimer += FrameSpeedDivide(1);
            if (frameTimer >= frameDelay)
            {
                frameTimer = 0;
                Projectile.frame = (Projectile.frame + 1) % frameCount;
            }

            switch (state)
            {
                case stateSpawn:
                    if (drawScale < 1f * sizeMod) drawScale += 0.05f * sizeMod;
                    if (timeSpentAlive >= 30) AdvanceState();
                    break;
                case stateChase:
                    if (npcTarget != null && npcTarget.life > 0)
                    {
                        var dist = Projectile.Center.Distance(npcTarget.Center);

                        if (chaseSpeed < 5) chaseSpeed += 0.2f;
                        chaseSpeed = Math.Min(chaseSpeed, dist);

                        var moveVec = Projectile.Center.DirectionTo(npcTarget.Center);
                        Projectile.velocity += moveVec * chaseSpeed;
                        Projectile.velocity.X = Math.Clamp(Projectile.velocity.X, -10, 10);
                        Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y, -10, 10);
                    }
                    else
                    {
                        if (timeSpentAlive % 6 == 0)
                        {
                            FindClosestEnemy();
                        }
                    }

                    if (Projectile.timeLeft < 6) AdvanceState();
                    break;
                case stateSequenceExplode:
                    if (Projectile.timeLeft < 12) drawScale += 0.3f;
                    if (Projectile.timeLeft < 6) SetState(stateExplode);
                    break;
            }

            if (Main.rand.NextBool(5))
            {
                Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                    Type: ModContent.DustType<SplatterBulletLastingDust>(),
                    Velocity: Vector2.Zero,
                    newColor: initialColor,
                    Scale: Main.rand.NextFloat(0.6f, 1.2f));
            }
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (targetState)
            {
                case stateSpawn:
                    Projectile.velocity = Main.rand.NextVector2Circular(5, 5);
                    break;
                case stateChase:
                    Projectile.friendly = true;
                    FindClosestEnemy();
                    break;
                case stateExplode:
                    Detonate();
                    Projectile.timeLeft = 6;
                    break;
                case stateSequenceExplode:
                    Projectile.timeLeft = 30 + (spawnOrder * 6);
                    break;
            }
        }

        private void FindClosestEnemy()
        {
            chaseSpeed = 0f;

            if (remainingTargetAttempts > 0)
            {
                remainingTargetAttempts--;
                float closestDistance = maxTargetDistance;

                foreach (var npc in Main.ActiveNPCs)
                {
                    float distance = Projectile.Center.Distance(npc.Center);
                    if (distance < closestDistance && IsTargetEnemy(npc))
                    {
                        closestDistance = distance;
                        npcTarget = npc;
                    }
                }
            } else
            {
                SetState(stateSequenceExplode);
            }
        }

        private void Detonate()
        {
            var finalExplosionRadius = (int)(100 * explosionRadiusModifier);
            Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
            Projectile.alpha = 255;
            Projectile.velocity = Vector2.Zero;
            var e = new ExplosionDustModel(_dustMaxVelocity: 25, _dustAmount: 20, _minScale: 1.5f, _maxScale: 3f, _radiusModifier: finalExplosionRadius);
            var a = new PlayAudioModel("Throwables/SplatBombDetonate", _volume: 0.4f, _pitchVariance: 0.5f, _pitch: 4f, _maxInstances: 3, _position: Projectile.Center);
            SoundHelper.PlayAudio(a);
            TripleHitDustBurst(playSample: false);
        }
    }
}
