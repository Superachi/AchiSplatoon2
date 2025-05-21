using AchiSplatoon2.Content.AsepriteAnimationData.Models;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.TacticoolerProjectiles
{
    internal class TacticoolerStand : BaseProjectile
    {
        protected override bool FallThroughPlatforms => false;

        private const int _stateThrow = 0;
        private const int _stateActivate = 1;
        private const int _stateIdle = 2;
        private const int _stateDespawn = 3;

        private readonly List<Player> _playersThatReceivedDrink = new();

        private int _buffRange;
        private int _fallDelay;
        private float _fallSpeed;
        private float _friction;
        private bool _enableLight;

        private AnimationFrames _frameData = new();
        private int _frameTimer = 0;
        private int _lastReferencedFrame = 0;

        private SlotId _throwSound;
        private SlotId _jingleSound;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            SetState(_stateThrow);

            _buffRange = 150;
            _fallDelay = 10;
            _fallSpeed = 0.5f;
            _friction = 0.98f;

            var data = AnimationDataPaths.Tacticooler.ToAnimationFrames();
            if (data == null)
            {
                Projectile.Kill();
                return;
            }

            _frameData = data;
            _lastReferencedFrame = -1;
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            switch (state)
            {
                case _stateThrow:
                    _throwSound = PlayAudio(SoundPaths.SplatBombThrow.ToSoundStyle(), volume: 0.3f, position: Projectile.Center);
                    break;
                case _stateActivate:
                    SoundHelper.StopSoundIfActive(_throwSound);
                    break;
            }
        }

        public override void AI()
        {
            _frameTimer++;

            if (_enableLight)
            {
                var lightCol = new Vector3(80, 60, 160);
                var lightVal = 0.005f + (float)(Math.Sin(timeSpentAlive / 20f)) * 0.001f;

                Lighting.AddLight(Projectile.Center, lightCol * lightVal);
            }

            switch (state)
            {
                case _stateThrow:
                    if (timeSpentAlive > _fallDelay)
                    {
                        Projectile.velocity.Y += _fallSpeed;
                    }

                    Projectile.velocity.X *= _friction;
                    break;

                case _stateActivate:
                    Projectile.velocity.Y += _fallSpeed;

                    if (_lastReferencedFrame != Projectile.frame)
                    {
                        _lastReferencedFrame = Projectile.frame;
                        switch (Projectile.frame)
                        {
                            case 0:
                                PlayAudio(SoundPaths.TacticoolerLanding.ToSoundStyle(), volume: 0.5f, pitchVariance: 0.4f, maxInstances: 1, position: Projectile.Center);
                                break;
                            case 8:
                                PlayAudio(SoundPaths.TacticoolerGizmo1.ToSoundStyle(), volume: 0.3f, pitchVariance: 0.2f, maxInstances: 1, position: Projectile.Center);
                                break;
                            case 17:
                                PlayAudio(SoundPaths.TacticoolerGizmo2.ToSoundStyle(), volume: 0.3f, pitchVariance: 0.2f, maxInstances: 1, position: Projectile.Center);
                                break;
                            case 24:
                                _enableLight = true;
                                break;
                            case 26:
                                _jingleSound = PlayAudio(SoundPaths.TacticoolerJingle.ToSoundStyle(), volume: 0.5f, pitchVariance: 0f, maxInstances: 1, position: Projectile.Center);
                                break;
                        }
                    }

                    var durationInFrames = Math.Floor(_frameData.Frames[Projectile.frame].Duration / 1000f * 60);
                    if (_frameTimer >= durationInFrames)
                    {
                        _frameTimer = 0;

                        if (Projectile.frame < 27)
                        {
                            Projectile.frame++;
                        }
                        else
                        {
                            AdvanceState();
                            return;
                        }
                    }

                    break;

                case _stateIdle:
                    if (Projectile.timeLeft < 5)
                    {
                        AdvanceState();
                        return;
                    }

                    if (timeSpentAlive % 4 == 0)
                    {
                        var player = FindClosestPlayer(_buffRange, _playersThatReceivedDrink);
                        if (player != null)
                        {
                            var p = CreateChildProjectile<TacticoolerCan>(Projectile.Center, new Vector2(0, -10), 0, false);
                            p.playerToFollow = player;
                            p.RunSpawnMethods();

                            _playersThatReceivedDrink.Add(player);

                            PlayAudio(SoundPaths.TacticoolerPickup.ToSoundStyle(), volume: 0.3f, pitchVariance: 0f, maxInstances: 1, position: Projectile.Center);
                            SoundHelper.StopSoundIfActive(_jingleSound);
                        }
                    }

                    if (timeSpentAlive % 15 == 0 && Main.rand.NextBool(3))
                    {
                        var dust = Dust.NewDustPerfect(Projectile.Top + Main.rand.NextVector2Circular(35, 35),
                            DustID.PlatinumCoin,
                            Vector2.Zero,
                            255,
                            Main.DiscoColor,
                            Main.rand.NextFloat(0.8f, 1.6f));
                        dust.noGravity = true;
                        dust.noLight = true;
                        dust.noLightEmittence = true;
                    }
                    break;

                case _stateDespawn:
                    Projectile.Kill();

                    PlayAudio(SoundID.NPCHit4, volume: 0.5f, pitchVariance: 0.2f, pitch: -1f, position: Projectile.Center);
                    PlayAudio(SoundID.NPCHit42, volume: 0.3f, pitchVariance: 0.3f, pitch: -0.2f, position: Projectile.Center);

                    for (int i = 0; i < 15; i++)
                    {
                        var d = Dust.NewDustPerfect(
                            Position: Projectile.Center,
                            Type: DustID.AncientLight,
                            Velocity: Main.rand.NextVector2CircularEdge(10, 10),
                            newColor: CurrentColor,
                            Scale: 2f);
                        d.noGravity = true;
                        d.noLight = true;
                    }

                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(Color.White, Projectile.rotation, scale: 1, alphaMod: 1, considerWorldLight: true, positionOffset: new Vector2(0, -28));

            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == _stateThrow)
            {
                // Left/right collide (bounce)
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    ProjectileBounce(oldVelocity);
                }

                // Top/bottom collide (bounce if going up, stand still if going down)
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    if (Projectile.velocity.Y < 0)
                    {
                        ProjectileBounce(oldVelocity);
                    }
                    else
                    {
                        ProjectileBounce(oldVelocity);
                        Projectile.velocity = Vector2.Zero;

                        AdvanceState();
                        return false;
                    }
                }

                if (timeSpentAlive >= 3)
                {
                    PlayAudio(SoundID.NPCHit4, volume: 0.5f, pitchVariance: 0.2f, pitch: -1f, position: Projectile.Center);
                    PlayAudio(SoundID.NPCHit42, volume: 0.3f, pitchVariance: 0.3f, pitch: -0.2f, position: Projectile.Center);
                }
            }

            return false;
        }

        private Player? FindClosestPlayer(float maxTargetDistance, List<Player> playersToSkip)
        {
            Player? playerTarget = null;

            float closestDistance = maxTargetDistance;
            foreach (var player in Main.ActivePlayers)
            {
                float distance = Projectile.Center.Distance(player.Center);
                if (!playersToSkip.Contains(player) && distance < closestDistance)
                {
                    closestDistance = distance;
                    playerTarget = player;
                }
            }

            return playerTarget;
        }
    }
}
