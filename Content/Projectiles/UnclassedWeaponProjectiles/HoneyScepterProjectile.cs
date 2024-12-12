using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles
{
    internal class HoneyScepterProjectile : BaseProjectile
    {
        private readonly int _maxTargets = 5;
        private NPC? _currentTarget = null;
        private readonly List<int> _hitTargets = new List<int>();

        private const int stateSearch = 0;
        private const int stateAttack = 1;
        private const int stateFlyOut = 2;

        private Vector2 _initialVelocity;
        protected override float DamageModifierAfterPierce => 0.9f;

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.extraUpdates = 240;
            Projectile.timeLeft = Projectile.extraUpdates * 60;
            Projectile.penetrate = -1;
        }

        protected override void AfterSpawn()
        {
            colorOverride = Color.Orange;
            PlayerItemAnimationFaceCursor(GetOwner());
            Initialize();
            PlayShootSound();
            SetState(stateSearch);

            Projectile.localNPCHitCooldown = -1;
            _initialVelocity = Projectile.Center.DirectionTo(Main.MouseWorld);
        }

        protected override void CreateDustOnSpawn()
        {
            for (int i = 1; i < 10; i++)
            {
                var d = Dust.NewDustDirect(
                    Projectile.Center + _initialVelocity * 40,
                    Projectile.width,
                    Projectile.height,
                    DustID.AncientLight,
                    newColor: ColorHelper.ColorWithAlpha255(InitialColor),
                    Scale: Main.rand.NextFloat(0.5f, 2f));
                d.velocity = WoomyMathHelper.AddRotationToVector2(_initialVelocity * 10, Main.rand.NextFloat(-30, 30)) * Main.rand.NextFloat(0.25f, 0.5f) * i;
                d.noGravity = true;
            }
        }

        protected override void PlayShootSound()
        {
            SoundHelper.PlayAudio(SoundID.Splash, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 3, pitch: 0.2f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item45, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 3, pitch: 0.5f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item21, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 3, pitch: 0.5f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item176, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 3, pitch: 0.8f, position: Projectile.Center);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case stateSearch:
                    Projectile.tileCollide = false;
                    break;
                case stateAttack:
                    Projectile.tileCollide = false;
                    break;
                case stateFlyOut:
                    Projectile.tileCollide = true;
                    break;
            }
        }

        public override void AI()
        {
            switch (state)
            {
                case stateSearch:
                    Projectile.velocity = Vector2.Zero;

                    if (_hitTargets.Count == 0)
                    {
                        _currentTarget = FindTarget(Main.MouseWorld, 250, _hitTargets);

                        if (_currentTarget == null || !Collision.CanHitLine(Projectile.Center, 1, 1, _currentTarget.Center, 1, 1))
                        {
                            Projectile.velocity = _initialVelocity;
                            SetState(stateFlyOut);
                            return;
                        }
                    }
                    else
                    {
                        _currentTarget = FindTarget(Projectile.Center, 400, _hitTargets);
                    }

                    if (_currentTarget != null)
                    {
                        SetState(stateAttack);
                    }
                    else
                    {
                        Projectile.Kill();
                    }

                    break;

                case stateAttack:
                    if (_currentTarget == null || !_currentTarget.active || _currentTarget.life <= 0)
                    {
                        SetState(stateSearch);
                    }
                    else
                    {
                        Projectile.position += Projectile.position.DirectionTo(_currentTarget.Center);
                    }

                    break;

                case stateFlyOut:
                    Projectile.timeLeft -= FrameSpeed() / 20;
                    break;
            }

            if (timeSpentAlive > FrameSpeed() * 0.2f && timeSpentAlive % 4 == 0)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.Honey2,
                    Velocity: Main.rand.NextVector2Circular(0.5f, 0.5f),
                    newColor: Color.White,
                    Scale: 1.2f);
                d.noGravity = true;

                if (Main.rand.NextBool(20))
                {
                    d = Dust.NewDustPerfect(
                        Position: Projectile.Center,
                        Type: DustID.Honey2,
                        Velocity: Main.rand.NextVector2Circular(2f, 2f),
                        newColor: Color.White,
                        Scale: 0.8f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            SetState(stateSearch);

            Projectile.position = target.Center;
            _hitTargets.Add(target.whoAmI);

            if (_hitTargets.Count == _maxTargets)
            {
                Projectile.Kill();
            }

            for (var i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.Honey,
                    Velocity: Main.rand.NextVector2Circular(3f, 3f),
                    newColor: Color.White,
                    Scale: 1.2f);
                d.noGravity = true;
            }

            for (var i = 0; i < 5; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.AmberBolt,
                    Velocity: Main.rand.NextVector2Circular(10f, 10f),
                    newColor: Color.White,
                    Scale: 2f);
                d.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (var i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.Honey,
                    Velocity: Main.rand.NextVector2Circular(3f, 3f),
                    newColor: Color.White,
                    Scale: 1.2f);
                d.noGravity = true;
            }

            return true;
        }

        private NPC? FindTarget(Vector2 position, float maxTargetDistance, List<int> excludedTargets)
        {
            NPC? npcTarget = null;

            float closestDistance = maxTargetDistance;
            foreach (var npc in Main.ActiveNPCs)
            {
                float distance = position.Distance(npc.Center);
                if (distance < closestDistance && IsTargetEnemy(npc, true) && !excludedTargets.Contains(npc.whoAmI) && !npc.wet)
                {
                    closestDistance = distance;
                    npcTarget = npc;
                }
            }

            return npcTarget;
        }
    }
}
