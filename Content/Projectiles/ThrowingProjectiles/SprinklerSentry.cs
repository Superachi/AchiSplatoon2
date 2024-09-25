using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.EelSplatana;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class SprinklerSentry : BaseBombProjectile
    {
        private Vector2 lockedPosition;
        private float previousVelocityX;
        private float previousVelocityY;
        private float prevX;
        private float prevY;
        private float prevXdiff;
        private float prevYdiff;

        private float xFacing = 0;
        private float yFacing = 0;

        private bool hasCollided = false;
        private bool sticking = false;
        private bool isStickingVertically;
        private bool fallback = false;
        private Vector2 stickingDirection = new Vector2(0, 0);

        private const int stateFly = 0;
        private const int stateGetStickAxis = 1;
        private const int stateGetStickDirection = 2;
        private const int stateWait = 3;
        private const int stateFire = 4;
        private const int stateShrink = 5;

        private const int baseAttackTime = 5;
        private int shotsLeft = 75;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 12;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600 * FrameSpeed();
            Projectile.tileCollide = true;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -12;
        }


        protected float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        protected float ShotsFired
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void AfterSpawn()
        {
            Initialize();

            terminalVelocity = terminalVelocity / FrameSpeed();
            airFriction = 0.999f;

            PlayAudio("Throwables/SplatBombThrow");

            if (IsThisClientTheProjectileOwner())
            {
                float distance = Vector2.Distance(Main.LocalPlayer.Center, Main.MouseWorld);
                float velocityMod = MathHelper.Clamp(distance / 250f, 0.4f, 1.2f);
                Projectile.velocity *= velocityMod;
                NetUpdate(ProjNetUpdateType.SyncMovement);
            }
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case stateFire:
                    DespawnOtherSprinklers();
                    break;
            }
        }

        public override void AI()
        {
            bool debug = false;
            Lighting.AddLight(Projectile.position, initialColor.R * brightness, initialColor.G * brightness, initialColor.B * brightness);

            // Apply gravity
            if (state == stateFly || fallback)
            {
                Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + (0.03f / FrameSpeed()), -terminalVelocity, terminalVelocity);
            }

            switch (state)
            {
                case stateFly:
                    // Apply air friction
                    Projectile.velocity.X = Projectile.velocity.X * airFriction;

                    // Rotation increased by velocity.X 
                    Projectile.rotation += Projectile.velocity.X * 0.04f;
                    break;
                case stateGetStickAxis:
                    // When sticking to a wall, we'll stop moving along one axis, but keep moving slightly along the other
                    // We can use this information to check from which direction we hit a wall
                    // That will then decide what direction the sprinkler will face towards
                    prevXdiff = Projectile.oldPosition.X - prevX;
                    prevYdiff = Projectile.oldPosition.Y - prevY;
                    var xDiff = Projectile.position.X - prevX;
                    var yDiff = Projectile.position.Y - prevY;

                    //debugMessage(debug, $"xDiff: {xDiff}, yDiff: {yDiff}, prevXdiff: {prevXdiff}, prevYdiff: {prevYdiff}");

                    bool foundAxis = false;
                    if (xFacing != 0)
                    {
                        foundAxis = true;
                        isStickingVertically = true;
                    }
                    else if (yFacing != 0)
                    {
                        foundAxis = true;
                        isStickingVertically = false;
                    }

                    if (foundAxis)
                    {
                        debugMessage(debug, $"isStickingVertically: {isStickingVertically}");
                        AdvanceState();
                    }

                    // Fallback if we can't find a sticking axis
                    Timer--;
                    if (Timer <= 0)
                    {
                        fallback = true;
                        isStickingVertically = false;
                        stickingDirection = new Vector2(0, 1);
                        AdvanceState();
                        debugMessage(debug, $"Couldn't find stick axis (is this a slope?) Stick dir: {stickingDirection}");
                    }
                    break;
                case stateGetStickDirection:
                    // Say we know the axis is vertical
                    // Check whether we approached it from the left or the right
                    if (!fallback)
                    {
                        if (isStickingVertically)
                        {
                            stickingDirection = new Vector2(Math.Sign(xFacing), 0);
                        }
                        else
                        {
                            stickingDirection = new Vector2(0, Math.Sign(yFacing));
                        }
                    }

                    debugMessage(debug, $"Sticking direction vector: {stickingDirection.X}, {stickingDirection.Y}");
                    var degrees = MathHelper.ToDegrees(stickingDirection.ToRotation()) - 90;
                    var finalRotation = MathHelper.ToRadians(degrees);
                    Projectile.rotation = finalRotation;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.position.X = prevX;
                    Projectile.position.Y = prevY;
                    Projectile.netUpdate = true;

                    StopAudio("Throwables/SplatBombThrow");
                    PlayAudio("Throwables/SprinklerDeployNew", volume: 0.3f, pitchVariance: 0.2f);
                    Timer = 30 * FrameSpeed();
                    AdvanceState();
                    break;
                case stateWait:
                    Timer--;
                    if (Timer <= 0)
                    {
                        AdvanceState();
                    }
                    break;
                case stateFire:
                    var baseDegrees = MathHelper.ToDegrees(stickingDirection.ToRotation());

                    if (Timer <= 0)
                    {
                        Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];

                        ShotsFired++;
                        shotsLeft--;
                        var cone = 150;
                        var a = (ShotsFired * 18) % 360;
                        var sinVal = Math.Sin(MathHelper.ToRadians(a));
                        Vector2 angleVector = CalculateShotAngle(cone, a, baseDegrees);

                        // Bullet spawn/timer
                        ShootSprinkler(angleVector);
                        if (sinVal != 0)
                        {
                            angleVector = CalculateShotAngle(cone, -a, baseDegrees);
                            ShootSprinkler(angleVector);
                        }

                        Timer = baseAttackTime * FrameSpeed();
                    }

                    Timer--;

                    if (shotsLeft <= 0)
                    {
                        AdvanceState();
                    }
                    break;
                case stateShrink:
                    drawScale *= 0.98f;
                    if (drawScale < 0.05f)
                    {
                        EmitBurstDust(dustMaxVelocity: 10, amount: 15, minScale: 1, maxScale: 2);
                        Projectile.Kill();
                    }
                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == stateFly)
            {
                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    xFacing = oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (xFacing == 0 && Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    yFacing = oldVelocity.Y;
                }

                // Set the previous position as we collide with a wall
                prevX = Projectile.position.X;
                prevY = Projectile.position.Y;
                hasCollided = true;

                Timer = 2 * FrameSpeed();
                AdvanceState();
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(inkColor: initialColor, rotation: Projectile.rotation, scale: drawScale, alphaMod: 1, considerWorldLight: false, additiveAmount: 1f);
            return false;
        }

        private void DespawnOtherSprinklers()
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.ModProjectile is SprinklerSentry
                    && projectile.identity != Projectile.identity
                    && projectile.owner == Projectile.owner)
                {
                    var p = projectile.ModProjectile as SprinklerSentry;

                    if (p.Projectile.timeLeft < Projectile.timeLeft)
                    {
                        p.state = stateShrink;
                    }
                }
            }
        }

        private Vector2 CalculateShotAngle(float cone, float offsetDegrees, float startingDegrees)
        {
            var sinVal = Math.Sin(MathHelper.ToRadians(offsetDegrees));
            var angle = (float)(sinVal * cone / 2) + startingDegrees + 180;
            var angleRadians = MathHelper.ToRadians(angle);
            return angleRadians.ToRotationVector2();
        }

        private void ShootSprinkler(Vector2 angleVector)
        {
            if (!IsThisClientTheProjectileOwner()) return;
            CreateChildProjectile(
                position: Projectile.Center - stickingDirection * 6f,
                velocity: angleVector * 9f,
                type: ModContent.ProjectileType<SprinklerProjectile>(),
                damage: Projectile.damage);
        }
    }
}
