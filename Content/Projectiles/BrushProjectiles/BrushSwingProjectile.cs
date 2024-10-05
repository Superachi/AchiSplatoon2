using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class BrushSwingProjectile : BaseProjectile
    {
        private Texture2D weaponSprite;

        private Player owner;
        private InkWeaponPlayer weaponPlayer;

        private const int stateInit = 0;
        private const int stateWindup = 1;
        private const int stateSwingLoop = 2;
        private const int stateRoll = 3;

        private int facingDirection;
        private bool enableWindUp = false;
        private float weaponUseTime = 30;
        private int swingsUntilRoll = 8;
        private float swingAngleCurrent;
        private float swingArc = 90;
        private float swingAngleGoal;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;

            
        }

        public override void AfterSpawn()
        {
            ApplyWeaponInstanceData();
            owner = GetOwner();
            weaponPlayer = owner.GetModPlayer<InkWeaponPlayer>();

            enablePierceDamagefalloff = false;
            wormDamageReduction = false;

            Projectile.velocity = Vector2.Zero;

            facingDirection = Math.Sign(Main.MouseWorld.X - owner.Center.X);
            float mouseAngle = MathHelper.ToDegrees(owner.DirectionTo(Main.MouseWorld).ToRotation());

            swingAngleCurrent = mouseAngle + (swingArc / 2 + 180) * -facingDirection;
            swingAngleGoal = swingAngleCurrent + swingArc * facingDirection;
        }

        public override void AI()
        {
            switch (state)
            {
                case stateInit:
                    RollerSwingRotate(swingAngleGoal, 0.3f);
                    break;
            }
        }

        private void RollerUpdateRotate()
        {
            // if (Projectile.friendly) VisualizeRadius();
            Player p = owner;

            float deg = (swingAngleCurrent);
            float rad = MathHelper.ToRadians(deg);
            float distanceFromPlayer = 48f;

            Projectile.gfxOffY = p.gfxOffY;
            Projectile.position.X = (p.Center.X) - (int)(Math.Cos(rad) * distanceFromPlayer) - Projectile.width / 2;
            Projectile.position.Y = (p.Center.Y) - (int)(Math.Sin(rad) * distanceFromPlayer) - Projectile.height / 2;
        }

        private void RollerSwingRotate(float angleDestinationDegrees, float lerpAmount)
        {
            RollerUpdateRotate();
            Player p = GetOwner();

            swingAngleCurrent = MathHelper.Lerp(swingAngleCurrent, angleDestinationDegrees, lerpAmount);

            var dirToPlayer = Projectile.Center
                .DirectionTo(p.Center)
                .ToRotation();

            Projectile.spriteDirection = facingDirection;
            if (facingDirection == 1)
            {
                Projectile.rotation = dirToPlayer + MathHelper.ToRadians(45 + 180);
            }
            else
            {
                Projectile.rotation = dirToPlayer + MathHelper.ToRadians(45 - 90);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Utils.DrawBorderString(Main.spriteBatch, $"swingAngleCurrent: {swingAngleCurrent}\n swingArc: {swingArc}\n swingAngleGoal: {swingAngleGoal}", Projectile.Center - Main.screenPosition + new Vector2(0, -200), Color.White);
            return base.PreDraw(ref lightColor);
        }
    }
}
