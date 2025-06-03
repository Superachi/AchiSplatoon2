using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.SplashdownProjectiles
{
    internal class SplashdownFistProjectile : BaseProjectile
    {
        protected override bool FallThroughPlatforms => false;

        private const int _stateRise = 0;
        private const int _stateHold = 1;
        private const int _stateFall = 2;
        private const int _stateExplode = 3;

        protected int _explosionRadius;
        protected int _finalExplosionRadius;

        public int faceDir;
        public int addedFallDelay;

        private float _addedRotation;
        private float _drawScale;
        private float _drawAlpha;
        private bool passing;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            SetState(_stateRise);

            enablePierceDamagefalloff = false;
            wormDamageReduction = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (Splashdown)WeaponInstance;
            _explosionRadius = weaponData.ExplosionRadius;
            _explosionRadius = (int)(_explosionRadius * 0.75f);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            switch (state)
            {
                case _stateRise:
                    Projectile.velocity.Y = -30f;

                    _drawScale = 1.5f;
                    _drawAlpha = 0f;

                    var maxLoop = 200;
                    var loop = 0;

                    while (Collision.SolidCollision(Projectile.Center, Projectile.width, Projectile.height))
                    {
                        if (loop >= maxLoop)
                        {
                            break;
                        }

                        loop++;
                        Projectile.position.Y -= 2;
                    }

                    break;

                case _stateHold:
                    _drawScale = 3f;
                    _drawAlpha = 0.6f;

                    var sparkle = CreateChildProjectile<StillSparkleVisual>(Projectile.Center, Vector2.Zero, 0, true);
                    sparkle.UpdateCurrentColor(CurrentColor);
                    sparkle.AdjustRotation(MathHelper.ToRadians(45), faceDir * 0.2f);
                    sparkle.AdjustScale(1.5f);

                    for (int i = 0; i < 10; i ++)
                    {
                        var d = DustHelper.NewDust(
                            Projectile.Center + Main.rand.NextVector2Circular(40, 40),
                            DustID.PortalBolt,
                            color: CurrentColor,
                            velocity: new Vector2(0, -Main.rand.NextFloat(3, 8)),
                            scale: Main.rand.NextFloat(1.2f, 2f),
                            data: new(scaleIncrement: -0.2f, gravity: 0));
                    }

                    break;

                case _stateFall:
                    Projectile.friendly = true;

                    break;

                case _stateExplode:
                    // Mechanic
                    Projectile.velocity = Vector2.Zero;

                    var a = new PlayAudioModel(SoundPaths.Silence, _volume: 0f, _maxInstances: 50, _position: Projectile.Center);
                    var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, Projectile.damage, false);
                    _finalExplosionRadius = (int)(_explosionRadius * MathHelper.Lerp(explosionRadiusModifier, 1, 0.99f));
                    p.SetProperties(_finalExplosionRadius, a);
                    p.RunSpawnMethods();

                    Projectile.friendly = false;
                    Projectile.damage = 0;

                    // Audio/visual
                    GameFeelHelper.ShakeScreenNearPlayer(Owner, true, strength: 6, speed: 5, duration: 15);

                    SoundHelper.PlayAudio(SoundPaths.SplashdownLanding.ToSoundStyle(), volume: 0.4f, pitchVariance: 0.2f, maxInstances: 5, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.Item38, volume: 0.2f, pitch: -0.6f, maxInstances: 5, position: Projectile.Center);

                    break;
            }
        }

        public override void AI()
        {
            switch (state)
            {
                case _stateRise:
                    _addedRotation = MathHelper.Lerp(_addedRotation, -70, 0.05f);
                    Projectile.rotation = MathHelper.ToRadians(faceDir * _addedRotation);
                    Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 0, 0.1f);

                    _drawAlpha = MathHelper.Lerp(_drawAlpha, 0.4f, 0.05f);

                    if (timeSpentInState > FrameSpeedMultiply(36))
                    {
                        Projectile.velocity.Y = 0f;
                        AdvanceState();
                    }

                    break;

                case _stateHold:
                    _drawScale = MathHelper.Lerp(_drawScale, 2f, 0.2f);

                    if (timeSpentInState > FrameSpeedMultiply(34 + addedFallDelay))
                    {
                        AdvanceState();
                    }

                    break;

                case _stateFall:
                    _addedRotation = MathHelper.Lerp(_addedRotation, 90, 0.3f);
                    Projectile.rotation = MathHelper.ToRadians(faceDir * _addedRotation);

                    if (Projectile.velocity.Y < 30f)
                    {
                        Projectile.velocity.Y += 3f;
                    }

                    if (timeSpentInState > FrameSpeedMultiply(120))
                    {
                        AdvanceState();
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        var d = DustHelper.NewSplatterBulletDust(
                            Projectile.Center + Main.rand.NextVector2Circular(32, 32),
                            new Vector2(0, -Main.rand.NextFloat(2, 8)),
                            CurrentColor,
                            Main.rand.NextFloat(2, 3));
                    }

                    break;

                case _stateExplode:
                    if (timeSpentInState > FrameSpeedMultiply(60))
                    {
                        Projectile.Kill();
                    }

                    _drawAlpha -= 0.1f;

                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == _stateFall)
            {
                AdvanceState();
            }

            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;

            if (Owner.Center.Y - 40 > Projectile.Center.Y && !Collision.SolidCollision(Projectile.Center, Projectile.width, Projectile.height))
            {
                return false;
            }

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

        public override bool PreDraw(ref Color lightColor)
        {
            if (state < _stateExplode)
            {
                DrawProjectile(
                    inkColor: CurrentColor,
                    rotation: Projectile.rotation,
                    scale: _drawScale,
                    alphaMod: _drawAlpha,
                    considerWorldLight: false,
                    additiveAmount: 0.8f,
                    flipSpriteSettings: faceDir == 1 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.None : Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally);
            }

            if (state > _stateRise)
            {
                var len = Projectile.oldPos.Length;
                for (int i = len - 1; i > 0; i--)
                {
                    var iMult = (float)(len - i) / len;
                    var posDiff = Projectile.oldPos[i] - Projectile.position;
                    var alphaMod = iMult * _drawAlpha * 0.5f;

                    DrawProjectile(
                        inkColor: CurrentColor,
                        rotation: Projectile.rotation,
                        scale: _drawScale,
                        alphaMod: alphaMod,
                        considerWorldLight: false,
                        additiveAmount: 0.8f,
                        flipSpriteSettings: faceDir == 1 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.None : Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally,
                        positionOffset: posDiff);
                }
            }

            return false;
        }
    }
}
