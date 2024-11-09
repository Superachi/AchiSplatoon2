using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class BaseBombProjectile : BaseProjectile
    {
        protected override bool FallThroughPlatforms => false;

        protected bool hasExploded = false;
        protected int explosionRadius;
        protected int finalExplosionRadius;

        protected int fallTimer = 0;
        protected int delayUntilFall = 15;
        protected float fallSpeed = 0.5f;
        protected bool canFall = false;

        protected float airFriction = 0.995f;
        protected float terminalVelocity = 20f;

        protected float brightness = 0.002f;
        protected float drawScale = 1f;

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBomb;

            explosionRadius = weaponData.ExplosionRadius;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
            enablePierceDamagefalloff = false;

            PlayAudio("Throwables/SplatBombThrow");

            if (IsThisClientTheProjectileOwner())
            {
                float distance = Vector2.Distance(Main.LocalPlayer.Center, Main.MouseWorld);
                float velocityMod = MathHelper.Clamp(distance / 250f, 0.4f, 1.2f);
                Projectile.velocity *= velocityMod;
                NetUpdate(ProjNetUpdateType.SyncMovement);
            }
        }

        protected void Detonate()
        {
            Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.position -= Projectile.velocity;
            Projectile.velocity = Vector2.Zero;

            var e = new ExplosionDustModel(_dustMaxVelocity: 25, _dustAmount: 30, _minScale: 2f, _maxScale: 4f, _radiusModifier: finalExplosionRadius);
            var a = new PlayAudioModel("Throwables/SplatBombDetonate", _volume: 0.6f, _pitchVariance: 0.2f, _maxInstances: 5);
            CreateExplosionVisual(e, a);
            StopAudio("Throwables/SplatBombFuse");
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!hasExploded)
            {
                DrawProjectile(inkColor: CurrentColor, rotation: Projectile.rotation, scale: drawScale, considerWorldLight: false, additiveAmount: 0.5f);
                return false;
            }
            return true;
        }

        protected void readSpawnVelocity(BinaryReader reader)
        {
            Projectile.velocity = reader.ReadVector2();
        }

        protected override void NetSendInitialize(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
        }

        protected override void NetReceiveInitialize(BinaryReader reader)
        {
            base.NetReceiveInitialize(reader);
            readSpawnVelocity(reader);
        }
    }
}
