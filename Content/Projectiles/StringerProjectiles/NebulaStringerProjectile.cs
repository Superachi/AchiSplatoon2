using AchiSplatoon2.Content.Projectiles.AccessoryProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class NebulaStringerProjectile : BaseProjectile
    {
        private NPC? target = null;

        private const int stateWait = 0;
        private const int stateSearch = 1;
        private const int stateChase = 2;
        private const int stateStopChase = 3;
        private const int stateExplode = 4;
        private const int stateStraight = 5;

        private Texture2D spritePartialCharge;
        private Texture2D spriteFullCharge;
        private float drawRotation = 0;

        protected virtual int ExplosionRadius { get => 100; }

        private bool countedForBurst = false;
        public bool parentFullyCharged = false;
        public bool firedWithFreshQuiver = false;

        private int firedPenetrate;
        private readonly int chaseTime = 0;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 5;
            Projectile.friendly = true;
            Projectile.timeLeft = 600 * FrameSpeed();
        }

        protected override void AfterSpawn()
        {
            colorOverride = Color.HotPink;
            Initialize(isDissolvable: false);
            wormDamageReduction = true;
            firedPenetrate = Projectile.penetrate;
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case stateSearch:
                    Projectile.tileCollide = true;
                    break;
                case stateChase:
                    Projectile.penetrate = -1;
                    Projectile.tileCollide = false;
                    break;
                case stateExplode:
                    SoundHelper.PlayAudio(SoundID.Item66, volume: 0.2f, pitchVariance: 0f, maxInstances: 5, pitch: 2, position: Projectile.position);

                    Projectile.timeLeft = 6 * FrameSpeed();
                    Projectile.position -= Projectile.velocity;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.Resize(ExplosionRadius, ExplosionRadius);

                    for (int i = 0; i < 30; i++)
                    {
                        var d = Dust.NewDustPerfect(Position: Projectile.Center, Type: 164, Velocity: Main.rand.NextVector2CircularEdge(6, 6), newColor: initialColor, Scale: Main.rand.NextFloat(0.6f, 1.2f));
                    }
                    break;
                case stateStopChase:
                    Projectile.penetrate = firedPenetrate;
                    break;
                case stateStraight:
                    Projectile.ArmorPenetration = 10;
                    drawRotation = Projectile.velocity.ToRotation();
                    break;
            }
        }

        public override void AI()
        {
            void DustStream()
            {
                if (timeSpentAlive % FrameSpeed() == 0 && state != stateExplode)
                {
                    var d = Dust.NewDustPerfect(Position: Projectile.Center, Type: 164, Velocity: Vector2.Zero, newColor: initialColor, Scale: Main.rand.NextFloat(0.6f, 1.2f));
                    d.velocity = Vector2.Zero;
                }
            }

            void StateWait()
            {
                if (!parentFullyCharged)
                {
                    SetState(stateStraight);
                    return;
                }

                Projectile.Center = GetOwner().Center;
                AdvanceState();
            }

            void StateSearch()
            {
                if (target == null || target.life <= 0)
                {
                    if (timeSpentAlive % 30 == 0)
                    {
                        target = FindClosestEnemy(500);
                    }
                }
                else
                {
                    AdvanceState();
                }

                Projectile.velocity *= 0.995f;
                if (timeSpentAlive > FrameSpeedMultiply(60) + Projectile.ai[2] * FrameSpeedMultiply(6))
                {
                    SetState(stateExplode);
                }

                DustStream();
            }

            void StateChase()
            {
                if (target != null && target.life > 0)
                {
                    var distance = Projectile.Center.Distance(target.Center);

                    var chaseSpeed = 0.05f;
                    if (distance < 100)
                    {
                        chaseSpeed = 0.1f;
                    }
                    Projectile.velocity += Projectile.Center.DirectionTo(target.Center) * chaseSpeed;

                    if (Projectile.velocity.Length() > 2.5f)
                    {
                        Projectile.velocity *= 0.8f;
                    }
                }
                else
                {
                    SetState(stateStopChase);
                }

                DustStream();
            }

            void StateStopChase()
            {
                if (target == null || target.life <= 0)
                {
                    if (timeSpentAlive % 30 == 0)
                    {
                        target = FindClosestEnemy(500);
                    }
                }
                else
                {
                    SetState(stateChase);
                }

                Projectile.velocity *= 0.995f;
                if (Projectile.velocity.Length() < 0.05f)
                {
                    SetState(stateExplode);
                }

                DustStream();
            }

            void StateStraight()
            {
                if (Main.rand.NextBool(10))
                {
                    DustStream();
                }
            }

            switch (state)
            {
                case stateWait:
                    StateWait();
                    break;
                case stateSearch:
                    StateSearch();
                    break;
                case stateChase:
                    StateChase();
                    break;
                case stateStopChase:
                    StateStopChase();
                    break;
                case stateStraight:
                    StateStraight();
                    break;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (state != stateExplode && state != stateStraight)
            {
                SetState(stateExplode);
            }

            if (!countedForBurst && parentFullyCharged)
            {
                countedForBurst = true;

                var parentProj = GetParentProjectile(parentIdentity);
                if (parentProj.ModProjectile is NebulaStringerCharge)
                {
                    var parentModProj = parentProj.ModProjectile as NebulaStringerCharge;
                    if (parentModProj.burstNPCTarget == -1)
                    {
                        parentModProj.burstNPCTarget = target.whoAmI;
                    }

                    if (parentModProj.burstNPCTarget == target.whoAmI)
                    {
                        parentModProj.burstHitCount++;
                    }

                    if (parentModProj.burstHitCount == parentModProj.burstRequiredHits)
                    {
                        TripleHitDustBurst(target.Center);
                        parentProj.Kill();

                        if (firedWithFreshQuiver)
                        {
                            CreateChildProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<FreshQuiverBlast>(), Projectile.damage, true);
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!parentFullyCharged)
            {
                for (int i = 0; i < 10; i++)
                {
                    var d = Dust.NewDustPerfect(Position: Projectile.Center, Type: 164, Velocity: Main.rand.NextVector2CircularEdge(2, 2), newColor: initialColor, Scale: Main.rand.NextFloat(0.6f, 1.2f));
                }
                return true;
            }

            ProjectileBounce(oldVelocity);

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Credits to: https://www.youtube.com/watch?v=cph82roy1_0
            if (spritePartialCharge == null)
            {
                spritePartialCharge = ModContent.Request<Texture2D>("AchiSplatoon2/Content/Assets/Textures/VortexDualieShot").Value;
            }

            if (spriteFullCharge == null)
            {
                spriteFullCharge = ModContent.Request<Texture2D>("AchiSplatoon2/Content/Assets/Textures/NebulaStringerShot").Value;
            }

            if (state == stateWait || state == stateExplode) return false;

            var spriteToRender = spritePartialCharge;
            if (parentFullyCharged)
            {
                spriteToRender = spriteFullCharge;
            }

            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = spriteToRender.Size() / 2;
            Color color = initialColor;
            float scale = 0.5f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 2)) * 0.25f;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 3; i++)
            {
                Main.EntitySpriteDraw(spriteToRender, position, null, color, drawRotation, origin, scale + (i * 0.5f), SpriteEffects.None);
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}