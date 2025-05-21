using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.EnumsAndConstants;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class UrchinEmblemProjectile : BaseProjectile
    {
        protected override float DamageModifierAfterPierce => 1f;

        private float _drawAlpha;
        private float _drawScale;
        private float _drawRotation;
        private float _brightness;

        private const int _stateSpawn = 0;
        private const int _stateFollowPlayer = 1;
        private const int _stateDisappear = 2;

        private int _hitboxSize;

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        protected override void AfterSpawn()
        {
            Projectile.damage = UrchinEmblem.AttackDamage;
            Projectile.ArmorPenetration = 100;
            Projectile.knockBack = 8;

            enablePierceDamagefalloff = false;
            wormDamageReduction = true;
            _hitboxSize = 80;
            SetState(_stateSpawn);

            // Visuals/audio
            _drawRotation = MathHelper.ToRadians(Main.rand.Next(0, 359));
            _drawScale = 1;
            _drawAlpha = 0;
            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer());

            SoundHelper.PlayAudio(SoundID.Splash, volume: 0.3f, pitch: 0f, pitchVariance: 0.3f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item21, volume: 0.6f, pitch: 0.5f, pitchVariance: 0.2f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundPaths.KrakenJump.ToSoundStyle(), volume: 0.1f, pitch: 0f, pitchVariance: 0.2f, position: Projectile.Center);

            if (Owner.immuneTime == 0)
            {
                Owner.immune = true;
                Owner.immuneTime = 18;
                Owner.immuneNoBlink = true;
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            switch (state)
            {
                case _stateDisappear:
                    Projectile.friendly = false;
                    break;
            }
        }

        public override void AI()
        {
            Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY + 16);

            var rotateDirection = 1;
            var rotateSpeed = rotateDirection * -0.3f;
            _drawRotation += rotateSpeed;
            _drawScale = _hitboxSize / 100f;

            if (Owner.immune)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center + Main.rand.NextVector2Circular(40, 40),
                    Type: DustID.PortalBolt,
                    Velocity: new Vector2(0, -4),
                    newColor: CurrentColor,
                    Scale: Main.rand.NextFloat(1f, 1.4f)
                );

                d.color = CurrentColor;
                d.noGravity = true;
                d.noLight = true;
            }

            switch (state)
            {
                case _stateSpawn:

                    if (_drawAlpha < 1f)
                    {
                        _drawAlpha += 0.1f;
                    }

                    if (timeSpentInState > 6)
                    {
                        _hitboxSize += 10;
                    }
                    else
                    {
                        _hitboxSize -= 4;
                    }

                    if (timeSpentInState > 12)
                    {
                        AdvanceState();
                    }

                    break;

                case _stateFollowPlayer:
                    _hitboxSize += 8;

                    if (timeSpentInState > 4)
                    {
                        AdvanceState();
                    }
                    break;

                case _stateDisappear:
                    _hitboxSize += 3;

                    if (_drawAlpha > 0f)
                    {
                        _drawAlpha -= 0.1f;
                    }

                    if (_drawAlpha <= 0f)
                    {
                        Projectile.Kill();
                        return;
                    }

                    break;
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            SetHitboxSize(_hitboxSize, out hitbox);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DisableCrit();
            modifiers.HitDirectionOverride = Math.Sign(target.position.X - GetOwner().position.X);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: target.Center,
                    Type: DustID.WhiteTorch,
                    Velocity: target.DirectionTo(Owner.Center) * -12 + Main.rand.NextVector2Circular(4, 4),
                    newColor: CurrentColor,
                    Scale: Main.rand.NextFloat(1f, 3f)
                );

                d.color = CurrentColor;
                d.noGravity = true;
                d.noLight = true;
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 8; i++)
            {
                var rotation = _drawRotation / 2 + i * 0.8f;

                DrawProjectile(
                    inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, CurrentColor, i * 0.2f),
                    rotation: rotation,
                    scale: _drawScale * 0.8f,
                    alphaMod: _drawAlpha * 0.4f,
                    considerWorldLight: false,
                    additiveAmount: 1f,
                    originOffset: new Vector2(-40, 0));
            }

            for (int i = 0; i < 8; i++)
            {
                var rotation = _drawRotation / -3 + i * 0.8f;

                DrawProjectile(
                    inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, CurrentColor, i * 0.2f),
                    rotation: rotation,
                    scale: _drawScale * 0.8f,
                    alphaMod: _drawAlpha * 0.1f,
                    considerWorldLight: false,
                    additiveAmount: 1f,
                    originOffset: new Vector2(10, 0),
                    spriteOverride: TexturePaths.LargeCrescentSlash.ToTexture2D());
            }

            //for (int i = 0; i < 8; i++)
            //{
            //    var rotation = i * 0.8f;

            //    DrawProjectile(
            //        CurrentColor,
            //        rotation: rotation,
            //        scale: 1,
            //        alphaMod: _drawAlpha * 0.2f,
            //        additiveAmount: 1f,
            //        considerWorldLight: false,
            //        positionOffset: new Vector2(0, 0),
            //        flipSpriteSettings: SpriteEffects.FlipHorizontally,
            //        spriteOverride: TexturePaths.GolemSlashSegment.ToTexture2D(),
            //        positionOverride: Owner.Center + WoomyMathHelper.AddRotationToVector2(new Vector2(100, 0), (45 * i) + _drawRotation * 20)
            //        );
            //}

            return false;
        }
    }
}
