using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles
{
    internal class HeavySplatlingProjectile : BaseProjectile
    {
        private int delayUntilFall = 20;
        private float fallSpeed = 0.1f;

        public bool chargedShot = false;
        private bool countedForBurst = false;

        private float pitch = 0f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();

            var weapon = (BaseSplatling)WeaponInstance;
            shootSample = weapon.ShootSample;

            if (weapon is HydraSplatling)
            {
                pitch = -0.5f;

                if (chargedShot)
                {
                    GameFeelHelper.ShakeScreenNearPlayer(GetOwner(), localOnly: true, strength: 1, duration: 5);
                }
            }
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayShootSound();
            Projectile.position += Main.rand.NextVector2Circular(8, 8);
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.3f;
            }

            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 2;
            fallSpeed *= 0.1f;
            delayUntilFall *= 2;
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.ShooterSpawnVisual(this);
        }

        public override void AI()
        {
            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += fallSpeed;
            }

            DustHelper.NewSplatterBulletDust(
                position: Projectile.Center,
                velocity: Projectile.velocity / 4,
                color: CurrentColor,
                scale: 1.4f
            );

            if (Main.rand.NextBool(20))
            {
                DustHelper.NewDropletDust(
                    position: Projectile.Center,
                    velocity: Projectile.velocity / 4,
                    color: CurrentColor,
                    scale: 1f);
            }
        }

        private HeavySplatlingCharge? GetParentModProjectile()
        {
            var p = GetParentProjectile(parentIdentity);
            if (p != null && p.ModProjectile is HeavySplatlingCharge)
            {
                return (HeavySplatlingCharge)p.ModProjectile;
            }
            return null;
        }

        protected override void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3, pitch: pitch);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Owner.GetModPlayer<AccessoryPlayer>().hasCrayonBox
                && target.life <= 0
                && IsTargetEnemy(target)
                && !NpcHelper.IsTargetAProjectile(target))
            {
                var proj = GetParentModProjectile();
                if (proj == null) return;
                if (proj.ChargedAmmo <= 0) return;

                proj.RestoreAmmo(0.5f);
                proj.crayonBoxChain++;

                Owner.GetModPlayer<HudPlayer>().SetOverheadText(
                    text: proj.crayonBoxChain == 1 ? $"{proj.crayonBoxChain} splat!" : $"{proj.crayonBoxChain} splats!",
                    displayTime: 60,
                    color: ColorHelper.LerpBetweenColorsPerfect(Color.Red.IncreaseHueBy(proj.crayonBoxChain * 5), Color.White, 0.2f));

                var pitchValue = Math.Min(2f / 12f + proj.crayonBoxChain / 24f, 12f / 12f);
                PlayAudio(SoundID.ResearchComplete, volume: 0.5f, pitch: pitchValue, maxInstances: 1);
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
        }

        protected override void CreateDustOnDespawn()
        {
            if (!IsThisClientTheProjectileOwner())
            {
                Projectile.position += Projectile.velocity;
                ProjectileDustHelper.ShooterTileCollideVisual(this);
            }
        }
    }
}