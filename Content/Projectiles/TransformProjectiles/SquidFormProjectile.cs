using Terraria;
using Microsoft.Xna.Framework;
using System;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.Dusts;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.TransformProjectiles
{
    internal class SquidFormProjectile : BaseProjectile
    {
        protected override bool FallThroughPlatforms => false;
        private SquidPlayer squidPlayer = null;

        private float _maxGroundedSpeed = 3f;
        private float _groundFriction = 0.9f;
        private float _airFriction = 0.99f;
        private float _terminalVelocity = 5f;
        private float _jumpHeight = 4f;

        private const int _stateDefault = 0;
        private const int _stateDespawn = 1;
        private const int _stateRefill = 2;

        private float _drawScale = 1f;
        private bool _drawFlip = false;
        private float _splashCooldown = 0;
        private float _splashCooldownMax = 15f;

        public void SetInitialParams(Vector2 velocity)
        {
            Projectile.velocity = new Vector2(velocity.X * 0.75f, velocity.Y * 0.5f);
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = GetOwner().width;
            Projectile.height = GetOwner().height;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }

        protected override void AfterSpawn()
        {
            if (GetOwner().ownedProjectileCounts[Projectile.type] > 0)
            {
                DebugHelper.Ping();
            }

            dissolvable = false;
            squidPlayer = GetOwner().GetModPlayer<SquidPlayer>();
            _drawFlip = GetOwner().direction == -1;

            SetState(_stateDefault);
            TransformDust();
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
        }

        public override void AI()
        {
            if (_splashCooldown > 0) _splashCooldown--;

            Projectile.timeLeft = 2;

            switch (state)
            {
                case _stateDefault:
                    StateDefault();
                    break;
                case _stateDespawn:
                    StateDespawn();
                    break;
            }
        }

        private void Animate()
        {
            Projectile.frame = PlayerHelper.IsPlayerGrounded(GetOwner()) && !PlayerHelper.IsPlayerOntopOfPlatform(GetOwner()) ? 0 : 1;

            if (!PlayerHelper.IsPlayerGrounded(GetOwner()) || Projectile.oldVelocity.Y != 0)
            {
                _drawFlip = false;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                if (!PlayerHelper.IsPlayerOntopOfPlatform(GetOwner()))
                {
                    Projectile.rotation = 0;
                }

                if (Projectile.velocity.X != 0)
                {
                    _drawScale = 1f + (float)Math.Sin(timeSpentAlive / 10f) / 10f;
                    _drawFlip = Projectile.velocity.X < 0;
                }
            }

            Projectile.velocity = GetOwner().velocity;
            Projectile.Center = GetOwner().Center + new Vector2(0, GetOwner().gfxOffY + GetOwner().height / 2 - 3);
        }

        private void StateDefault()
        {
            if (timeSpentAlive < 5)
            {
                _drawScale += 0.2f;
            }
            else if (_drawScale > 1)
            {
                _drawScale -= 0.1f;
            }

            Animate();
        }

        private void StateDespawn()
        {
            _drawScale += 0.08f;

            Animate();

            if (timeSpentInState >= 12)
            {
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            var wepMp = GetOwnerModPlayer<WeaponPlayer>();
            if (wepMp.CustomWeaponCooldown < 6)
            {
                wepMp.CustomWeaponCooldown = 6;
            }

            TransformDust();
            squidPlayer.SetState(SquidPlayer.stateHuman);
        }


        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var color = GetOwnerModPlayer<ColorChipPlayer>().GetColorFromChips();
            DrawProjectile(
                ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.9f),
                Projectile.rotation,
                scale: _drawScale,
                alphaMod: GetOwner().immune ? 1 - (float)GetOwner().immuneAlpha / 255f : 1,
                flipSpriteSettings: _drawFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                frameOverride: Projectile.frame + 2);

            DrawProjectile(
                ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.2f),
                Projectile.rotation,
                scale: _drawScale,
                alphaMod: GetOwner().immune ? 1 - (float)GetOwner().immuneAlpha / 255f : 1,
                flipSpriteSettings: _drawFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            return false;
        }

        // Methods called from outside (by SquidPlayer for example)
        public void StartDespawn()
        {
            if (state != _stateDespawn)
            {
                SetState(_stateDespawn);
            }
        }

        public void TransformDust()
        {
            Color dustColor = GetOwner().GetModPlayer<ColorChipPlayer>().GetColorFromChips();

            bool isSmall = GetOwnerModPlayer<SquidPlayer>().IsSquid() && PlayerHelper.IsPlayerGrounded(GetOwner());
            var position = isSmall ? GetOwner().Center + new Vector2(0, GetOwner().height / 2) : GetOwner().Center;

            if (isSmall)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(
                        Position: position,
                        Type: ModContent.DustType<ChargerBulletDust>(),
                        Velocity: Main.rand.NextVector2Circular(5, 2),
                        newColor: dustColor,
                        Scale: Main.rand.NextFloat(1f, 2f));
                }
            }
            else
            {
                for (int i = 0; i < 25; i++)
                {
                    Dust.NewDustPerfect(
                        Position: position,
                        Type: ModContent.DustType<ChargerBulletDust>(),
                        Velocity: Main.rand.NextVector2Circular(5, 5),
                        newColor: dustColor,
                        Scale: Main.rand.NextFloat(1f, 2f));
                }
            }

        }

        public void LandingEffect()
        {
            if (_splashCooldown > 0) return;
            _splashCooldown = _splashCooldownMax;

            Color dustColor = GetOwner().GetModPlayer<ColorChipPlayer>().GetColorFromChips();

            for (int i = 0; i < 10; i++)
            {
                var velocity = i < 5 ? new Vector2(4, 0) : new Vector2(-4, 0);

                Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: ModContent.DustType<SplatterDropletDust>(),
                    Velocity: velocity + Main.rand.NextVector2Circular(2, 2),
                    newColor: dustColor,
                    Scale: Main.rand.NextFloat(1f, 2f));
            }

            SoundHelper.PlayAudio($"SwimForm/slime0{Main.rand.Next(5)}", 0.4f, 0.2f, 10, 0, GetOwner().Center);

            List<SoundStyle> sounds = new()
            {
                SoundID.NPCHit8,
                SoundID.NPCHit13,
                SoundID.NPCHit17,
                SoundID.NPCHit19,
                SoundID.NPCHit29
            };

            SoundHelper.PlayAudio(Main.rand.NextFromCollection(sounds), volume: 0.1f, pitchVariance: 0.5f, maxInstances: 50, pitch: 1f, position: GetOwner().Center);
        }

        public void ResetDrawScale()
        {
            _drawScale = 1f;
        }
    }
}
