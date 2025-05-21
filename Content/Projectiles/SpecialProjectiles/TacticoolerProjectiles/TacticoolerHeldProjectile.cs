using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.TacticoolerProjectiles
{
    internal class TacticoolerHeldProjectile : BaseProjectile
    {
        // State machine
        private const int _stateFlip = 0;
        private const int _stateReady = 1;
        private const int _stateThrow = 2;
        private const int _stateDespawn = 3;

        // Visuals
        protected float drawScale = 1f;
        protected readonly float drawRotation = 0f;
        protected int drawDirection = 0;
        protected Vector2 drawPosition = Vector2.Zero;
        protected Vector2 holdOffset = Vector2.Zero;
        protected Vector2 holdOffsetDefault = Vector2.Zero;
        protected float rotationOffset = 0;
        protected Vector2 mouseDirection = Vector2.Zero;
        private bool _flipDone = false;

        // Mechanics
        private int _startDelay = 0;

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            _startDelay = 36;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            SetState(_stateFlip);
        }

        public override void AI()
        {
            if (Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            HideWeapon();

            Projectile.timeLeft++;
            mouseDirection = Owner.DirectionTo(Main.MouseWorld);

            Owner.channel = true;
            Owner.heldProj = Projectile.whoAmI;

            // Rotate and position the zooka + player arm
            Projectile.Center = Owner.Center.Round() + new Vector2(0, Owner.gfxOffY);
            var heightBoost = Owner.mount.Active ? -Owner.mount.HeightBoost * 0.6f : 0;

            drawPosition = Owner.Center.Round() + new Vector2(0, Owner.gfxOffY - 14) + new Vector2(0, heightBoost);
            Owner.direction = Owner.position.X > Main.MouseWorld.X ? -1 : 1;
            drawDirection = Owner.direction;
            holdOffset = Vector2.Lerp(holdOffset, holdOffsetDefault, 0.2f);

            var armRotateDeg = -200f;
            if (drawDirection == -1) armRotateDeg = 200f;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(armRotateDeg));

            switch (state)
            {
                case _stateFlip:
                    StateFlip();
                    break;
                case _stateReady:
                    StateReady();
                    break;
                case _stateThrow:
                    StateThrow();
                    break;
                case _stateDespawn:
                    StateDespawn();
                    break;
            }
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            var weaponPlayer = Owner.GetModPlayer<WeaponPlayer>();
            switch (state)
            {
                case _stateFlip:
                    weaponPlayer.allowSubWeaponUsage = false;
                    drawScale = 0f;
                    break;

                case _stateReady:
                    drawScale = 1f;
                    break;

                case _stateThrow:
                    if (IsThisClientTheProjectileOwner())
                    {
                        CreateChildProjectile<TacticoolerStand>(Owner.Center.Round() + new Vector2(0, Owner.gfxOffY - 14), Owner.DirectionTo(Main.MouseWorld) * 15, 0, true);
                    }
                    break;

                case _stateDespawn:
                    drawScale = 1f;
                    PlayAudio(SoundPaths.BombRushActivate.ToSoundStyle(), volume: 0.3f, position: Owner.Center);
                    break;
            }
        }

        private void HideWeapon()
        {
            Owner.itemLocation = new Vector2(-1000, -1000);
            Owner.itemAnimation = 2;
            Owner.itemTime = 2;
        }

        private void StateFlip()
        {
            // Perform the flip
            Projectile.rotation = Owner.fullRotation;
            if (timeSpentInState > 15)
            {
                drawScale = MathHelper.Lerp(drawScale, 1f, 0.2f);
            }

            if (!Owner.mount.Active && !_flipDone)
            {
                if (timeSpentInState < 8)
                {
                    Owner.fullRotation = MathHelper.Lerp(Owner.fullRotation, Owner.direction, 0.1f);
                    Owner.fullRotationOrigin = new Vector2(10f, 20f);
                }
                else if (timeSpentInState > 12)
                {
                    if (Math.Abs(Owner.fullRotation) < 6)
                    {
                        Owner.fullRotation += -Owner.direction * 0.5f;
                        Owner.fullRotationOrigin = new Vector2(10f, 20f);
                    }
                    else
                    {
                        _flipDone = true;
                        Owner.fullRotation = 0;
                    }
                }
            }

            // Flip sound
            if (timeSpentAlive == 12)
            {
                PlayAudio(SoundID.Item18, volume: 2f, position: Owner.Center, pitch: 0.5f);
            }

            if (timeSpentAlive == 15)
            {
                PlayAudio(SoundPaths.BombRushActivate.ToSoundStyle(), volume: 0.5f, position: Owner.Center);
            }

            if (timeSpentInState > _startDelay)
            {
                AdvanceState();
            }
        }

        private void StateReady()
        {
            if (!Owner.GetModPlayer<SpecialPlayer>().SpecialActivated)
            {
                SetState(_stateDespawn);
                return;
            }

            if (InputHelper.GetInputMouseLeftHold())
            {
                SetState(_stateThrow);
                return;
            }
        }

        private void StateThrow()
        {
            if (timeSpentInState > 30)
            {
                Projectile.Kill();

                Owner.itemAnimation = 0;
                Owner.itemTime = 0;
            }
        }

        private void StateDespawn()
        {
            drawScale = MathHelper.Lerp(drawScale, 0f, 0.2f);

            if (timeSpentInState > 15)
            {
                Owner.GetModPlayer<SpecialPlayer>().UnreadySpecial();

                Owner.itemAnimation = 0;
                Owner.itemTime = 0;

                Owner.heldProj = -1;
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            Owner.GetModPlayer<WeaponPlayer>().allowSubWeaponUsage = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (state == _stateThrow)
            {
                return false;
            }

            Vector2 position = drawPosition - Main.screenPosition + holdOffset;
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Rectangle sourceRectangle = texture.Frame(horizontalFrames: 1);
            Vector2 origin = sourceRectangle.Size() / 2f + new Vector2(-8 * drawDirection, 8);

            // The light value in the world
            var lightInWorld = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
            var finalColor = new Color(lightInWorld.R, lightInWorld.G, lightInWorld.G);

            SpriteBatch spriteBatch = Main.spriteBatch;

            Main.EntitySpriteDraw(
                texture,
                position,
                sourceRectangle,
                finalColor,
                Projectile.rotation + MathHelper.ToRadians(rotationOffset),
                origin,
                drawScale,
                drawDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
