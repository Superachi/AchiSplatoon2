using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
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

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class SplatBombProjectile : BaseBombProjectile
    {
        private int maxFuseTime = 180;
        private bool hasCollided = false;
        private bool hasExploded = false;
        private int explosionRadius;
        private int finalExplosionRadius;

        private float airFriction = 0.995f;
        private float groundFriction = 0.95f;
        private bool applyGravity = false;
        private float terminalVelocity = 10f;
        private float xVelocityBeforeBump;

        private int state = 0;
        private const int stateFly = 0;
        private const int stateRoll = 1;
        private const int stateRollFuse = 2;
        private const int stateExplode = 3;
        private const int stateDespawn = 4;

        private Color lightColor;
        private float brightness = 0.001f;

        protected float FuseTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -12;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            BaseBomb weaponData = (BaseBomb)weaponSource;
            explosionRadius = weaponData.ExplosionRadius;

            PlayAudio("Throwables/SplatBombThrow");
            lightColor = GenerateInkColor();

            FuseTime = maxFuseTime;
            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CanHitPastShimmer[Type] = false;
            ProjectileID.Sets.CanDistortWater[Type] = true;
        }

        private void AllowMovement()
        {
            // Reduce horizontal speed (more when grounded)
            var frictionMod = airFriction;
            if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
            {
                frictionMod = groundFriction;

                if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                {
                    Projectile.velocity.X = 0f;
                    Projectile.netUpdate = true;
                }
            }

            // If the projectile hits a wall horizontally, bounce off of it
            if (Projectile.velocity.X == 0f)
            {
                Projectile.velocity.X = -xVelocityBeforeBump * 0.8f;
            }
            else
            {
                xVelocityBeforeBump = Projectile.velocity.X;
            }

            Projectile.velocity.X = Projectile.velocity.X * frictionMod;

            // Rotation increased by velocity.X 
            Projectile.rotation += Projectile.velocity.X * 0.05f;

            // Apply gravity
            Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + 0.3f, -terminalVelocity, terminalVelocity);
        }

        #region State machine
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
                    Projectile.friendly = true;
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

        #endregion

        // Deal less damage against the eater of worlds on expert
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y == 0 && state == 0)
            {
                AdvanceState();
            }

            return false;
        }
    }
}
