using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

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
        private int childVelocity = 20;
        private bool startedJumpSwing = false;

        private float groundWindUpDelayMod;
        private float groundAttackVelocityMod;
        private float jumpWindUpDelayMod;
        private float jumpAttackVelocityMod;
        private float jumpAttackDamageMod;

        private int swingTime = 15;
        private int facingDirection;

        private Player owner;
        private InkWeaponPlayer weaponPlayer;

        protected float SwingAngleDegrees
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

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

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseRoller;
            swingSample = weaponData.SwingSample;
            windUpSample = weaponData.WindUpSample;

            groundWindUpDelayMod = weaponData.GroundWindUpDelayModifier;
            groundAttackVelocityMod = weaponData.GroundAttackVelocityModifier;

            jumpWindUpDelayMod = weaponData.JumpWindUpDelayModifier;
            jumpAttackDamageMod = weaponData.JumpAttackDamageModifier;
            jumpAttackVelocityMod = weaponData.JumpAttackVelocityModifier;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            ApplyWeaponInstanceData();
            Initialize(isDissolvable: false);

            owner = GetOwner();
            weaponPlayer = owner.GetModPlayer<InkWeaponPlayer>();

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

        public override void AI()
        {
            Player owner = GetOwner();
            if (owner.dead)
            {
                Projectile.Kill();
                return;
            }

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

            var armRotateDeg = 135f;
            if (facingDirection == -1) armRotateDeg = -135f;
            owner.direction = facingDirection;
            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(armRotateDeg));
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            if (state == stateRolling)
            {
                var playerVelocity = AbsPlayerSpeed();
                if (playerVelocity > 2)
                {
                    float damageMod = Math.Min(0.5f + playerVelocity / 3, 5);
                    if (!IsPlayerGrounded()) damageMod *= 0.5f;

                    modifiers.FinalDamage *= damageMod;
                    modifiers.Knockback += playerVelocity / 2;

                    if (damageMod >= 2.5f)
                    {
                        TripleHitDustBurst(playSample: false);
                    }
                }
                else
                {
                    modifiers.FinalDamage *= 0.5f;
                }
            } else
            {
                modifiers.FinalDamage *= 1f;
            }

            modifiers.HitDirectionOverride = Math.Sign(target.position.X - owner.position.X);
        }

        public override void OnKill(int timeLeft)
        {
            weaponPlayer.isUsingRoller = false;
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
                        windUpTime = (int)(windUpTime * jumpWindUpDelayMod);
                    } else
                    {
                        windUpTime = (int)(windUpTime * groundWindUpDelayMod);
                    }

                    Projectile.friendly = false;
                    PlayAudio(windUpSample, pitchVariance: 0.1f, maxInstances: 5, pitch: -0.1f);
                    break;
                case stateSwing:
                    Projectile.friendly = true;
                    PlayAudio(swingSample, pitchVariance: 0.1f, maxInstances: 5);
                    break;
                case stateRolling:
                    weaponPlayer.isUsingRoller = true;
                    break;
            }
        }

        protected void StateWindUp()
        {
            float lerpAmount = 0.15f;
            if (startedJumpSwing)
            {
                lerpAmount /= jumpWindUpDelayMod;
            } else
            {
                lerpAmount /= groundWindUpDelayMod;
            }

            if (facingDirection == 1) RollerSwingRotate(20, lerpAmount);
            else RollerSwingRotate(160, lerpAmount);

            if (stateTimer > windUpTime) AdvanceState();
        }

        protected void StateSwing()
        {
            float lerpAmount = 0.2f;
            if (facingDirection == 1) RollerSwingRotate(200, lerpAmount);
            else RollerSwingRotate(-25, lerpAmount);

            if (stateTimer >= 2 && stateTimer < 6)
            {
                Player p = GetOwner();

                var vecFromPlayer = Main.MouseWorld.DirectionFrom(p.Center);
                float i = stateTimer - 2;
                float velocityMult = 4f + i * (1 - i * 0.15f);
                Vector2 velocity = vecFromPlayer * velocityMult;

                // Make it so thrown projectiles always match the roller's direction,
                // disregarding whether the player moves the mouse to the other side
                if (Math.Sign(velocity.X) != facingDirection) velocity.X *= -1;

                int damage = Projectile.damage;
                if (startedJumpSwing)
                {
                    damage = (int)(Projectile.damage * jumpAttackDamageMod);
                    velocity *= jumpAttackVelocityMod;
                } else
                {
                    velocity *= groundAttackVelocityMod;
                }
                CreateChildProjectile<RollerInkProjectile>(p.Center, velocity, damage);
                CreateChildProjectile<RollerInkProjectile>(p.Center, velocity * 0.75f, damage);

                if (owner.HeldItem.ModItem is DynamoRoller)
                {
                    CreateChildProjectile<RollerInkProjectile>(p.Center, velocity * 1.25f, damage);
                }
            }

            if (stateTimer > swingTime) AdvanceState();
        }

        protected void StateRolling()
        {
            if (!InputHelper.GetInputMouseLeftHold() || owner.GetModPlayer<BaseModPlayer>().HasHeldItemChanged())
            {
                Projectile.Kill();
                return;
            }

            RollerUpdateRotate();
            if (AbsPlayerSpeed() >= 2)
            {
                Projectile.friendly = true;

                if (owner.velocity.Y == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var posRand = Main.rand.NextVector2Circular(12, 12);
                        var xVelocityRand = Main.rand.NextFloat(1, 3);
                        var finalXVel = Math.Sign(owner.velocity.X) * AbsPlayerSpeed() / 4;

                        Dust d = Dust.NewDustPerfect(
                            Position: new Vector2(owner.Center.X + facingDirection * 64, owner.position.Y + owner.height) + posRand,
                            Type: ModContent.DustType<SplatterDropletDust>(),
                            Velocity: new Vector2(finalXVel, -AbsPlayerSpeed()),
                            Alpha: Main.rand.Next(0, 32),
                            newColor: GenerateInkColor(),
                            Scale: Main.rand.NextFloat(0.8f, 1.6f));
                    }

                    Dust.NewDustPerfect(
                        Position: new Vector2(owner.Center.X + facingDirection * 64, owner.position.Y + owner.height),
                        Type: ModContent.DustType<SplatterBulletLastingDust>(),
                        Velocity: new Vector2(0, AbsPlayerSpeed() * Main.rand.NextFloat(-1, 1)),
                        Alpha: Main.rand.Next(0, 32),
                        newColor: GenerateInkColor(),
                        Scale: Main.rand.NextFloat(0.5f, 1f));
                }
            }
            else
            {
                Projectile.friendly = false;
            }
        }

        private float AbsPlayerSpeed()
        {
            return Math.Abs(owner.velocity.X);
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

            Projectile.gfxOffY = p.gfxOffY;
            Projectile.position.X = (p.Center.X) - (int)(Math.Cos(rad) * distanceFromPlayer) - Projectile.width / 2;
            Projectile.position.Y = (p.Center.Y) - (int)(Math.Sin(rad) * distanceFromPlayer) - Projectile.height / 2;
        }

        private void RollerSwingRotate(float angleDestinationDegrees, float lerpAmount)
        {
            RollerUpdateRotate();
            Player p = GetOwner();

            SwingAngleDegrees = MathHelper.Lerp(SwingAngleDegrees, angleDestinationDegrees, lerpAmount);

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
