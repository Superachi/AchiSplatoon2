using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.Projectiles.BrushProjectiles;

namespace AchiSplatoon2.Content.Projectiles.RollerProjectiles
{
    internal class RollerSwingProjectile : BaseProjectile
    {
        private string swingSample;
        private string windUpSample;

        private const int stateInit = 0;
        private const int stateWindUp = 1;
        private const int stateSwing = 2;
        private const int stateRolling = 3;
        private int stateTimer = 0;

        private int windUpTime = 20;
        private bool startedJumpSwing = false;
        private float jumpWindUpTimeMod = 1.5f;
        private float jumpSwingVelocityMod = 2f;
        private float jumpSwingDamageMod = 1.5f;

        private int swingTime = 15;
        private int swingTimeCurrent = 0;
        private int groundedTime = 0;
        private int facingDirection;

        Player owner;

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 36000;

            //DrawOriginOffsetX = -Projectile.width / 2;
            //DrawOriginOffsetY = -Projectile.height / 2;
        }

        protected float SwingAngleDegrees
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseRoller;
            swingSample = weaponData.SwingSample;
            windUpSample = weaponData.WindUpSample;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            ApplyWeaponInstanceData();
            Initialize(isDissolvable: false);

            owner = GetOwner();
            enablePierceDamagefalloff = false;
            wormDamageReduction = false;

            facingDirection = Math.Sign(Main.MouseWorld.X - owner.Center.X);
            if (facingDirection == 1) SwingAngleDegrees = 180;
            else SwingAngleDegrees = 0;

            if (!IsPlayerGrounded())
            {
                startedJumpSwing = true;
            }

            AdvanceState();
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            stateTimer = 0;
            switch (state)
            {
                case stateWindUp:
                    if (startedJumpSwing)
                    {
                        startedJumpSwing = true;
                        windUpTime = (int)(windUpTime * jumpWindUpTimeMod);
                    }

                    Projectile.friendly = false;
                    PlayAudio(windUpSample, pitchVariance: 0.1f, maxInstances: 5);
                    break;
                case stateSwing:
                    Projectile.friendly = true;
                    PlayAudio(swingSample, pitchVariance: 0.1f, maxInstances: 5);
                    break;
                case stateRolling:
                    break;
            }
        }

        private bool IsPlayerGrounded()
        {
            var p = GetOwner();
            return p.velocity.Y == 0 && p.oldVelocity.Y == 0;
        }

        private void RollerUpdateRotate()
        {
            // if (Projectile.friendly) VisualizeRadius();
            Player p = GetOwner();

            float deg = (SwingAngleDegrees);
            float rad = MathHelper.ToRadians(deg);
            float distanceFromPlayer = 48f;

            Projectile.position.X = (p.Center.X) - (int)(Math.Cos(rad) * distanceFromPlayer) - Projectile.width / 2;
            Projectile.position.Y = (p.Center.Y) - (int)(Math.Sin(rad) * distanceFromPlayer) - Projectile.height / 2;
        }

        private void RollerSwingRotate(float angleDestinationDegrees, float lerpAmount)
        {
            RollerUpdateRotate();
            Player p = GetOwner();

            // DebugHelper.PrintInfo($"{SwingAngleDegrees}");
            SwingAngleDegrees = MathHelper.Lerp(SwingAngleDegrees, angleDestinationDegrees, lerpAmount);

            var dirToPlayer = Projectile.Center
                .DirectionTo(p.Center)
                .ToRotation();
            Projectile.rotation = dirToPlayer + MathHelper.ToRadians(45 + 180);
        }

        protected void StateWindUp()
        {
            if (facingDirection == 1) RollerSwingRotate(15, 0.15f);
            else RollerSwingRotate(160, 0.15f);

            if (stateTimer > windUpTime) AdvanceState();
        }

        private float VectorToDegrees(Vector2 vector)
        {
            var rad = vector.ToRotation();
            return MathHelper.ToDegrees(vector.ToRotation());
        }

        private Vector2 DegreesToVector(float degrees)
        {
            return MathHelper.ToRadians(degrees).ToRotationVector2();
        }

        protected void StateSwing()
        {
            if (facingDirection == 1) RollerSwingRotate(200, 0.25f);
            else RollerSwingRotate(-40, 0.15f);

            if (stateTimer >= 2 && stateTimer < 6)
            {
                float velocityRandMod = 1f;
                if (startedJumpSwing)
                {
                    velocityRandMod = 0.25f;
                }
                Player p = GetOwner();

                var vecFromPlayer = Main.MouseWorld.DirectionFrom(p.Center);
                float velocityMult = stateTimer + 2f;

                Vector2 velocity = vecFromPlayer * velocityMult;
                velocity += Main.rand.NextVector2Circular(1, 0.5f) * velocityRandMod;

                // Make it so thrown projectiles always match the roller's direction,
                // disregarding whether the player moves the mouse to the other side
                if (Math.Sign(velocity.X) != facingDirection) velocity.X *= -1;

                int damage = Projectile.damage;
                if (startedJumpSwing)
                {
                    damage = (int)(Projectile.damage * jumpSwingDamageMod);
                    velocity *= jumpSwingVelocityMod;
                }
                CreateChildProjectile<RollerInkProjectile>(p.Center, velocity, damage);

                if (Main.rand.NextBool(3))
                {
                    velocity += Main.rand.NextVector2Circular(1, 0.5f) * velocityRandMod;
                    CreateChildProjectile<RollerInkProjectile>(p.Center, velocity, damage);
                }
            }

            if (stateTimer > swingTime) AdvanceState();
        }

        protected void StateRolling()
        {
            if (!InputHelper.GetInputMouseLeftHold())
            {
                Projectile.Kill();
                return;
            }

            RollerUpdateRotate();
            var p = GetOwner();
            if (p.velocity.X != 0)
            {
                Projectile.friendly = true;
            } else
            {
                Projectile.friendly = false;
            }
        }

        public override void AI()
        {
            Player owner = GetOwner();
            owner.heldProj = Projectile.whoAmI;
            stateTimer++;

            switch (state)
            {
                case stateWindUp:
                    StateWindUp();
                    break;
                case stateSwing:
                    StateSwing();
                    break;
                case stateRolling:
                    StateRolling();
                    break;
            }

            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f));
            Vector2 armPosition = owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2);
        }
    }
}
