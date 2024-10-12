using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class BrushSwingProjectile : BaseProjectile
    {
        private Texture2D weaponSprite;

        private Player owner;
        private InkWeaponPlayer weaponPlayer;

        private const int stateWindup = 0;
        private const int stateSwingForward = 1;
        private const int stateSwingBack = 2;
        private const int stateRoll = 3;

        private int facingDirection;

        private float WeaponUseTime() => baseWeaponUseTime * FrameSpeed();
        private float baseWeaponUseTime = 6f;
        private float shotVelocity;

        private float swingAngleCurrent;
        private float swingArc = 80;
        private float swingAngleGoal;
        private int windupTime;

        // How much time the player can stay in the air for, before the rolling state reverts to swinging
        private int rollCoyoteTime;
        private int RollCoyoteTimeDefault => 30 * FrameSpeed();

        private Vector2 drawPositionOffset;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 36000;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = (int)(WeaponUseTime()) + 2 * FrameSpeed();
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;
            if (weaponData == null) throw new InvalidCastException($"Tried casting {nameof(WeaponInstance)} to type {nameof(BaseBrush)}, but the result was null.");


            baseWeaponUseTime = weaponData.BaseWeaponUseTime / owner.GetAttackSpeed(DamageClass.Melee);
            baseWeaponUseTime = Math.Max(4, baseWeaponUseTime);
            shotVelocity = weaponData.ShotVelocity;

            swingArc = weaponData.SwingArc;
            windupTime = weaponData.WindupTime;

            shootSample = weaponData.ShootSample;
            shootAltSample = weaponData.ShootAltSample;
        }

        public override void AfterSpawn()
        {
            owner = GetOwner();
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            enablePierceDamagefalloff = false;
            wormDamageReduction = false;

            weaponPlayer = owner.GetModPlayer<InkWeaponPlayer>();

            Projectile.velocity = Vector2.Zero;
            SetSwingAngleFromMouse(direction: 1);
            SetState(stateWindup);
        }

        protected override void SetState(int targetState)
        {
            var wepMP = owner.GetModPlayer<InkWeaponPlayer>();
            base.SetState(targetState);

            switch (state)
            {
                case stateWindup:
                    if (windupTime == 0)
                    {
                        return;
                    }

                    Projectile.friendly = false;
                    wepMP.isBrushRolling = false;
                    weaponPlayer.isBrushAttacking = true;

                    PlayAudio("Rollers/SwingMedium", volume: 0.2f, maxInstances: 5, position: Projectile.Center, pitch: 1);
                    swingAngleCurrent = facingDirection == 1 ? 180 : 0;
                    break;

                case stateSwingForward:
                case stateSwingBack:
                    Projectile.friendly = true;
                    wepMP.isBrushRolling = false;
                    weaponPlayer.isBrushAttacking = true;
                    break;

                case stateRoll:
                    Projectile.friendly = false;
                    wepMP.isBrushRolling = true;
                    weaponPlayer.isBrushAttacking = false;
                    rollCoyoteTime = RollCoyoteTimeDefault;

                    if (swingAngleCurrent < -45)
                    {
                        swingAngleCurrent += 360;
                    }
                    break;
            }
        }

        public override void AI()
        {
            var baseMP = owner.GetModPlayer<BaseModPlayer>();
            var wepMP = owner.GetModPlayer<InkWeaponPlayer>();

            if (owner.dead || baseMP.HasHeldItemChanged())
            {
                Projectile.Kill();
                return;
            }

            switch (state)
            {
                case stateWindup:
                    StateWindup();
                    break;

                case stateSwingForward:
                case stateSwingBack:
                    StateSwing();
                    break;

                case stateRoll:
                    StateRoll();
                    break;
            }

            // Arm movement
            var armRotateDeg = 135f;
            if (facingDirection == -1) armRotateDeg = -135f;
            owner.direction = facingDirection;
            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(armRotateDeg));

            // Dust
            if (timeSpentAlive % 6 == 0)
            {
                Dust.NewDustPerfect(
                    Position: Projectile.Center + drawPositionOffset * -1.3f + Main.rand.NextVector2Circular(Projectile.width * 0.5f, Projectile.height * 0.5f),
                    Type: ModContent.DustType<ChargerBulletDust>(),
                    Velocity: Vector2.Normalize(Projectile.position - Projectile.oldPosition) * -3f,
                    newColor: initialColor, Scale: 1.5f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            var wepMP = owner.GetModPlayer<InkWeaponPlayer>();
            wepMP.isBrushRolling = false;
            wepMP.isBrushAttacking = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(
                Color.White,
                Projectile.rotation,
                spriteOverride: TextureAssets.Item[itemIdentifier].Value,
                flipSpriteSettings: facingDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                positionOffset: drawPositionOffset);

            Utils.DrawBorderString(Main.spriteBatch, $"SwingAngle = {swingAngleCurrent}", owner.Center - Main.screenPosition + new Vector2(0, -200), Color.White);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.HitDirectionOverride = Math.Sign(target.position.X - GetOwner().position.X);

            if (state == stateRoll)
            {
                modifiers.FinalDamage *= 2f;
                modifiers.Knockback *= 3f;
            }
        }

        #region State machine functions (AI loop)

        private void StateWindup()
        {
            if (windupTime == 0)
            {
                if (PlayerMeetsRollingRequirements())
                {
                    SetState(stateRoll);
                    return;
                }

                AdvanceState();
                return;
            }

            float lerpAmount = 0.03f;
            if (facingDirection == 1)
            {
                RollerSwingRotate(20, lerpAmount);
            }
            else
            {
                RollerSwingRotate(160, lerpAmount);
            }

            if (timeSpentInState > windupTime * FrameSpeed())
            {
                AdvanceState();
            }
        }

        private void StateSwing()
        {
            RollerSwingRotate(swingAngleGoal, 3f / WeaponUseTime());

            if (timeSpentInState == (int)(WeaponUseTime() / 2f * 0.8f))
            {
                Shoot();
            }

            if (timeSpentInState > WeaponUseTime())
            {
                if (!InputHelper.GetInputMouseLeftHold())
                {
                    Projectile.Kill();
                    return;
                }

                if (PlayerMeetsRollingRequirements())
                {
                    SetState(stateRoll);
                    return;
                }

                bool isSwingingForward = state == stateSwingForward;
                int direction = isSwingingForward ? -1 : 1;
                SetSwingAngleFromMouse(direction);
                SetState(isSwingingForward ? stateSwingBack : stateSwingForward);
            }
        }

        private void StateRoll()
        {
            if (!InputHelper.GetInputMouseLeftHold())
            {
                Projectile.Kill();
                return;
            }

            if (!IsPlayerGrounded())
            {
                rollCoyoteTime--;
            }
            else
            {
                rollCoyoteTime = RollCoyoteTimeDefault;
            }

            if (!IsMouseUnderneathPlayer() || rollCoyoteTime <= 0 || AbsPlayerSpeed() < float.Epsilon)
            {
                swingAngleCurrent = GetMouseAngle();
                if (windupTime > 0)
                {
                    SetState(stateWindup);
                    return;
                }

                SetState(stateSwingForward);
                return;
            }

            if (SignPlayerSpeed() != 0)
            {
                owner.direction = SignPlayerSpeed();
                facingDirection = owner.direction;
            }

            // Emit dust when running
            bool isFastEnough = AbsPlayerSpeed() >= 6;
            Projectile.friendly = isFastEnough;
            if (isFastEnough && IsPlayerGrounded() && Main.rand.NextBool(10))
            {
                var posRand = Main.rand.NextVector2Circular(12, 12);
                var xVelocityRand = Main.rand.NextFloat(1, 3);
                var finalXVel = SignPlayerSpeed() * AbsPlayerSpeed() / 4;

                Dust d = Dust.NewDustPerfect(
                    Position: new Vector2(owner.Center.X + facingDirection * 72, owner.position.Y + owner.height) + posRand,
                    Type: ModContent.DustType<SplatterDropletDust>(),
                    Velocity: new Vector2(finalXVel * 2, -AbsPlayerSpeed() * 0.7f),
                    Alpha: Main.rand.Next(0, 32),
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(0.8f, 1.6f));

                Dust.NewDustPerfect(
                    Position: new Vector2(owner.Center.X + facingDirection * 72, owner.position.Y + owner.height),
                    Type: ModContent.DustType<SplatterBulletLastingDust>(),
                    Velocity: new Vector2(0, AbsPlayerSpeed() * Main.rand.NextFloat(0, 1)),
                    Alpha: Main.rand.Next(0, 32),
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(0.8f, 1.2f));
            }

            float lerpAmount = 0.05f;
            if (facingDirection == 1) RollerSwingRotate(200, lerpAmount);
            else RollerSwingRotate(-20, lerpAmount);
        }

        #endregion

        #region Angle functions

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

        private bool IsMouseUnderneathPlayer()
        {
            return GetMouseAngle() < -50 && GetMouseAngle() > -120;
        }

        private bool PlayerMeetsRollingRequirements()
        {
            return IsMouseUnderneathPlayer() && IsPlayerGrounded() && AbsPlayerSpeed() > float.Epsilon;
        }

        #endregion

        private bool IsPlayerGrounded()
        {
            return owner.GetModPlayer<BaseModPlayer>().IsPlayerGrounded();
        }

        private void Shoot()
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                switch (WeaponInstance)
                {
                    case DesertBrush:
                        CreateChildProjectile<DesertBrushProjectile>(owner.Center, owner.DirectionTo(Main.MouseWorld) * shotVelocity + Main.rand.NextVector2Circular(i, i), Projectile.damage / 2);
                        break;
                    default:
                        CreateChildProjectile<InkbrushProjectile>(owner.Center, owner.DirectionTo(Main.MouseWorld) * shotVelocity + Main.rand.NextVector2Circular(i, i), Projectile.damage / 2);
                        break;
                }
            }

            if (Main.rand.NextBool(2))
            {
                PlayAudio(shootSample, volume: 0.08f, pitchVariance: 0.3f, maxInstances: 10);
            }
            else
            {
                PlayAudio(shootAltSample, volume: 0.08f, pitchVariance: 0.3f, maxInstances: 10);
            }
        }

        private int SignPlayerSpeed()
        {
            return Math.Sign(owner.velocity.X);
        }

        private float AbsPlayerSpeed()
        {
            return Math.Abs(owner.velocity.X);
        }

        private void RollerUpdateRotate()
        {
            // if (Projectile.friendly && Main.rand.NextBool(10)) VisualizeRadius();
            Player p = owner;

            float deg = (swingAngleCurrent);
            float rad = MathHelper.ToRadians(deg);
            float distanceFromPlayer = 64f;

            Projectile.gfxOffY = p.gfxOffY;
            Projectile.position.X = (p.Center.X) - (int)(Math.Cos(rad) * distanceFromPlayer) - Projectile.width / 2;
            Projectile.position.Y = (p.Center.Y) - (int)(Math.Sin(rad) * distanceFromPlayer) - Projectile.height / 2;

            float drawDistanceFromPlayer = 48f;
            drawPositionOffset = Projectile.position - new Vector2(
                (p.Center.X) - (int)(Math.Cos(rad) * drawDistanceFromPlayer) - Projectile.width / 2,
                (p.Center.Y) - (int)(Math.Sin(rad) * drawDistanceFromPlayer) - Projectile.height / 2
                );
            drawPositionOffset *= -1;
        }

        private void RollerSwingRotate(float angleDestinationDegrees, float lerpAmount)
        {
            RollerUpdateRotate();
            Player p = GetOwner();

            swingAngleCurrent = MathHelper.Lerp(swingAngleCurrent, angleDestinationDegrees, lerpAmount) % 360;

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
    }
}
