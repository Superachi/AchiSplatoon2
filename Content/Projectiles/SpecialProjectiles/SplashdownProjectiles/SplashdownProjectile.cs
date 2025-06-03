using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System.Globalization;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.SplashdownProjectiles
{
    internal class SplashdownProjectile : BaseProjectile
    {
        protected override bool FallThroughPlatforms => false;

        private const int _stateRise = 0;
        private const int _stateHold = 1;
        private const int _stateFall = 2;
        private const int _stateExplode = 3;

        private float _playerSpinSpeed;
        private float _fallDurationDamageMod;

        protected int _explosionRadius;
        protected int _finalExplosionRadius;

        private bool _summonFists;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            SetState(_stateRise);

            enablePierceDamagefalloff = false;
            wormDamageReduction = true;
            SummonFists();

            _fallDurationDamageMod = 1f;
            _playerSpinSpeed = 0.8f;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (Splashdown)WeaponInstance;

            _explosionRadius = weaponData.ExplosionRadius;
            _summonFists = weaponData.SummonFists;
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            switch (state)
            {
                case _stateRise:
                    Projectile.velocity.Y = -30f;

                    SoundHelper.PlayAudio(SoundPaths.InkjetBoost.ToSoundStyle(), volume: 0.8f, maxInstances: 5, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundPaths.SplashdownCharge.ToSoundStyle(), volume: 0.5f, maxInstances: 5, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundPaths.SplashdownChargeWarning.ToSoundStyle(), volume: 0.4f, maxInstances: 5, position: Projectile.Center);

                    for (int i = 0; i < 15; i++)
                    {
                        var d = DustHelper.NewDust(
                            Projectile.Center,
                            DustID.PortalBolt,
                            color: CurrentColor,
                            velocity: Main.rand.NextVector2CircularEdge(10, 10),
                            scale: Main.rand.NextFloat(1.2f, 2f),
                            data: new(scaleIncrement: -0.2f, gravity: 0));
                    }

                    break;

                case _stateHold:
                    var sparkle = CreateChildProjectile<StillSparkleVisual>(Owner.Center, Vector2.Zero, 0, true);
                    sparkle.UpdateCurrentColor(CurrentColor);
                    sparkle.AdjustRotation(MathHelper.ToRadians(0), Owner.direction * 0.05f);
                    sparkle.AdjustScale(1.5f);

                    Owner.immuneNoBlink = true;

                    break;

                case _stateFall:
                    Projectile.friendly = true;

                    SoundHelper.PlayAudio(SoundID.Item45, volume: 0.7f, maxInstances: 5, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.Item152, volume: 0.4f, maxInstances: 5, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.DD2_JavelinThrowersAttack, volume: 0.7f, pitch: 0.2f, maxInstances: 5, position: Projectile.Center);

                    break;

                case _stateExplode:
                    // Mechanic
                    Owner.immune = true;
                    Owner.immuneTime = 90;
                    Owner.immuneNoBlink = false;

                    Projectile.damage = MultiplyProjectileDamage(_fallDurationDamageMod);
                    Projectile.velocity = Vector2.Zero;

                    var a = new PlayAudioModel(SoundPaths.Silence, _volume: 0f, _maxInstances: 50, _position: Projectile.Center);
                    var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, Projectile.damage, false);
                    _finalExplosionRadius = (int)(_explosionRadius * MathHelper.Lerp(explosionRadiusModifier, 1, 0.99f));
                    p.SetProperties(_finalExplosionRadius, a);
                    p.RunSpawnMethods();

                    // Audio/visual
                    GameFeelHelper.ShakeScreenNearPlayer(Owner, true, strength: 10, speed: 5, duration: 15);

                    SoundHelper.PlayAudio(SoundPaths.SplashdownLanding.ToSoundStyle(), volume: 0.7f, maxInstances: 5, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.Item38, volume: 0.5f, pitch: -1f, maxInstances: 5, position: Projectile.Center);

                    break;
            }
        }

        public override void PostAI()
        {
            base.PostAI();
            Owner.Center = Projectile.Center;
            Owner.velocity = Vector2.Zero;
        }

        public override void AI()
        {
            if (Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            Owner.Center = Projectile.Center;
            Owner.velocity = Vector2.Zero;

            var weaponPlayer = Owner.GetModPlayer<WeaponPlayer>();
            weaponPlayer.CustomWeaponCooldown = 15;

            switch (state)
            {
                case _stateRise:
                    _playerSpinSpeed = MathHelper.Lerp(_playerSpinSpeed, 0f, 0.05f);
                    Owner.fullRotation += _playerSpinSpeed;
                    Owner.fullRotationOrigin = new Vector2(10f, 20f);

                    Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 0, 0.13f);

                    if (timeSpentInState > FrameSpeedMultiply(36))
                    {
                        Projectile.velocity.Y = 0f;
                        Owner.fullRotation = 0;
                        AdvanceState();
                    }

                    break;

                case _stateHold:
                    Owner.immune = true;
                    Owner.immuneTime = 2;

                    if (timeSpentInState > FrameSpeedMultiply(20))
                    {
                        AdvanceState();
                    }

                    break;

                case _stateFall:
                    Owner.immune = true;
                    Owner.immuneTime = 2;

                    if (Projectile.velocity.Y < 24f)
                    {
                        Projectile.velocity.Y += 3f;
                    }

                    if (timeSpentInState > FrameSpeedMultiply(20))
                    {
                        _fallDurationDamageMod += 0.012f;
                    }

                    if (timeSpentInState > FrameSpeedMultiply(120))
                    {
                        AdvanceState();
                    }

                    break;

                case _stateExplode:
                    Projectile.Kill();

                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == _stateFall)
            {
                AdvanceState();
                return false;
            }

            if (state < _stateFall)
            {
                return false;
            }

            var maxLoop = 100;
            var loop = 0;

            while (Collision.SolidCollision(Projectile.Center, Owner.width, Owner.height))
            {
                if (loop >= maxLoop)
                { 
                    break; 
                }

                loop++;
                Projectile.position.Y -= 2;
            }

            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0.5f;

            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (state == _stateFall)
            {
                PlayCorrectDirectDustBurst(target);
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        private void SummonFists()
        {
            if (!IsThisClientTheProjectileOwner())
            {
                return;
            }

            bool hasPunchingGlove = Owner.HasAccessory<PunchingGlove>();
            var fistDistance = 160f * (hasPunchingGlove ? 0.75f : 1f);

            if (_summonFists)
            {
                var p = CreateChildProjectile<SplashdownFistProjectile>(Owner.Center + new Vector2(-fistDistance, 0), Vector2.Zero, Projectile.damage, true);
                p.faceDir = -1;

                p = CreateChildProjectile<SplashdownFistProjectile>(Owner.Center + new Vector2(fistDistance, 0), Vector2.Zero, Projectile.damage, true);
                p.faceDir = 1;

                if (hasPunchingGlove)
                {
                    p = CreateChildProjectile<SplashdownFistProjectile>(Owner.Center + new Vector2(-fistDistance * 3, 0), Vector2.Zero, Projectile.damage, true);
                    p.faceDir = -1;
                    p.addedFallDelay = 12;

                    p = CreateChildProjectile<SplashdownFistProjectile>(Owner.Center + new Vector2(fistDistance * 3, 0), Vector2.Zero, Projectile.damage, true);
                    p.faceDir = 1;
                    p.addedFallDelay = 12;
                }
            }
        }
    }
}
