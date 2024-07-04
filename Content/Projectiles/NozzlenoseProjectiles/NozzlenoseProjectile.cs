using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.NozzlenoseProjectiles
{
    internal class NozzlenoseProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private bool canFall = false;
        private float terminalVelocity = 6f;

        private bool countedForBurst = false;
        private float damageIncreasePerHit;

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

        public override void AfterSpawn()
        {
            Initialize();

            L3Nozzlenose weaponData = (L3Nozzlenose)weaponSource;
            shootSample = weaponData.ShootSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;

            damageIncreasePerHit = weaponData.DamageIncreasePerHit;

            PlayShootSound();
        }

        public override void AI()
        {
            Timer++;
            if (Timer >= FrameSpeed(delayUntilFall))
            {
                if (!canFall)
                {
                    canFall = true;
                    NetUpdate(ProjNetUpdateType.SyncMovement, true);
                }
            }

            if (canFall)
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }

            Color dustColor = GenerateInkColor();
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = (Projectile.velocity.X + random) * -0.5f;
                float velY = (Projectile.velocity.Y + random) * -0.5f;
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DamageVariationScale *= 0.5f;

            var parent = Main.projectile[(int)Projectile.ai[2]];
            if (parent.ModProjectile is NozzlenoseShooter)
            {
                // Prevent crits for the 1st/2nd hits, then dramatically increase the chance on the 3rd hit
                var parentModProj = parent.ModProjectile as NozzlenoseShooter;

                if (parent.ai[2] < 2)
                {
                    modifiers.DisableCrit();
                }

                // Only apply the bonus if the target matches the NPC hit by the first bullet
                if (parentModProj.NPCTarget == target.whoAmI)
                {
                    modifiers.FlatBonusDamage += (Projectile.damage * (parent.ai[2] * 0.5f));

                    if (parent.ai[2] >= 2) {
                        Projectile.CritChance *= 5;
                        Projectile.knockBack *= 3;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!countedForBurst)
            {
                countedForBurst = true;
                var parent = Main.projectile[(int)Projectile.ai[2]];
                var parentModProj = parent.ModProjectile as NozzlenoseShooter;

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

        private void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
        }

        // Netcode
        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.Write(canFall);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            canFall = reader.ReadBoolean();
        }
    }
}