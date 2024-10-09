using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class BrushSwingProjectile : BaseProjectile
    {
        private Texture2D weaponSprite;

        private Player owner;
        private InkWeaponPlayer weaponPlayer;

        private const int stateInit = 0;
        private const int stateWindup = 1;
        private const int stateSwingForward = 2;
        private const int stateSwingBack = 3;
        private const int stateRoll = 4;

        private int facingDirection;
        private bool enableWindUp = false;
        private float weaponUseTime = 8;
        private int swingsUntilRoll = 8;
        private float swingAngleCurrent;
        private float swingArc = 120;
        private float swingAngleGoal;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 36000;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20 * FrameSpeed();
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
            SetSwingAngleFromMouse(direction: 1);
        }

        private float GetMouseAngle()
        {
            return MathHelper.ToDegrees(owner.DirectionFrom(Main.MouseWorld).ToRotation());
        }

        private void SetSwingAngleFromMouse(int direction)
        {
            facingDirection = Math.Sign(Main.MouseWorld.X - owner.Center.X);
            swingAngleCurrent = GetMouseAngle() + (swingArc / 2) * direction * -facingDirection;
            swingAngleGoal = swingAngleCurrent + swingArc * direction * facingDirection;
        }

        public override void AI()
        {
            switch (state)
            {
                case stateInit:
                case stateWindup:
                    AdvanceState();
                    break;
                case stateSwingForward:
                case stateSwingBack:
                    StateSwing();

                    if (timeSpentInState > weaponUseTime)
                    {
                        bool isSwingingForward = state == stateSwingForward;
                        int direction = isSwingingForward ? -1 : 1;
                        SetSwingAngleFromMouse(direction);
                        SetState(isSwingingForward ? stateSwingBack : stateSwingForward);
                    }
                    break;
            }

            var armRotateDeg = 135f;
            if (facingDirection == -1) armRotateDeg = -135f;
            owner.direction = facingDirection;
            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(armRotateDeg));
        }

        private void StateSwing()
        {
            RollerSwingRotate(swingAngleGoal, 3f / (float)weaponUseTime);

            if (timeSpentInState == (int)(weaponUseTime / 2))
            {
                CreateChildProjectile<InkbrushProjectile>(owner.Center, owner.DirectionTo(Main.MouseWorld) * 10, Projectile.damage);
            }

            if (timeSpentInState > weaponUseTime)
            {
                if (!InputHelper.GetInputMouseLeftHold())
                {
                    Projectile.Kill();
                }
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
            if (state < stateSwingForward) return false;

            DrawProjectile(
                Color.White,
                Projectile.rotation,
                spriteOverride: TextureAssets.Item[itemIdentifier].Value,
                flipSpriteSettings: facingDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            Utils.DrawBorderString(Main.spriteBatch, $"swingAngleCurrent: {swingAngleCurrent}\n swingArc: {swingArc}\n swingAngleGoal: {swingAngleGoal}", owner.Center - Main.screenPosition + new Vector2(0, -200), Color.White);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.HitDirectionOverride = Math.Sign(target.position.X - GetOwner().position.X);
        }
    }
}
