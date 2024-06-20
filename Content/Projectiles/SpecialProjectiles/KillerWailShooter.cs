using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class KillerWailShooter : BaseProjectile
    {
        private const int stateAiming = 0;
        private const int statePreFire = 1;
        private const int stateFiring = 2;
        private const int stateStop = 3;
        private const int stateDespawn = 4;
        private Vector2 aimVector;
        private Vector2 firePosition;

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 56;
            Projectile.friendly = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            PlayAudio("Specials/KillerWailSpawn", volume: 0.5f, pitchVariance: 0f, maxInstances: 3);
        }

        protected float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        protected override void AdvanceState()
        {
            base.AdvanceState();
            Timer = 0;
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case statePreFire:
                    Projectile.velocity = Vector2.Zero;
                    firePosition = Projectile.position;
                    PlayAudio("Specials/KillerWailCharge", volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
                    break;
                case stateFiring:
                    PlayAudio("Specials/KillerWailFire", volume: 0.6f, pitchVariance: 0.1f, maxInstances: 3);
                    break;
                case stateStop:
                    Projectile.scale = 1;
                    break;
            }
        }

        public override void AI()
        {
            Timer++;
            Lighting.AddLight(Projectile.Center, GenerateInkColor().ToVector3());

            if (IsThisClientTheProjectileOwner())
            {
                Player owner = Main.player[Projectile.owner];

                switch (state)
                {
                    case stateAiming:
                        // Slow down movement
                        Projectile.velocity *= 0.93f;
                        if (Math.Abs(Projectile.velocity.X) < 0.1f) Projectile.velocity.X = 0f;
                        if (Math.Abs(Projectile.velocity.Y) < 0.1f) Projectile.velocity.Y = 0f;

                        // Rotate projectile sprite and adjust angle to point towards cursor
                        float radians = Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                        aimVector = radians.ToRotationVector2();
                        Projectile.rotation = radians;

                        if (Timer > 70) { AdvanceState(); }
                        break;
                    case statePreFire:
                        // Telegraph the attack
                        if (Timer % 7 == 0 && Timer < 60)
                        {
                            var proj = Projectile.NewProjectile(
                            spawnSource: Projectile.GetSource_FromThis(),
                            position: Projectile.Center,
                            velocity: aimVector * 8f,
                            Type: ModContent.ProjectileType<KillerWailTelegraph>(),
                            Damage: 0,
                            KnockBack: 0,
                            Owner: Main.myPlayer);
                        }

                        if (Timer > 60) { Projectile.scale -= 0.08f; }
                        if (Timer > 70) { AdvanceState(); }
                        break;
                    case stateFiring:
                        // Spawn shockwave projectiles, piercing through walls/enemies
                        Projectile.scale = 2f + (float)Math.Sin(MathHelper.ToRadians(Timer * 80)) * 0.2f;

                        // Shake the speaker visually
                        Projectile.position = firePosition + Main.rand.NextVector2Circular(3, 3);

                        if (Timer % 5 == 0)
                        {
                            Projectile.position = firePosition;

                            // Add recoil to the killer wail speaker
                            firePosition -= aimVector * 2f;

                            // Apply more knockback on the final hit
                            float knockbackMult = 1f;
                            if (Timer >= 195)
                            {
                                knockbackMult = 5f;
                            }

                            var proj = Projectile.NewProjectile(
                            spawnSource: Projectile.GetSource_FromThis(),
                            position: Projectile.Center,
                            velocity: aimVector * Main.rand.NextFloat(7, 8),
                            Type: ModContent.ProjectileType<KillerWailProjectile>(),
                            Damage: Projectile.damage,
                            KnockBack: Projectile.knockBack * knockbackMult,
                            Owner: Main.myPlayer);

                            PlayAudio(SoundID.Item73, volume: 0.4f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.4f);
                        }
                        if (Timer > 200) { AdvanceState(); }
                        break;
                    case stateStop:
                        // Stop firing, but linger a bit
                        if (Timer < 10)
                        {
                            Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.2f, 0.1f);
                        }
                        if (Timer >= 10 && Timer < 50)
                        {
                            Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.1f);
                        }
                        if (Timer >= 55)
                        {
                            Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 0.2f);
                        }

                        if (Timer > 60) { AdvanceState(); }
                        break;
                    case stateDespawn:
                        Projectile.Kill();
                        break;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            EmitBurstDust(20, 15, 1, 2);
            PlayAudio("Specials/KillerWailDespawn", volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
        }
    }
}
