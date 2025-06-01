using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class SquidAuraProjectile : BaseProjectile
    {
        protected override float DamageModifierAfterPierce => 1f;

        protected float drawAlpha;
        protected float drawScale;
        protected float drawRotation;

        protected const int stateSpawn = 0;
        protected const int stateFollowPlayer = 1;
        protected const int stateDisappear = 2;

        protected int hitboxSize;

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
            enablePierceDamagefalloff = false;
            wormDamageReduction = true;
            hitboxSize = 80;
            SetState(stateSpawn);

            // Visuals/audio
            drawRotation = MathHelper.ToRadians(Main.rand.Next(0, 359));
            drawScale = 1;
            drawAlpha = 0;
            UpdateCurrentColor(Owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer());

            SoundHelper.PlayAudio(SoundID.Splash, volume: 0.3f, pitch: 0f, pitchVariance: 0.3f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item21, volume: 0.6f, pitch: 0.5f, pitchVariance: 0.2f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundPaths.KrakenJump.ToSoundStyle(), volume: 0.1f, pitch: 0f, pitchVariance: 0.2f, position: Projectile.Center);
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
                case stateDisappear:
                    Projectile.friendly = false;
                    break;
            }
        }

        public override void AI()
        {
            var isSquid = Owner.GetModPlayer<SquidPlayer>().IsSquid();
            Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY + (isSquid ? 16 : 0));

            var rotateDirection = 1;
            var rotateSpeed = rotateDirection * -0.3f;
            drawRotation += rotateSpeed;
            drawScale = hitboxSize / 100f;

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
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            SetHitboxSize(hitboxSize, out hitbox);
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
                var rotation = drawRotation / 2 + i * 0.8f;

                DrawProjectile(
                    inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, CurrentColor, i * 0.2f),
                    rotation: rotation,
                    scale: drawScale * 0.8f,
                    alphaMod: drawAlpha * 0.4f,
                    considerWorldLight: false,
                    additiveAmount: 1f,
                    originOffset: new Vector2(-40, 0));
            }

            for (int i = 0; i < 8; i++)
            {
                var rotation = drawRotation / -3 + i * 0.8f;

                DrawProjectile(
                    inkColor: ColorHelper.LerpBetweenColorsPerfect(CurrentColor, CurrentColor, i * 0.2f),
                    rotation: rotation,
                    scale: drawScale * 0.8f,
                    alphaMod: drawAlpha * 0.1f,
                    considerWorldLight: false,
                    additiveAmount: 1f,
                    originOffset: new Vector2(10, 0),
                    spriteOverride: TexturePaths.LargeCrescentSlash.ToTexture2D());
            }

            return false;
        }
    }
}
