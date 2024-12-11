using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class VortexDualieRollProjectile : DualieRollProjectile
    {
        protected override bool DisplayDefaultDusts => false;
        private Player owner;

        protected override void AfterSpawn()
        {
            base.AfterSpawn();

            var xDir = InputHelper.GetInputX();
            owner = GetOwner();

            int goalDistance = 300;
            int finalDistance = 0;
            bool canTeleport = false;
            int loopMax = 50;
            int loopAmount = 0;

            Vector2 teleportPosition = owner.position;
            for (int i = 0; i < loopMax; i++)
            {
                finalDistance = (goalDistance / loopMax) * i;

                Vector2 checkPosition = new Vector2(owner.position.X + finalDistance * xDir, owner.position.Y);
                if (HasLineOfSight(checkPosition) && !IsSolidAt(checkPosition))
                {
                    canTeleport = true;
                    teleportPosition = checkPosition;
                    loopAmount = i - 1;
                }
                else
                {
                    break;
                }
            }

            if (canTeleport)
            {
                BurstStream();
                float velocityMod = (float)loopAmount / (float)loopMax;

                for (int i = 0; i < finalDistance; i++)
                {
                    var scale = Main.rand.NextFloat(0.8f, 1.2f);
                    if (i < 12)
                    {
                        Rectangle rect = new Rectangle(
                            (int)owner.position.X + i,
                            (int)owner.position.Y + owner.height / 4,
                            owner.width,
                            owner.height / 2);
                        DustStream(xDir, rect, velocityMod);

                        rect = new Rectangle(
                            (int)owner.position.X + (finalDistance - i) * xDir,
                            (int)owner.position.Y + owner.height / 4,
                            owner.width,
                            owner.height / 2);
                        DustStream(-xDir, rect, velocityMod);
                    }
                }

                owner.Teleport(teleportPosition, TeleportationStyleID.DebugTeleport);
                BurstStream();
            }
            else
            {
                owner.velocity.X *= 2;
            }
        }

        protected override void PlayRollSound()
        {
            PlayAudio(SoundID.Item25, 0.3f, pitchVariance: 0.1f, maxInstances: 5, pitch: 0f);
            PlayAudio(SoundID.Item67, 0.3f, pitchVariance: 0.1f, maxInstances: 5, pitch: 0.8f);
            PlayAudio(SoundID.Item60, 0.5f, pitchVariance: 0.1f, maxInstances: 5, pitch: 0f);
        }

        private void CreateDust(Vector2 velocity, Vector2 position)
        {
            Dust d = Dust.NewDustPerfect(
                Position: position,
                Type: 226,
                Velocity: velocity,
                Alpha: 0,
                newColor: Color.Blue,
                Scale: Main.rand.NextFloat(0.8f, 1.2f));
            d.noGravity = true;
        }

        private void DustStream(int xDirection, Rectangle rectangle, float velocityMod = 1f)
        {
            Vector2 velocity = new Vector2(Main.rand.NextFloat(3, 30) * xDirection, 0) * velocityMod;
            Vector2 position = Main.rand.NextVector2FromRectangle(rectangle);
            CreateDust(velocity, position);
        }

        private void BurstStream()
        {
            for (int i = 0; i < 5; i++)
            {
                CreateDust(new Vector2(Main.rand.NextFloat(-1, 1), i * 2), owner.Center);
                CreateDust(new Vector2(Main.rand.NextFloat(-1, 1), -i * 2), owner.Center);
            }
        }

        private bool IsSolidAt(Vector2 positionToCheck)
        {
            return Framing.GetTileSafely(positionToCheck).HasTile && Collision.SolidCollision(positionToCheck, owner.width, owner.height);
        }

        private bool HasLineOfSight(Vector2 positionToCheck)
        {
            return Collision.CanHitLine(owner.position, owner.width * 2, owner.height, positionToCheck, owner.width * 2, owner.height);
        }
    }
}
