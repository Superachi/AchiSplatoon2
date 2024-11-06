using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles.NozzlenoseProjectiles
{
    internal class NozzlenoseProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private readonly bool canFall = false;

        private bool countedForBurst = false;
        private float damageIncreasePerHit;

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
            var weaponData = (L3Nozzlenose)WeaponInstance;

            shootSample = weaponData.ShootSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;

            damageIncreasePerHit = weaponData.DamageIncreasePerHit;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayShootSound();
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.3f;
            }
            
            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 3;
            fallSpeed *= 0.2f;
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.ShooterSpawnVisual(this);
        }

        public override void AI()
        {
            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }

            Color dustColor = initialColor;
            Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1.4f);

            if (Main.rand.NextBool(20))
            {
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this);
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DamageVariationScale *= 0.5f;

            var parent = Main.projectile[(int)Projectile.ai[2]];
            if (parent != null && parent.ModProjectile is NozzlenoseShooter)
            {
                // Prevent crits for the 1st/2nd hits, then dramatically increase the chance on the 3rd hit
                var parentModProj = (NozzlenoseShooter)parent.ModProjectile;

                if (parent.ai[2] < 2)
                {
                    modifiers.DisableCrit();
                }

                // Only apply the bonus if the target matches the NPC hit by the first bullet
                if (parentModProj.NPCTarget == target.whoAmI)
                {
                    modifiers.FlatBonusDamage += Projectile.damage * (parent.ai[2] * 0.5f);

                    if (parent.ai[2] >= 2)
                    {
                        Projectile.CritChance *= 5;
                        Projectile.knockBack *= 3;
                    }
                }
            }
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!countedForBurst)
            {
                countedForBurst = true;
                var parent = Main.projectile[(int)Projectile.ai[2]];
                var parentModProj = (NozzlenoseShooter)parent.ModProjectile;

                if (parent != null)
                {
                    if (parent.ModProjectile is NozzlenoseShooter)
                    {
                        // Make the first bullet save the ID of the target
                        if (parent.ai[2] == 0 && parentModProj.NPCTarget == -1)
                        {
                            parentModProj.NPCTarget = target.whoAmI;
                        }

                        // Increment the counter per bullet hit
                        if (parentModProj.NPCTarget == target.whoAmI)
                        {
                            parent.ai[2]++;
                            parent.ai[2] = Math.Clamp(parent.ai[2]++, 0, 3);
                        }
                    }

                    // Display a visual effect on the third consecutive hit
                    if (parent.ai[2] == 3)
                    {
                        TripleHitDustBurst(target.Center);
                    }
                }
            }
        }

        protected override void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
        }
    }
}