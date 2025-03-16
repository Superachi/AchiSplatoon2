using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Prefixes.SlosherPrefixes;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class SlosherMainProjectile : BaseProjectile
    {
        private readonly int weaponDamage;
        private bool hasChildren = false;

        public Dictionary<string, List<int>> targetsToIgnore = new Dictionary<string, List<int>>();
        private string _timestamp = "";

        private Vector2 _initialAngle = new Vector2(0, 0);
        private float _initialShotSpeed = 0f;
        private int _initialUseTime = 0;

        private int _ammo = 0;
        private int _ammoStart = 0;
        private int _repetitions = 0;
        private int _repetitionsStart = 0;
        private float _individualShotDeviation = 0;
        private int _repetitionCooldown = 0;

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseSlosher)WeaponInstance;

            shootSample = weaponData.ShootSample;
            shootAltSample = weaponData.ShootWeakSample;

            _ammo = weaponData.ShotCount;
            _repetitions = weaponData.Repetitions;
            _individualShotDeviation = weaponData.AimDeviation / 10f;
        }

        protected override void ApplyWeaponPrefixData()
        {
            base.ApplyWeaponPrefixData();
            var prefix = PrefixHelper.GetWeaponPrefixById(weaponSourcePrefix);

            if (prefix is BaseSlosherPrefix slosherWeaponPrefix)
            {
                _ammo += slosherWeaponPrefix.AmmoBonus;
                _ammoStart = _ammo;

                _repetitions += slosherWeaponPrefix.RepetitionBonus;
                _repetitionsStart = _repetitions;
            }
        }

        private void SetInitialAngle()
        {
            _initialAngle = Owner.DirectionTo(Main.MouseWorld);
            Owner.direction = _initialAngle.X > 0 ? 1 : -1;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            _ammoStart = _ammo;
            _repetitionsStart = _repetitions;
            _timestamp = DateTime.Now.ToString("dd-mm-ss-fff");

            _initialUseTime = Owner.itemTime;
            _initialShotSpeed = Projectile.velocity.Length() / 2;
            SetInitialAngle();
            Projectile.velocity = Vector2.Zero;

            if (Main.rand.NextBool(2))
            {
                PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 5, position: Owner.Center);
            }
            else
            {
                PlayAudio(shootAltSample, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 5, position: Owner.Center);
            }

            PlayAudio(SoundID.Item1, volume: 0.6f, pitchVariance: 0.2f, maxInstances: 5, position: Owner.Center);
            PlayAudio(SoundID.Item7, volume: 1f, pitchVariance: 0.2f, maxInstances: 5, position: Owner.Center);
            PlayAudio(SoundID.Item20, volume: 0.2f, pitch: 0.4f, pitchVariance: 0.2f, maxInstances: 5, position: Owner.Center);
        }

        public override void AI()
        {
            Projectile.Center = Owner.Center + new Vector2(10 * Owner.direction, 0);
            if (!Collision.CanHitLine(Projectile.Center, 0, 0, Owner.Center, 0, 0))
            {
                Projectile.Center = Owner.Center;
            }

            if (_repetitionCooldown > 0)
            {
                _repetitionCooldown--;
                return;
            }

            if (timeSpentAlive > 10 && timeSpentAlive % 2 == 0 && _ammo > 0)
            {
                _ammo--;

                Vector2 mouseAngle = Owner.DirectionTo(Main.MouseWorld);
                Vector2 childVel = _initialAngle * (1 + _initialShotSpeed) * (1f + _ammo / 40f);

                var yellowChipCount = Owner.GetModPlayer<ColorChipPlayer>().ColorChipAmounts[(int)ChipColor.Yellow];
                if (yellowChipCount > 0)
                {
                    childVel *= 1f - (0.1f * yellowChipCount / 2);
                }

                if (IsThisClientTheProjectileOwner())
                {
                    hasChildren = true;

                    var proj = CreateChildProjectile<SlosherChildProjectile>(
                        position: Projectile.Center,
                        velocity: childVel,
                        damage: Projectile.damage,
                        triggerSpawnMethods: false);

                    proj._delayUntilFall = (_ammo + 1) * 2;
                    proj.maxScale = 0.5f + (_ammo / 8f);

                    var sweepingArc = 5;
                    var startAngleOffset = -50;
                    var sweepingArcMult = Owner.direction * Math.Abs(mouseAngle.X);
                    var addedAngle = (_ammo * sweepingArc + startAngleOffset) * sweepingArcMult;

                    proj.Projectile.velocity = WoomyMathHelper.AddRotationToVector2(proj.Projectile.velocity, addedAngle);
                    proj.Projectile.velocity *= 1 + (_repetitions / 10f);
                    proj.Projectile.velocity += Owner.velocity * new Vector2(0.3f, 0.15f);
                    proj.Projectile.velocity += Main.rand.NextVector2Circular(_individualShotDeviation, _individualShotDeviation);

                    proj.parentTimestamp = _timestamp;
                    if (targetsToIgnore.TryGetValue(_timestamp, out List<int> targetsIgnoredByChild))
                    {
                        proj.targetsToIgnore = targetsIgnoredByChild;
                    }
                    proj.RunSpawnMethods();
                }

                if (_ammo == 0)
                {
                    _repetitionCooldown = (int)(_initialUseTime / Math.Max(1f, _repetitionsStart + 0.5f)) - _ammoStart;
                }
            }

            if (_ammo == 0)
            {
                if (_repetitionCooldown == 0)
                {
                    if (_repetitions > 0)
                    {
                        _repetitions--;
                        _ammo = _ammoStart;
                        _timestamp = DateTime.Now.ToString("dd-mm-ss-fff");
                        SetInitialAngle();

                        if (Main.rand.NextBool(2))
                        {
                            PlayAudio(shootSample, volume: 0.08f, pitch: 0.6f, pitchVariance: 0.3f, maxInstances: 5, position: Owner.Center);
                        }
                        else
                        {
                            PlayAudio(shootAltSample, volume: 0.08f, pitch: 0.6f, pitchVariance: 0.3f, maxInstances: 5, position: Owner.Center);
                        }

                        PlayAudio(SoundID.Item7, volume: 0.7f, pitchVariance: 0.2f, maxInstances: 5, position: Owner.Center);
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            if (hasChildren)
            {
                modifiers.FinalDamage *= 0;
                modifiers.HideCombatText();
                Projectile.Kill();
            }
        }
    }
}
