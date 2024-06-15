using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatBombProjectile : BaseProjectile
    {
        private int maxFuseTime = 180;
        private bool applyGravity = false;
        private float terminalVelocity = 10f;
        private bool hasCollided = false;
        private bool hasExploded = false;
        private int explosionRadius = 300;
        private int finalExplosionRadius;

        private int state = 0;
        private const int stateFly = 0;
        private const int stateRoll = 1;
        private const int stateRollFuse= 2;
        private const int stateExplode = 3;
        private const int stateDespawn = 4;

        private Color lightColor;
        private float brightness = 0.001f;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -12;
        }

        protected float FuseTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            PlayAudio("Throwables/SplatBombThrow");
            lightColor = GenerateInkColor();

            FuseTime = maxFuseTime;
            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
        }

        private void EmitBurstDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, float radiusModifier = 100f)
        {
            float radiusMult = radiusModifier / 160;
            amount = Convert.ToInt32(amount * radiusMult);

            // Ink
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = GenerateInkColor();

                var dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<BlasterExplosionDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor, Main.rand.NextFloat(minScale, maxScale));
                dust.velocity *= radiusMult;
            }

            // Firework
            for (int i = 0; i < amount / 2; i++)
            {
                Color dustColor = GenerateInkColor();
                var dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB,
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor);
                dust.velocity *= radiusMult / 2;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    modifiers.FinalDamage /= 3;
                }
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CanHitPastShimmer[Type] = false;
            ProjectileID.Sets.CanDistortWater[Type] = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y == 0 && state == 0)
            {
                AdvanceState();
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasExploded)
            {
                SetState(3);
            }
        }

        private void AllowMovement()
        {
            // Reduce horizontal speed when grounded
            if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
            {
                Projectile.velocity.X = Projectile.velocity.X * 0.95f;

                if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                {
                    Projectile.velocity.X = 0f;
                    Projectile.netUpdate = true;
                }
            }

            // Rotation increased by velocity.X 
            Projectile.rotation += Projectile.velocity.X * 0.05f;

            // Apply gravity
            Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + 0.3f, -terminalVelocity, terminalVelocity);
        }

        private void SetState(int target)
        {
            state = target;

            switch (state)
            {
                case stateRoll:
                    hasCollided = true;
                    maxFuseTime = 30;
                    FuseTime = maxFuseTime;

                    break;
                case stateRollFuse:
                    maxFuseTime = 30;
                    FuseTime = maxFuseTime;

                    PlayAudio("Throwables/SplatBombFuse", volume: 0.4f, pitchVariance: 0.05f, maxInstances: 5);
                    break;
                case stateExplode:
                    maxFuseTime = 6;
                    FuseTime = maxFuseTime;

                    hasExploded = true;
                    Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
                    Projectile.alpha = 255;
                    Projectile.tileCollide = false;
                    EmitBurstDust(dustMaxVelocity: 30, amount: 15, minScale: 2, maxScale: 3, radiusModifier: finalExplosionRadius);
                    StopAudio("Throwables/SplatBombFuse");
                    PlayAudio("Throwables/SplatBombDetonate", volume: 0.6f, pitchVariance: 0.2f, maxInstances: 5);
                    break;
                case stateDespawn:
                    Projectile.Kill();
                    break;
            }
        }

        private void AdvanceState()
        {
            state++;
            SetState(state);
        }

        public override void AI()
        {
            FuseTime--;
            switch (state)
            {
                case stateFly:
                    if (FuseTime < maxFuseTime - 30)
                    {
                        applyGravity = true;
                    }
                    break;
                case stateRollFuse:
                    brightness += 0.0001f * (1 - FuseTime / maxFuseTime);
                    break;
            }

            Lighting.AddLight(Projectile.position, lightColor.R * brightness, lightColor.G * brightness, lightColor.B * brightness);

            if (FuseTime <= 0)
            {
                AdvanceState();
                return;
            }

            if (state < stateExplode)
            {
                AllowMovement();
            }
        }
    }
}
