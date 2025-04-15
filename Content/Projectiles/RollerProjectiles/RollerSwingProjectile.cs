using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.RollerProjectiles
{
    internal class RollerSwingProjectile : BaseProjectile
    {
        protected override bool ConsumeInkAfterSpawn => false;

        private SoundStyle swingSample;
        private SoundStyle windUpSample;

        private const int stateInit = 0;
        private const int stateWindUp = 1;
        private const int stateSwing = 2;
        private const int stateRolling = 3;
        private int stateTimer = 0;

        private int windUpTime = 20;
        private readonly int childVelocity = 20;
        private bool startedJumpSwing = false;

        private float groundWindUpDelayMod;
        private float groundAttackVelocityMod;
        private float jumpWindUpDelayMod;
        private float jumpAttackVelocityMod;
        private float jumpAttackDamageMod;

        private readonly int swingTime = 15;
        private int facingDirection;

        private Player owner;
        private WeaponPlayer weaponPlayer;

        private float rollInkCost = 0f;
        private string _swingId = "";

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
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseRoller)WeaponInstance;
            swingSample = weaponData.SwingSample;
            windUpSample = weaponData.WindUpSample;

            groundWindUpDelayMod = weaponData.GroundWindUpDelayModifier;
            groundAttackVelocityMod = weaponData.GroundAttackVelocityModifier;

            jumpWindUpDelayMod = weaponData.JumpWindUpDelayModifier;
            jumpAttackDamageMod = weaponData.JumpAttackDamageModifier;
            jumpAttackVelocityMod = weaponData.JumpAttackVelocityModifier;

            rollInkCost = weaponData.InkCost / 60;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            ApplyWeaponInstanceData();
            Initialize(isDissolvable: false);

            owner = GetOwner();
            weaponPlayer = owner.GetModPlayer<WeaponPlayer>();

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
            var squidMP = owner.GetModPlayer<SquidPlayer>();

            if (owner.dead || squidMP.IsSquid() || PlayerHelper.IsPlayerImmobileViaDebuff(owner))
            {
                Projectile.Kill();
                return;
            }

            owner.heldProj = Projectile.whoAmI;
            stateTimer++;
            wormDamageReduction = false;

            switch (state)
            {
                case stateWindUp:
                    StateWindUp();
                    break;
                case stateSwing:
                    StateSwing();
                    break;
                case stateRolling:
                    wormDamageReduction = true;
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
                    float damageMod = Math.Min(0.5f + playerVelocity / 5, 3);
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
            }
            else
            {
                modifiers.FinalDamage *= 1f;
            }

            modifiers.HitDirectionOverride = Math.Sign(target.position.X - owner.position.X);
        }

        protected override void AfterKill(int timeLeft)
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
                    }
                    else
                    {
                        windUpTime = (int)(windUpTime * groundWindUpDelayMod);
                    }

                    Projectile.friendly = false;
                    PlayAudio(windUpSample, volume: 0.1f, pitchVariance: 0.1f, maxInstances: 5, pitch: -0.1f);
                    break;
                case stateSwing:
                    ConsumeInk();
                    _swingId = Main.time.ToString();

                    Projectile.friendly = true;
                    PlayAudio(swingSample, volume: 0.15f, pitchVariance: 0.1f, maxInstances: 5);
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
            }
            else
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

            int minTime = 2;
            if (stateTimer >= minTime && stateTimer < 6)
            {
                Player p = GetOwner();

                var vecFromPlayer = Main.MouseWorld.DirectionFrom(p.Center);
                float i = stateTimer - minTime;
                float velocityMult = 3.2f + (i * 0.4f);
                Vector2 velocity = vecFromPlayer * velocityMult;

                // Make it so thrown projectiles always match the roller's direction,
                // disregarding whether the player moves the mouse to the other side
                if (Math.Sign(velocity.X) != facingDirection) velocity.X *= -1;

                int damage = Projectile.damage;
                if (startedJumpSwing)
                {
                    damage = (int)(damage * jumpAttackDamageMod);
                    velocity *= jumpAttackVelocityMod;
                }
                else
                {
                    velocity *= groundAttackVelocityMod;
                }

                var proj = CreateChildProjectile<RollerInkProjectile>(p.Center, velocity, damage);
                proj.RollerSwingId = _swingId;
            }

            if (stateTimer > swingTime)
            {
                AdvanceState();
            }
        }

        protected void StateRolling()
        {
            if (!InputHelper.GetInputMouseLeftHold() || owner.GetModPlayer<InventoryPlayer>().HasHeldItemChanged())
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
                        var finalXVel = Math.Sign(owner.velocity.X) * AbsPlayerSpeed() / 4;

                        if (Main.rand.NextBool(4))
                        {
                            DustHelper.NewDropletDust(
                                position: new Vector2(owner.Center.X + facingDirection * 64, owner.position.Y + owner.height) + posRand,
                                velocity: new Vector2(finalXVel, -AbsPlayerSpeed()),
                                color: GenerateInkColor(),
                                minScale: 0.8f,
                                maxScale: 1.6f);
                        }
                    }

                    Dust.NewDustPerfect(
                        Position: new Vector2(owner.Center.X + facingDirection * 64, owner.position.Y + owner.height),
                        Type: ModContent.DustType<SplatterBulletLastingDust>(),
                        Velocity: new Vector2(0, AbsPlayerSpeed() * Main.rand.NextFloat(-1, 1)),
                        Alpha: Main.rand.Next(0, 32),
                        newColor: GenerateInkColor(),
                        Scale: Main.rand.NextFloat(0.5f, 1f));
                }

                var inkTankPlayer = owner.GetModPlayer<InkTankPlayer>();
                ConsumeInk(rollInkCost);

                if (inkTankPlayer.HasNoInk())
                {
                    inkTankPlayer.CreateLowInkPopup();
                    Projectile.Kill();
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

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(
                Color.White,
                Projectile.rotation,
                spriteOverride: TextureAssets.Item[itemIdentifier].Value,
                flipSpriteSettings: facingDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            return false;
        }
    }
}
