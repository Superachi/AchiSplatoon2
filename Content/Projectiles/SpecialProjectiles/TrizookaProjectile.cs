using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class TrizookaProjectile : BaseProjectile
    {
        public int shotNumber = 0;
        protected override bool CountDamageForSpecialCharge { get => false; }

        private const float explosionRadius = 200;
        private int finalExplosionRadius;
        private const float explosionTime = 6f;
        protected int damageBeforePiercing;

        private float delayUntilFall = 0;
        private float fallSpeed = 0;
        private float terminalVelocity = 0;
        private float orbitDistance = 0;

        private Vector2 _hitboxLocation;
        private List<int> _hitTargets = new List<int>();

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
        }

        protected float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as TrizookaSpecial;

            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
        }

        protected override void AfterSpawn()
        {
            _hitboxLocation = Projectile.Center;

            Initialize();
            ApplyWeaponInstanceData();

            damageBeforePiercing = Projectile.damage;

            delayUntilFall = 20f;
            fallSpeed = 0.1f;
            terminalVelocity = 18f;

            Projectile.penetrate = 2;
        }

        private void SetHitboxLocation()
        {
            double degrees = timeSpentAlive * 3 + shotNumber * 120;
            double radians = degrees * (Math.PI / 180);
            double distance = orbitDistance;

            _hitboxLocation.X = Projectile.Center.X - (int)(Math.Cos(radians) * distance);
            _hitboxLocation.Y = Projectile.Center.Y - (int)(Math.Sin(radians) * distance);
        }

        private void EmitTrailInkDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust.NewDustPerfect(
                    _hitboxLocation,
                    ModContent.DustType<BlasterTrailDust>(),
                    Main.rand.NextVector2Circular(1, 1),
                    255,
                    CurrentColor,
                    Main.rand.NextFloat(minScale, maxScale));
            }

            if (Main.rand.NextBool(5))
            {
                var d = Dust.NewDustPerfect(
                    _hitboxLocation,
                    DustID.AncientLight,
                    Main.rand.NextVector2Circular(5, 5),
                    255,
                    CurrentColor,
                    Main.rand.NextFloat(minScale / 2, maxScale / 2));
                d.noGravity = true;
            }

            if (Main.rand.NextBool(15))
            {
                var d = Dust.NewDustPerfect(
                    _hitboxLocation,
                    DustID.FireworksRGB,
                    -Projectile.velocity / 2 + Main.rand.NextVector2Circular(3, 3),
                    255,
                    CurrentColor,
                    Main.rand.NextFloat(minScale / 2, maxScale / 2));
                d.noGravity = true;
            }
        }

        protected void Explode()
        {
            if (Projectile.Center.Distance(Owner.Center) < 200)
            {
                GameFeelHelper.ShakeScreenNearPlayer(Owner, true, strength: 3, speed: 4, duration: 20);
            }

            var p = CreateChildProjectile<BlastProjectile>(
                position: _hitboxLocation,
                velocity: Vector2.Zero,
                damage: Projectile.damage);

            var s = new PlayAudioModel(_soundPath: SoundPaths.Silence, _volume: 0f);
            p.SetProperties(radius: finalExplosionRadius, audioModel: s, ignoredTargets: _hitTargets);
            p.colorOverride = colorOverride;
            p.RunSpawnMethods();

            var volumeMod = 0.7f;
            PlayAudio(SoundID.Splash, volume: 0.1f * volumeMod, maxInstances: 10, pitch: -0.5f);
            PlayAudio(SoundID.Item38, volume: 0.2f * volumeMod, maxInstances: 10, pitch: -2f);
            PlayAudio(SoundPaths.BlasterExplosion.ToSoundStyle(), volume: 0.05f * volumeMod, pitchVariance: 0.2f, maxInstances: 10, pitch: -2f);
            PlayAudio(SoundPaths.TrizookaSplash.ToSoundStyle(), volume: 0.1f * volumeMod, pitchVariance: 0.2f, maxInstances: 10);

            Projectile.Kill();
        }

        protected override void AdvanceState()
        {
            base.AdvanceState();
            Timer = 0;
        }

        public override void AI()
        {
            if (timeSpentAlive > 20 && orbitDistance < 48) orbitDistance += 1f;
            SetHitboxLocation();

            switch (state)
            {
                case 0:
                    if (Timer >= delayUntilFall * FrameSpeed())
                    {
                        Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + fallSpeed, -terminalVelocity, terminalVelocity);

                        if (Projectile.velocity.Y >= 0)
                        {
                            Projectile.velocity.X *= 0.98f;
                        }
                    }

                    EmitTrailInkDust(dustMaxVelocity: 0.5f, amount: 4, minScale: 0.8f, maxScale: 3);

                    bool CheckSolid()
                    {
                        return Framing.GetTileSafely(_hitboxLocation).HasTile && Collision.SolidCollision(_hitboxLocation, Projectile.width, Projectile.height);
                    }

                    if (Timer > 600 || CheckSolid())
                    {
                        Explode();
                    }
                    break;
            }

            Timer += 1f;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 40;
            hitbox = new Rectangle((int)_hitboxLocation.X - size / 2, (int)_hitboxLocation.Y - size / 2, size, size);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.friendly) return false;

            var oldPosition = Projectile.Center;
            Projectile.Center = _hitboxLocation;
            bool? check = Rectangle.Intersect(target.Hitbox, Projectile.Hitbox) != Rectangle.Empty;
            if (check == true)
            {
                DebugHelper.PrintDebug(shotNumber);

                _hitTargets.Add(target.whoAmI);
                if (state == 0) Explode();
            }

            Projectile.Center = oldPosition;

            return check;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
