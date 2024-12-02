using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class TorpedoProjectile : BaseProjectile
    {
        private const int stateFly = 0;
        private const int stateLockOnGrow = 1;
        private const int stateLockOnShrink = 2;
        private const int stateChase = 3;
        private const int stateExplode = 4;
        private const int stateRoll = 5;

        private NPC? target = null;
        private float chaseSpeed = 0f;
        private readonly float chaseSpeedMax = 15f;

        private int pelletCount;
        private float pelletDamageMod;
        private int explosionRadius = 150;
        private readonly int detectionRadius = 300;
        private Vector2? lockOnPosition;

        // Physics
        private float drawScale = 1f;
        private float drawRotation = 0f;
        private readonly int delayUntilFall = 10;
        private readonly float fallSpeed = 0.5f;
        private float xVelocityBeforeBump;

        // Audio
        private SlotId throwAudio;

        // Visual
        protected float brightness = 0.002f;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;

            ProjectileID.Sets.CanHitPastShimmer[Type] = false;
            ProjectileID.Sets.CanDistortWater[Type] = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as Torpedo;
            explosionRadius = weaponData.ExplosionRadius;
            pelletCount = weaponData.PelletCount;
            pelletDamageMod = weaponData.PelletDamageMod;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            explosionRadius = (int)(explosionRadius * explosionRadiusModifier);
            wormDamageReduction = true;

            throwAudio = PlayAudio(SoundPaths.SplatBombThrow.ToSoundStyle());

            if (IsThisClientTheProjectileOwner())
            {
                float distance = Vector2.Distance(Main.LocalPlayer.Center, Main.MouseWorld);
                float velocityMod = MathHelper.Clamp(distance / 300f, 0.4f, 1f);
                Projectile.velocity *= velocityMod;
                NetUpdate(ProjNetUpdateType.SyncMovement);
            }
        }

        protected override void SetState(int targetState)
        {
            var oldState = state;
            base.SetState(targetState);

            switch (state)
            {
                case stateLockOnGrow:
                    var soundExists = SoundEngine.TryGetActiveSound(throwAudio, out var sound);
                    if (soundExists)
                    {
                        sound.Stop();
                    }

                    PlayAudio(SoundPaths.TorpedoLockOn.ToSoundStyle(), volume: 0.2f, maxInstances: 5, position: Projectile.Center);
                    Projectile.frame++;
                    break;
                case stateExplode:
                    var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, Projectile.damage, false);
                    var a = new PlayAudioModel("Throwables/SplatBombDetonate", _volume: 0.4f, _pitchVariance: 0.5f, _pitch: 4f, _maxInstances: 3, _position: Projectile.Center);

                    p.SetProperties(explosionRadius, a);
                    p.RunSpawnMethods();
                    Projectile.timeLeft = 6;

                    if (oldState != stateRoll)
                    {
                        for (int i = 0; i < pelletCount; i++)
                        {
                            var dir = MathHelper.ToRadians(i * (180 / pelletCount) + 180).ToRotationVector2();
                            var pellet = CreateChildProjectile<TorpedoPelletProjectile>(
                                Projectile.Center,
                                dir * 3 + Main.rand.NextVector2Circular(1, 1),
                                (int)(Projectile.damage * pelletDamageMod));

                            pellet.explosionRadius = explosionRadius / 2;
                        }
                    }
                    break;
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, CurrentColor.R * brightness, CurrentColor.G * brightness, CurrentColor.B * brightness);

            void LockOn()
            {
                if (target != null && target.life > 0)
                {
                    lockOnPosition = target.Center;
                }

                drawRotation = Projectile.DirectionTo((Vector2)lockOnPosition).ToRotation();
                Projectile.velocity *= 0.6f;

                if (timeSpentInState >= 20)
                {
                    SetState(stateChase);
                }
            }

            switch (state)
            {
                case stateFly:
                    MovementWithGravity();
                    if (timeSpentAlive % 6 == 0 && timeSpentInState >= 12)
                    {
                        target = FindClosestEnemy(detectionRadius, checkLineOfSight: true);
                        if (target != null)
                        {
                            SetState(stateLockOnGrow);
                        }
                    }
                    break;

                case stateLockOnGrow:
                    if (drawScale < 2)
                    {
                        drawScale += 0.2f;
                    }
                    else
                    {
                        SetState(stateLockOnShrink);
                    }
                    LockOn();
                    break;

                case stateLockOnShrink:
                    if (drawScale > 1)
                    {
                        drawScale -= 0.1f;
                    }
                    LockOn();
                    break;

                case stateChase:
                    chaseSpeed = Math.Min(chaseSpeed + 0.2f, chaseSpeedMax);

                    if (timeSpentInState >= 50)
                    {
                        drawScale += 0.1f;
                    }

                    if (timeSpentInState >= 60)
                    {
                        SetState(stateExplode);
                        return;
                    }

                    if (timeSpentInState % 2 == 0)
                    {
                        Projectile.frame++;
                        if (Projectile.frame > 4)
                        {
                            Projectile.frame = 1;
                        }
                    }

                    if (target != null && target.life > 0)
                    {
                        lockOnPosition = target.Center;
                        Projectile.velocity = Projectile.DirectionTo(target.Center) * chaseSpeed;
                    }

                    Projectile.velocity = Projectile.DirectionTo((Vector2)lockOnPosition) * chaseSpeed;
                    if (Vector2.Distance(Projectile.Center, (Vector2)lockOnPosition) <= chaseSpeedMax * 2)
                    {
                        SetState(stateExplode);
                    }

                    drawRotation = Projectile.velocity.ToRotation();

                    if (Projectile.soundDelay == 0)
                    {
                        Projectile.soundDelay = 5;
                        PlayAudio(SoundPaths.TorpedoChase.ToSoundStyle(), volume: 0.3f * (chaseSpeed / chaseSpeedMax), pitchVariance: 0.2f, maxInstances: 5, pitch: 0.05f);
                    }
                    break;

                case stateRoll:
                    MovementWithGravity();
                    if (timeSpentInState >= 30)
                    {
                        SetState(stateExplode);
                    }
                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == stateFly)
            {
                SetState(stateRoll);
            }
            else if (state == stateChase)
            {
                SetState(stateExplode);
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (state == stateExplode) return false;

            DrawProjectile(inkColor: CurrentColor, rotation: drawRotation + MathHelper.ToRadians(45), scale: drawScale, alphaMod: 1, considerWorldLight: false, additiveAmount: 0.5f);
            return false;
        }

        private void MovementWithGravity()
        {
            var frictionMod = 0.995f;
            var rotateMod = 0.02f;

            if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
            {
                frictionMod = 0.95f;
                rotateMod = 0.04f;

                if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                {
                    Projectile.velocity.X = 0f;
                    Projectile.netUpdate = true;
                }
            }

            if (Projectile.velocity.X == 0f)
            {
                Projectile.velocity.X = -xVelocityBeforeBump * 0.7f;
            }
            else
            {
                xVelocityBeforeBump = Projectile.velocity.X;
            }

            Projectile.velocity.X = Projectile.velocity.X * frictionMod;
            Projectile.rotation += Projectile.velocity.X * rotateMod;
            drawRotation = Projectile.rotation;

            if (timeSpentAlive >= delayUntilFall) Projectile.velocity.Y += fallSpeed;
        }
    }
}
