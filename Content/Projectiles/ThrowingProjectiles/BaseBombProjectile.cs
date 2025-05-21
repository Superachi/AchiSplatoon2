using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
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

        protected SlotId? throwAudio = null;

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

            throwAudio = PlayAudio(SoundPaths.SplatBombThrow.ToSoundStyle());

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
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;

            var a = new PlayAudioModel(SoundPaths.SplatBombDetonate, _volume: 0.6f, _pitchVariance: 0.2f, _maxInstances: 5, _position: Projectile.Center);
            var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, Projectile.damage, false);
            p.SetProperties(finalExplosionRadius, a);
            p.RunSpawnMethods();

            SoundHelper.StopSoundIfActive(throwAudio);
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
