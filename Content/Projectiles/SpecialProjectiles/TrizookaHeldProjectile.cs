using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class TrizookaHeldProjectile : BaseProjectile
    {
        // State machine
        private const int _stateFlip = 0;
        private const int _stateReady = 1;
        private const int _stateFire = 2;
        private const int _stateDespawn = 3;

        // Visuals
        private float _drawScale = 1f;
        private readonly float _drawRotation = 0f;
        private int _drawDirection = 0;
        private Vector2 _drawPosition = Vector2.Zero;
        private Vector2 _holdOffset = Vector2.Zero;
        private Vector2 _holdOffsetDefault = Vector2.Zero;
        private float _rotationOffset = 0;
        private bool _flipDone = false;

        // Mechanics
        private int _shotsRemaining = 0;
        private int _shotDelay = 0;
        private int _startDelay = 0;

        // Misc
        private Vector2 _mouseDirection = Vector2.Zero;

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

            var weaponData = (TrizookaSpecial)WeaponInstance;
            _shotDelay = 48;
            _startDelay = 36;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();
            SetState(_stateFlip);

            _shotsRemaining = 3;
            _holdOffsetDefault = new Vector2(Owner.direction * -2, -4);
        }

        public override void AI()
        {
            Projectile.timeLeft++;
            _mouseDirection = Owner.DirectionTo(Main.MouseWorld);

            Owner.channel = true;
            Owner.heldProj = Projectile.whoAmI;

            // Rotate and position the zooka + player arm
            Projectile.Center = Owner.Center.Round() + new Vector2(0, Owner.gfxOffY);
            var heightBoost = Owner.mount.Active ? -Owner.mount.HeightBoost * 0.6f : 0;

            _drawPosition = Owner.Center.Round() + new Vector2(0, Owner.gfxOffY) + new Vector2(0, heightBoost);
            Owner.direction = Owner.position.X > Main.MouseWorld.X ? -1 : 1;
            _drawDirection = Owner.direction;
            _holdOffset = Vector2.Lerp(_holdOffset, _holdOffsetDefault, 0.2f);

            var armRotateDeg = 90f;
            if (_drawDirection == -1) armRotateDeg = -90f;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(armRotateDeg));

            // Hide the player's regular item, and prevent other item usage by setting the itemAnimation/itemTime
            Owner.itemLocation = new Vector2(-1000, -1000);
            Owner.itemAnimation = Owner.itemAnimationMax;
            Owner.itemTime = Owner.itemTimeMax;

            switch (state)
            {
                case _stateFlip:
                    StateFlip();
                    break;
                case _stateReady:
                    StateReady();
                    break;
                case _stateFire:
                    StateFire();
                    break;
                case _stateDespawn:
                    StateDespawn();
                    break;
            }
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case _stateFlip:
                    _drawScale = 0f;
                    break;

                case _stateReady:
                    Owner.fullRotation = 0;
                    _drawScale = 1f;
                    break;

                case _stateFire:
                    // --- Mechanical
                    _shotsRemaining--;

                    // Shoot
                    var shotPosition = Owner.Center;
                    if (Collision.CanHitLine(Owner.Center, 0, 0, Main.MouseWorld, 0, 0))
                    {
                        shotPosition = Owner.Center + _mouseDirection * 40;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        var p = CreateChildProjectile<TrizookaProjectile>(
                            position: shotPosition,
                            velocity: _mouseDirection * 10,
                            damage: Projectile.damage);

                        p.colorOverride = CurrentColor;
                        p.shotNumber = i;
                    }

                    // Recoil
                    Owner.velocity -= _mouseDirection * 3;

                    // --- Audio/visual
                    // Recoil
                    _drawScale = 1.2f;
                    _holdOffset = _holdOffsetDefault + -_mouseDirection * 20;

                    // Sound
                    var volumeMod = 1.4f;
                    PlayAudio(SoundID.Item66, volume: 0.5f * volumeMod, position: Owner.Center);
                    PlayAudio(SoundID.Item80, volume: 0.2f * volumeMod, position: Owner.Center);
                    PlayAudio(SoundPaths.TrizookaLaunch.ToSoundStyle(), volume: 0.2f * volumeMod, position: Owner.Center);
                    PlayAudio(SoundPaths.TrizookaLaunchAlly.ToSoundStyle(), volume: 0.5f * volumeMod, position: Owner.Center);

                    // Muzzle flare
                    for (int i = 1; i < 20; i++)
                    {
                        var d = Dust.NewDustDirect(
                            shotPosition,
                            0,
                            0,
                            DustID.RainbowTorch,
                            newColor: ColorHelper.ColorWithAlpha255(CurrentColor),
                            Scale: Main.rand.NextFloat(0.5f, 2f));

                        d.velocity = WoomyMathHelper.AddRotationToVector2(_mouseDirection * 4, Main.rand.NextFloat(-30, 30)) * Main.rand.NextFloat(0.25f, 0.5f) * i;
                        d.noGravity = true;
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        var g = Gore.NewGoreDirect(Terraria.Entity.GetSource_None(), Owner.Center - _mouseDirection * 60 + new Vector2(-20, -15), Vector2.Zero, GoreID.Smoke1, Main.rand.NextFloat(0.8f, 1.2f));
                        g.velocity = WoomyMathHelper.AddRotationToVector2(
                            -_mouseDirection * Main.rand.NextFloat(1, 4),
                            -30,
                            30);
                        g.alpha = 128;
                    }

                    // Discard shell
                    var shellVelocity = WoomyMathHelper.AddRotationToVector2(-_mouseDirection * 8, Main.rand.Next(-30, 30));
                    CreateChildProjectile<TrizookaShell>(Owner.Center, shellVelocity, 0, true);

                    // Camera shake
                    GameFeelHelper.ShakeScreenNearPlayer(Owner, true, strength: 4, speed: 2, duration: 15);
                    break;

                case _stateDespawn:
                    _drawScale = 1f;
                    _holdOffset = _holdOffsetDefault;
                    PlayAudio(SoundPaths.TrizookaDeactivate.ToSoundStyle(), volume: 0.3f, position: Owner.Center);
                    break;
            }
        }

        private void StateFlip()
        {
            Projectile.rotation = Owner.fullRotation;
            if (timeSpentInState > 15)
            {
                _drawScale = MathHelper.Lerp(_drawScale, 1f, 0.2f);
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

            if (timeSpentAlive == 12)
            {
                PlayAudio(SoundID.Item18, volume: 2f, position: Owner.Center, pitch: 0.5f);
            }

            if (timeSpentAlive == 15)
            {
                PlayAudio(SoundPaths.TrizookaActivate.ToSoundStyle(), volume: 0.5f, position: Owner.Center);
            }

            if (timeSpentInState > _startDelay)
            {
                AdvanceState();
            }
        }

        private void StateReady()
        {
            MakeProjectileFaceCursor(Owner);

            if (!Owner.GetModPlayer<SpecialPlayer>().SpecialActivated)
            {
                SetState(_stateDespawn);
            }

            if (InputHelper.GetInputMouseLeftHold())
            {
                SetState(_stateFire);
            }
        }

        private void StateFire()
        {
            _drawScale = MathHelper.Lerp(_drawScale, 1f, 0.3f);

            if (timeSpentInState < _shotDelay / 4)
            {
                _rotationOffset = MathHelper.Lerp(_rotationOffset, -Owner.direction * 20, 0.2f);
            }
            else
            {
                _rotationOffset = MathHelper.Lerp(_rotationOffset, 0, 0.1f);
            }

            if (timeSpentInState > _shotDelay)
            {
                _drawScale = 1f;
                if (_shotsRemaining > 0)
                {
                    SetState(_stateReady);
                }
                else
                {
                    SetState(_stateDespawn);
                }
            }

            // Extend special buff
        }

        private void StateDespawn()
        {
            _drawScale = MathHelper.Lerp(_drawScale, 0f, 0.2f);

            if (timeSpentInState > 15)
            {
                Owner.GetModPlayer<SpecialPlayer>().UnreadySpecial();

                Owner.itemAnimation = 0;
                Owner.itemTime = 0;

                Owner.heldProj = -1;
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = _drawPosition - Main.screenPosition + _holdOffset;
            Texture2D texture = TexturePaths.ZookaStages.ToTexture2D();

            Rectangle sourceRectangle = texture.Frame(horizontalFrames: 4, frameX: 3 - _shotsRemaining);
            Vector2 origin = sourceRectangle.Size() / 2f + new Vector2(-8 * _drawDirection, 8);

            // The light value in the world
            var lightInWorld = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
            var finalColor = new Color(lightInWorld.R, lightInWorld.G, lightInWorld.G);

            SpriteBatch spriteBatch = Main.spriteBatch;

            Main.EntitySpriteDraw(
                texture,
                position,
                sourceRectangle,
                finalColor,
                Projectile.rotation + MathHelper.ToRadians(_rotationOffset),
                origin,
                _drawScale,
                _drawDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private void MakeProjectileFaceCursor(Player owner)
        {
            PlayerItemAnimationFaceCursor(owner);

            // Change player direction depending on what direction the charger is held when charging
            float mouseDirRadians;
            mouseDirRadians = _mouseDirection.ToRotation();
            var mouseDirDegrees = MathHelper.ToDegrees(mouseDirRadians);

            if (mouseDirDegrees >= -90 && mouseDirDegrees <= 90)
            {
                owner.direction = 1;
                Projectile.rotation = mouseDirRadians;
            }
            else
            {
                owner.direction = -1;
                var sign = Math.Sign(mouseDirDegrees);
                if (sign > 0)
                {
                    Projectile.rotation = MathHelper.ToRadians((mouseDirDegrees - 180));
                }
                else
                {
                    Projectile.rotation = MathHelper.ToRadians(mouseDirDegrees + 180);
                }
            }
        }
    }
}
