using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaPelletProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;

        private bool countedForBurst = false;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrella;

            shootSample = weaponData.ShootSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.4f;
            }

            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 2;
            fallSpeed *= 0.5f;
            delayUntilFall *= 2;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
        }

        public override void AI()
        {
            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }

            if (timeSpentAlive > 6)
            {
                Color dustColor = CurrentColor;

                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1.2f);

                if (Main.rand.NextBool(20))
                {
                    DustHelper.NewDropletDust(
                        position: Projectile.Center,
                        velocity: Projectile.velocity / 4,
                        color: dustColor,
                        scale: 0.8f);
                }
            }
        }

        protected override void CreateDustOnSpawn()
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (!countedForBurst)
            {
                countedForBurst = true;
                var parentProj = GetParentProjectile(parentIdentity);

                if (parentProj.ModProjectile is BrellaShotgunProjectile)
                {
                    var parentModProj = parentProj.ModProjectile as BrellaShotgunProjectile;
                    if (parentModProj.burstNPCTarget == -1)
                    {
                        parentModProj.burstNPCTarget = target.whoAmI;
                    }

                    if (parentModProj.burstNPCTarget == target.whoAmI)
                    {
                        parentModProj.burstHitCount++;
                    }

                    if (parentModProj.burstHitCount == parentModProj.burstRequiredHits)
                    {
                        TripleHitDustBurst(target.Center);
                        parentProj.Kill();
                    }
                }
            }

            // In the original Splatoon game, Undercover Brella instantly regains its shield when splatting an opponent
            var heldItem = GetOwner().HeldItem.ModItem;
            bool isUndercover = heldItem is UndercoverBrella;
            var brellaMP = GetOwner().GetModPlayer<BrellaPlayer>();
            if (target.life < 1 && !brellaMP.shieldAvailable && isUndercover)
            {
                brellaMP.shieldCooldown = 1;
            }
        }
    }
}
