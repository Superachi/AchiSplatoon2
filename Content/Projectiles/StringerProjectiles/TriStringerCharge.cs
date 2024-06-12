using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using rail;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons;
using Mono.Cecil;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using System.Linq;

namespace AchiSplatoon2.Content.Projectiles.StringerProjectiles
{
    internal class TriStringerCharge : BaseProjectile
    {
        private bool hasFired = false;
        private int timeLeftAfterFiring = 120;

        protected virtual string ShootSample { get => "TriStringerShoot"; }
        protected virtual string ShootWeakSample { get => "BambooChargerShootWeak"; }
        protected virtual float[] ChargeTimeThresholds { get => [36f, 72f]; }
        private int chargeLevel = 0;
        private float maxChargeTime;
        protected virtual float ShotgunArc { get => 4f; }
        protected virtual int ProjectileCount { get => 3; }
        protected virtual bool AllowStickyProjectiles { get => true; }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            maxChargeTime = ChargeTimeThresholds.Last();

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 36000;
            Projectile.penetrate = 10;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Vector2.Zero;
            PlayAudio(soundPath: "ChargeStart");
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.channel && !hasFired)
            {
                var len = ChargeTimeThresholds.Length;
                if (chargeLevel < len)
                {
                    ++ChargeTime;
                    if (ChargeTime >= ChargeTimeThresholds[chargeLevel])
                    {
                        chargeLevel++;

                        for (int i = 0; i < 10; i++)
                        {
                            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                        }

                        PlayAudio(soundPath: "ChargeReady", volume: 0.3f, pitch: (chargeLevel - 1) * 0.2f, maxInstances: 1);

                        if (chargeLevel == len)
                        {
                            PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);
                        }
                    }
                } else
                {
                    if (Main.rand.NextBool(50))
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                    }
                }

                SyncProjectilePosWithPlayer(owner);
                PlayerItemAnimationFaceCursor(owner);
                return;
            }

            if (!hasFired)
            {
                hasFired = true;
                SyncProjectilePosWithWeaponBarrel(Projectile.position, Projectile.velocity, new TriStringer());

                // Adjust damage and velocity based on charge level
                float arcModifier = 2f;
                float velocityModifier = 0.75f;
                int projectileType = ModContent.ProjectileType<TriStringerProjectileWeak>();
                if (chargeLevel == 0)
                {
                    Projectile.damage /= 2;
                    PlayAudio("BambooChargerShootWeak");
                } else {
                    if (chargeLevel == 1)
                    {
                        Projectile.damage = (int)(Projectile.damage * 0.75);
                        velocityModifier = 1f;
                        arcModifier = 1.5f;
                    } else
                    {
                        velocityModifier = 1.5f;
                        arcModifier = 1f;
                    }

                    if (AllowStickyProjectiles) { projectileType = ModContent.ProjectileType<TriStringerProjectile>(); }
                    PlayAudio("TriStringerShoot");
                }

                // The angle that the player aims at (player-to-cursor)
                float aimAngle = MathHelper.ToDegrees(
                    owner.DirectionTo(Main.MouseWorld).ToRotation()
                );

                if (ChargeTime == 0) return; // Prevent division by 0, though we shouldn't end up with this anyway

                float finalArc = Math.Clamp(ShotgunArc * (maxChargeTime / ChargeTime) * arcModifier, ShotgunArc, ShotgunArc * 5);
                float degreesPerProjectile = finalArc / ProjectileCount;
                int middleProjectile = ProjectileCount / 2;
                float degreesOffset = -(middleProjectile * degreesPerProjectile);

                for (int i = 0; i < ProjectileCount; i++)
                {
                    // Convert angle: degrees -> radians -> vector
                    float degrees = aimAngle + degreesOffset;
                    float radians = MathHelper.ToRadians(degrees);
                    Vector2 angleVector = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
                    Vector2 velocity = angleVector * velocityModifier; // * (0.95f + i * 0.05f);

                    // Spawn projectile
                    int proj = Projectile.NewProjectile(
                        spawnSource: Projectile.GetSource_FromThis(),
                        position: Projectile.position,
                        velocity: velocity,
                        Type: projectileType,
                        Damage: Projectile.damage,
                        KnockBack: Projectile.knockBack,
                        Owner: Main.myPlayer);

                    // Set a number in the arrow
                    // Can be used to make them explode in sequence
                    Main.projectile[proj].ai[2] = i;

                    // Adjust the angle for the next projectile
                    degreesOffset += degreesPerProjectile;
                }

                PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);
                Projectile.Kill();
                return;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (!hasFired) return;
        }
    }
}