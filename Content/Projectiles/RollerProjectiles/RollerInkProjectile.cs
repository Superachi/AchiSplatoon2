using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.RollerProjectiles
{
    internal class RollerInkProjectile : BaseProjectile
    {
        private float _delayUntilFall;
        private float _fallSpeed;
        private float drawScale;
        private float drawRotation;
        private bool visible;
        private float damageFalloffMod = 1f;
        private int _damageReductionDelay;
        public string RollerSwingId;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = FrameSpeedMultiply(20);
        }

        protected override void AfterSpawn()
        {
            Initialize();

            // Set visuals
            _delayUntilFall = FrameSpeedMultiply(30);
            _fallSpeed = 0.5f;

            Projectile.frame = 0; //Main.rand.Next(0, Main.projFrames[Projectile.type]);
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale += Main.rand.NextFloat(0.8f, 1.6f);
        }

        public override void AI()
        {
            var owner = GetOwner();
            if (!visible && Vector2.Distance(owner.Center, Projectile.Center) > 30)
            {
                visible = true;
            }

            if (timeSpentAlive > FrameSpeedMultiply(10))
            {
                if (damageFalloffMod > 0.5f)
                {
                    damageFalloffMod -= 0.004f;
                }
            }

            // Rotation increased by velocity.X 
            drawRotation += Math.Sign(Projectile.velocity.X) * 0.05f;

            // Start falling eventually
            if (timeSpentAlive >= _delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += _fallSpeed;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                if (Projectile.velocity.Y < 0)
                {
                    ProjectileBounce(oldVelocity, new Vector2(0.8f, 0.2f));
                    return false;
                }
            }

            ProjectileDustHelper.ShooterTileCollideVisual(this);
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (visible)
            {
                DrawProjectile(inkColor: CurrentColor, rotation: drawRotation, scale: drawScale * damageFalloffMod, considerWorldLight: false);

                if (timeSpentAlive % FrameSpeedMultiply(2) == 0 && Main.rand.NextBool(5))
                {
                    DustHelper.NewDust(
                        dustType: ModContent.DustType<SplatterBulletDust>(),
                        position: Projectile.Center + Main.rand.NextVector2Circular(20, 20),
                        velocity: Projectile.velocity * FrameSpeed() / 4,
                        color: CurrentColor,
                        scale: 2f,
                        data: new(emitLight: true, scaleIncrement: -0.1f, gravity: 0.3f));
                }
            }

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.FinalDamage *= damageFalloffMod;

            target.immune[Projectile.owner] = 15;

            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.ModProjectile is RollerInkProjectile rollerProj
                    && projectile.identity != Projectile.identity
                    && projectile.owner == Projectile.owner)
                {
                    if (rollerProj.RollerSwingId == RollerSwingId)
                    {
                        rollerProj.Projectile.damage = (int)(Projectile.damage * DamageModifierAfterPierce);
                    }
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = (int)(30 * damageFalloffMod);
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            for (int i = 0; i < 10; i++)
            {
                DustHelper.NewDropletDust(
                    position: Projectile.Center,
                    velocity: Projectile.velocity / 3 + Main.rand.NextVector2Circular(3, 3),
                    color: CurrentColor,
                    minScale: 0.8f,
                    maxScale: 1.4f);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
