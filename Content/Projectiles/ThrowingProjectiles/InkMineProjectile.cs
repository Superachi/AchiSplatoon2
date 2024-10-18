using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class InkMineProjectile : BaseProjectile
    {
        private const int stateSpawn = 0;
        private const int stateAwait = 1;
        private const int stateActivate = 2;
        private const int stateExplode = 3;

        private int detectionRadius;
        private int fuseTime = 0;
        private int delayUntilExplosion = 30;
        protected int explosionRadius;

        private float circleDrawMod;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.timeLeft = 36000;
        }

        public override void ApplyWeaponInstanceData()
        {
            var weaponData = WeaponInstance as InkMine;
            detectionRadius = weaponData.DetectionRadius;
            explosionRadius = weaponData.ExplosionRadius;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            detectionRadius = (int)(detectionRadius * explosionRadiusModifier);
            enablePierceDamagefalloff = false;
            wormDamageReduction = true;

            PlayAudio("Throwables/SprinklerDeployNew", volume: 0.2f, maxInstances: 10, position: Projectile.Center);
            SetState(stateSpawn);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case stateSpawn:
                    Projectile.velocity = Vector2.Zero;
                    DetonateOtherMines();
                    circleDrawMod = 0f;
                    break;

                case stateAwait:
                    circleDrawMod = 0.8f;
                    break;

                case stateActivate:
                    PlayAudio("Throwables/InkMineActivate", volume: 0.2f, pitchVariance: 0.05f, maxInstances: 10, position: Projectile.Center);
                    break;

                case stateExplode:
                    var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, Projectile.damage, false);
                    p.SetProperties(
                        explosionRadius,
                        new PlayAudioModel("Throwables/InkMineDetonate", _volume: 0.5f, _pitchVariance: 0.3f, _maxInstances: 10, _position: Projectile.Center));
                    p.AfterSpawn();
                    Projectile.timeLeft = 6;
                    break;
            }
        }

        public override void AI()
        {
            switch (state)
            {
                case stateSpawn:
                    circleDrawMod = MathHelper.Lerp(circleDrawMod, 0.8f, 0.2f);
                    if (timeSpentAlive > 30) SetState(stateAwait);
                    InkMineRadiusDust();
                    break;

                case stateAwait:
                    if (timeSpentAlive % 6 == 0 && FindClosestEnemy(detectionRadius) != null)
                    {
                        SetState(stateActivate);
                    }

                    if (Projectile.timeLeft < 30)
                    {
                        SetState(stateExplode);
                    }

                    InkMineRadiusDust();
                    break;

                case stateActivate:
                    fuseTime++;
                    if (fuseTime >= delayUntilExplosion)
                    {
                        SetState(stateExplode);
                    }

                    circleDrawMod *= 0.96f;
                    SpawnActivationDust();
                    InkMineRadiusDust();
                    break;
            }
        }

        private void DetonateOtherMines()
        {
            List<InkMineProjectile> activeMines = new();

            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.ModProjectile is InkMineProjectile
                    && projectile.identity != Projectile.identity
                    && projectile.owner == Projectile.owner)
                {
                    activeMines.Add(projectile.ModProjectile as InkMineProjectile);
                }
            }

            if (activeMines.Count > 1)
            {
                activeMines = activeMines.OrderBy(x => x.timeSpentAlive).ToList();
                activeMines.RemoveRange(0, 1);

                foreach (var mine in activeMines)
                {
                    if (mine.state < stateActivate)
                    {
                        mine.SetState(stateActivate);
                    }
                }
            }
        }

        private void SpawnActivationDust()
        {
            Color color = initialColor;
            Dust a = Dust.NewDustPerfect(
                Position: Projectile.Center + Main.rand.NextVector2Circular(detectionRadius * circleDrawMod, detectionRadius * circleDrawMod),
                Type: ModContent.DustType<ChargerBulletDust>(),
                Velocity: new Vector2(0, Main.rand.NextFloat(-1, -4)),
                Alpha: 0,
                newColor: color,
                Scale: Main.rand.NextFloat(1f, 1.5f));
        }

        private void SpawnInkMineDust(Vector2 offset)
        {
            float scale = 1.2f + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * 3)) * 0.2f;
            Color color = initialColor;
            Dust a = Dust.NewDustPerfect(
                Position: Projectile.Center + offset,
                Type: ModContent.DustType<SplatterBulletDust>(),
                Velocity: Vector2.Zero,
                Alpha: 0,
                newColor: color,
                Scale: scale);
        }

        private void InkMineRadiusDust()
        {
            float spaceBetweenDust = 15;
            for (int i = 0; i < 7; i++)
            {
                float rotation = MathHelper.ToRadians((timeSpentAlive * 2 + i * spaceBetweenDust) % 360);
                Vector2 offset = rotation.ToRotationVector2() * detectionRadius * circleDrawMod;

                SpawnInkMineDust(offset);
                SpawnInkMineDust(-offset);
            }
        }
    }
}
