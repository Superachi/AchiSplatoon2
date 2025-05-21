using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class SlosherChildProjectile : BaseProjectile
    {
        public float maxScale = 1f;
        public float _delayUntilFall;
        private readonly float delayUntilFall = 3f;
        private float fallSpeed;
        private readonly float terminalVelocity = 12f;
        private float _damageFalloffMod = 1f;

        private Color bulletColor;
        private float drawScale = 0f;
        private readonly float drawRotation = 0f;
        public List<int> targetsToIgnore = new List<int>();
        public string parentTimestamp = "";

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 14;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseSlosher;

            fallSpeed = weaponData.ShotGravity;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            wormDamageReduction = true;

            Projectile.frame = 2;
            Projectile.rotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
        }

        public override void AI()
        {
            if (timeSpentAlive > 3 && IsThisClientTheProjectileOwner())
            {
                Projectile.friendly = true;
            }

            var checkSize = 4;
            if (timeSpentAlive < 4
                && IsThisClientTheProjectileOwner()
                && !Collision.CanHitLine(Projectile.Center, checkSize, checkSize, Owner.Center, checkSize, checkSize)
                && !Collision.CanHitLine(Projectile.Top, checkSize, checkSize, Owner.Top, checkSize, checkSize)
                && !Collision.CanHitLine(Projectile.Bottom, checkSize, checkSize, Owner.Bottom, checkSize, checkSize))
            {
                Projectile.Kill();
                return;
            }

            if (isFakeDestroyed) return;

            // Start falling eventually
            if (FrameSpeedMultiply(timeSpentAlive) >= FrameSpeedMultiply(_delayUntilFall))
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.985f;
                }

                if (_damageFalloffMod > 0.5f)
                {
                    _damageFalloffMod -= 0.002f;
                }
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            Projectile.rotation += Math.Sign(Projectile.velocity.X) * 0.05f;
            if (drawScale <= maxScale) drawScale += 0.1f;

            if (timeSpentAlive < FrameSpeedMultiply(10)
                && timeSpentAlive % FrameSpeedMultiply(3) == 0)
            {
                DustHelper.NewDust(
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    position: Projectile.Center,
                    velocity: Projectile.velocity * (FrameSpeed() / 2) + Main.rand.NextVector2Circular(1f, 1f),
                    color: CurrentColor,
                    scale: Main.rand.NextFloat(1f, 1.6f),
                    data: new(emitLight: true, scaleIncrement: -0.05f, gravity: 0.5f, gravityDelay: 10));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (isFakeDestroyed) return false;

            // Bounce against ceilings
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                if (Projectile.velocity.Y < 0)
                {
                    ProjectileBounce(oldVelocity, new Vector2(0.8f, 0.2f));
                    return false;
                }
            }

            Projectile.position += Projectile.velocity;
            ProjectileDustHelper.ShooterTileCollideVisual(this, volumeMod: 0.5f);

            if (IsThisClientTheProjectileOwner() && !NetHelper.IsSinglePlayer())
            {
                FakeDestroy();
                return false;
            }

            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!targetsToIgnore.Contains(target.whoAmI))
            {
                return base.CanHitNPC(target);
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            targetsToIgnore.Add(target.whoAmI);

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.owner == Projectile.owner
                    && proj.type == ModContent.ProjectileType<SlosherChildProjectile>())
                {
                    var modProj = (SlosherChildProjectile)proj.ModProjectile;
                    if (!modProj.targetsToIgnore.Contains(target.whoAmI)
                        && modProj.parentTimestamp == parentTimestamp)
                    {
                        modProj.targetsToIgnore.Add(target.whoAmI);
                    }

                    AddSelfToParentList(target);
                }
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        private void AddSelfToParentList(NPC target)
        {
            if (parentProjectile != null
                && parentProjectile.active
                && parentProjectile.type == ModContent.ProjectileType<SlosherMainProjectile>())
            {
                var parentProj = (SlosherMainProjectile)parentProjectile.ModProjectile;
                if (!parentProj.targetsToIgnore.ContainsKey(parentTimestamp))
                {
                    parentProj.targetsToIgnore.Add(parentTimestamp, new List<int>());
                }
                else
                {
                    var list = parentProj.targetsToIgnore[parentTimestamp];
                    list.Add(target.whoAmI);
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 32;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            var accMP = Owner.GetModPlayer<AccessoryPlayer>();
            bool hasCoil = accMP.hasSteelCoil;

            if (targetsToIgnore.Count == 0)
            {
                if (hasCoil)
                {
                    modifiers.FinalDamage *= AdamantiteCoil.BaseDamageMod;
                }
            }
            else
            {
                if (hasCoil)
                {
                    modifiers.FinalDamage *= AdamantiteCoil.PostFirstHitDamageMod;
                }
            }

            modifiers.FinalDamage *= _damageFalloffMod;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (isFakeDestroyed) return false;

            DrawProjectile(ColorHelper.ColorWithAlpha255(CurrentColor), Projectile.rotation, drawScale, alphaMod: 0.9f, considerWorldLight: false);

            return false;
        }
    }
}
