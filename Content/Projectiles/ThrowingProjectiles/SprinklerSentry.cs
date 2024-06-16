using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class SprinklerSentry : BaseBombProjectile
    {
        private float previousVelocityX;
        private float previousVelocityY;
        private float prevX;
        private float prevY;
        private float prevXdiff;
        private float prevYdiff;
        private bool sticking = false;
        private bool isStickingHorizontally;
        private Vector2 stickingDirection = new Vector2(0, 0);
        private bool hasCollided = false;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 12;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60 * FrameSpeed();
            Projectile.tileCollide = true;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -12;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            terminalVelocity = terminalVelocity / FrameSpeed();
            airFriction = 0.999f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, glowColor.R * brightness, glowColor.G * brightness, glowColor.B * brightness);

            if (!hasCollided)
            {
                // Apply air friction
                Projectile.velocity.X = Projectile.velocity.X * airFriction;

                // Rotation increased by velocity.X 
                Projectile.rotation += Projectile.velocity.X * 0.04f;

                // Apply gravity
                Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + (0.05f / FrameSpeed()), -terminalVelocity, terminalVelocity);
            } else
            {
                if (!sticking)
                {
                    // When sticking to a wall, we'll stop moving along one axis, but keep moving slightly along the other
                    // We can use this information to check from which direction we hit a wall
                    // That will then decide what direction the sprinkler will face towards
                    prevXdiff = Projectile.oldPosition.X - prevX;
                    prevYdiff = Projectile.oldPosition.Y - prevY;
                    var xDiff = Projectile.position.X - prevX;
                    var yDiff = Projectile.position.Y - prevY;

                    if (xDiff == prevXdiff)
                    {
                        isStickingHorizontally = true;
                        sticking = true;
                    }
                    else if (yDiff == prevYdiff)
                    {
                        isStickingHorizontally = false;
                        sticking = true;
                    }

                    Main.NewText($"isStickingHorizontally: {isStickingHorizontally}");

                } else
                {
                    if (stickingDirection.Equals(Vector2.Zero))
                    {
                        if (isStickingHorizontally)
                        {
                            if (prevXdiff < 0)
                            {
                                stickingDirection = new Vector2(-1, 0);
                            }
                            else
                            {
                                stickingDirection = new Vector2(1, 0);
                            }
                        }
                        else
                        {
                            if (prevYdiff < 0)
                            {
                                stickingDirection = new Vector2(0, -1);
                            }
                            else
                            {
                                stickingDirection = new Vector2(0, 1);
                            }
                        }

                        Main.NewText($"Sticking direction vector: {stickingDirection.X}, {stickingDirection.Y}");
                        Projectile.velocity = Vector2.Zero;
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasCollided)
            {
                // Set the previous position as we collide with a wall
                prevX = Projectile.position.X;
                prevY = Projectile.position.Y;
                hasCollided = true;
            }

            return false;
        }
    }
}
