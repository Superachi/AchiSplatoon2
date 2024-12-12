using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class CoralStringerProjectile : TriStringerProjectile
    {
        private readonly bool countedForBurst = false;

        private Vector2 startingVelocity;
        public int sineDirection = 0;
        private float currentRadians;
        private float amplitude = 0;
        private int sineCooldown = 0;

        private bool _upgradedWeapon = false;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 20;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 80 * FrameSpeed();
            Projectile.tileCollide = true;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            UpdateCurrentColor(parentFullyCharged ? CurrentColor.IncreaseHueBy(-20) : CurrentColor);

            sineCooldown = 3 * FrameSpeed();

            if (parentFullyCharged)
            {
                Projectile.penetrate += 3;
                sineDirection = -GetOwner().direction;
            }
            else
            {
                sineDirection = GetOwnerModPlayer<StatisticsPlayer>().attacksUsed % 2 == 0 ? 1 : -1;
            }

            if (_upgradedWeapon)
            {
                Projectile.timeLeft *= 2;
            }
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();

            if (WeaponInstance is CoralStringer coralStringer)
            {
                _upgradedWeapon = coralStringer.CanShotBounce;
            };
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.25f;
            }

            Projectile.extraUpdates *= 2;
            Projectile.timeLeft *= 2;
        }

        protected override void CreateDustOnSpawn()
        {
        }

        protected override void CreateDustOnDespawn()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(2f, 2f),
                    Type: parentFullyCharged ? DustID.CorruptSpray : DustID.HallowSpray,
                    Velocity: Main.rand.NextVector2CircularEdge(1f, 1f),
                    newColor: CurrentColor.IncreaseHueBy(Main.rand.Next(-10, 10)),
                    Scale: Main.rand.NextFloat(0.4f, 1.2f));
            }
        }

        public override void AI()
        {
            float startingRadians = startingVelocity.ToRotation();
            float frequency = Projectile.velocity.Length() * 2;

            if (amplitude < Projectile.velocity.Length() * 0.6f)
            {
                amplitude += 0.01f;
            }

            currentRadians = startingRadians + (float)Math.Sin(MathHelper.ToRadians(timeSpentAlive * frequency + 90) * sineDirection);
            Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, currentRadians * amplitude);

            if (timeSpentAlive % 2 == 0 && timeSpentAlive > FrameSpeed() / 2 && Main.rand.NextBool(5))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: parentFullyCharged ? DustID.CorruptSpray : DustID.HallowSpray,
                    Velocity: Projectile.velocity * 5,
                    newColor: Color.White,
                    Scale: Main.rand.NextFloat(0.5f, 1f),
                    Alpha: 1);

                d.noGravity = true;
            }

            if (timeSpentAlive % 20 * FrameSpeed() == 0 && Main.rand.NextBool(parentFullyCharged ? 10 : 30))
            {
                Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2CircularEdge(1, 1),
                    Type: Main.rand.Next(110, 113),
                    Velocity: new Vector2(0, Main.rand.NextFloat(-2, -5)),
                    newColor: CurrentColor.IncreaseHueBy(Main.rand.Next(-20, 20)),
                    Scale: Main.rand.NextFloat(0.4f, 0.8f));
            }

            if (timeSpentAlive % 30 * FrameSpeed() == 0 && Main.rand.NextBool(parentFullyCharged ? 10 : 30))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2CircularEdge(1, 1),
                    Type: DustID.AncientLight,
                    Velocity: Projectile.velocity / 4,
                    newColor: CurrentColor.IncreaseHueBy(Main.rand.Next(-20, 20)),
                    Scale: Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (_upgradedWeapon)
            {
                ProjectileBounce(oldVelocity, Vector2.One);
                return false;
            }

            PlayAudio(SoundID.NPCDeath9, volume: 0.2f, maxInstances: 10, pitch: 0f);
            PlayAudio(SoundID.Item86, volume: 0.2f, maxInstances: 10, pitch: 0.5f);

            return true;
        }
    }
}
