using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.TransformProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class SquidPlayer : ModPlayer
    {
        public int state = 0;
        public const int stateHuman = 0;
        public const int stateSquid = 1;

        private SquidFormProjectile? _squidFormProjectile = null;
        private float _oldOpacityForAnimation = 1f;
        private float _oldWingTime = 0f;
        private bool _landed = false;

        private float _yCameraOffset = 0f;
        private float _yCameraOffsetGoal = 0f;
        private float _squidJumpTime = 0f;
        private float _squidJumpTimeMax = 0f;

        public void SetState(int state)
        {
            this.state = state;

            switch (this.state)
            {
                case stateHuman:
                    _squidFormProjectile = null;
                    Player.wingTime = _oldWingTime;

                    Player.opacityForAnimation = 1;
                    SoundHelper.PlayAudio(SoundPaths.SwimFormExit.ToSoundStyle(), 0.2f, 0.2f, 10, 0, Player.Center);
                    _yCameraOffsetGoal = 0;
                    break;

                case stateSquid:
                    Player.mount.Dismount(Player);

                    _squidJumpTimeMax = 15f;
                    _squidJumpTime = PlayerHelper.IsPlayerGrounded(Player) ? _squidJumpTimeMax : 0;

                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<SquidFormProjectile>()] == 0)
                    {
                        _squidFormProjectile = (SquidFormProjectile)ProjectileHelper.CreateProjectile(Player, ModContent.ProjectileType<SquidFormProjectile>());
                        _squidFormProjectile.Projectile.velocity.X = Player.direction;
                        _squidFormProjectile.SetInitialParams(Player.velocity);
                    }

                    _oldWingTime = Player.wingTime;

                    Player.opacityForAnimation = 0;
                    SoundHelper.PlayAudio(SoundPaths.SwimFormEnter.ToSoundStyle(), 0.3f, 0.2f, 10, 0, Player.Center);
                    break;
            }
        }

        public override void PreUpdate()
        {
            _yCameraOffset = MathHelper.Lerp(_yCameraOffset, _yCameraOffsetGoal, 0.1f);

            switch (state)
            {
                case stateHuman:
                    if (InputHelper.GetInputY() == -1
                        && !Player.wet
                        && Player.ItemTimeIsZero
                        && Player.grappling[0] == -1
                        && !Player.mount.Active
                        && !Player.GetModPlayer<DualiePlayer>().isRolling)
                    {
                        SetState(stateSquid);
                        return;
                    }

                    break;

                case stateSquid:
                    Player.noFallDmg = true;
                    Player.AddBuff(ModContent.BuffType<SwimFormBuff>(), 2);

                    if (_squidFormProjectile == null)
                    {
                        SetState(stateHuman);
                        return;
                    }

                    if (InputHelper.GetInputY() != -1 || Player.wet || Player.grappling[0] != -1)
                    {
                        _squidFormProjectile.StartDespawn();
                    }

                    if (Math.Abs(Player.velocity.X) > 4)
                    {
                        Player.velocity.X *= 0.9f;
                    }

                    _yCameraOffsetGoal = PlayerHelper.IsPlayerGrounded(Player) ? 20 : 0;

                    if (InputHelper.GetInputJump() && _squidJumpTime > 0)
                    {
                        Player.velocity.Y = -6;
                    }

                    if (PlayerHelper.IsPlayerGrounded(Player))
                    {
                        _squidJumpTime = _squidJumpTimeMax;

                        if (Math.Abs(Player.velocity.X) > 1)
                        {
                            if (Main.time % 30 == 0)
                            {
                                int soundId = Main.rand.Next(5);
                                var path = SoundPaths.Slime00;
                                path = path.Remove(path.Length - 1) + soundId;

                                SoundHelper.PlayAudio(path.ToSoundStyle(), 0.2f, 0.2f, 10, 0, Player.Center);
                            }
                        }

                        if (!_landed)
                        {
                            _landed = true;

                            if (!PlayerHelper.IsPlayerOntopOfPlatform(Player))
                            {
                                _squidFormProjectile?.LandingEffect();
                            }
                        }
                    }
                    else
                    {
                        _landed = false;
                        _squidJumpTime--;

                        if (InputHelper.GetInputJumpReleased())
                        {
                            _squidJumpTime = 0;
                        }
                    }

                    break;
            }
        }

        public override void PreUpdateMovement()
        {
            if (state == stateSquid)
            {
                Player.gravity = Player.defaultGravity / 2;
                Player.rocketTime = 0;
                Player.wingTime = 0;
            }
        }

        public override void SetControls()
        {
            if (state == stateSquid)
            {
                //Player.controlDown = false;
                //Player.controlLeft = false;
                //Player.controlRight = false;
                //Player.controlUp = false;
                Player.controlJump = false;
                Player.controlHook = false;
                Player.controlUseTile = false;
                Player.controlThrow = false;
            }
        }

        public bool IsSquid()
        {
            return state == stateSquid;
        }

        public bool IsFlat()
        {
            var result = IsSquid() && PlayerHelper.IsPlayerGrounded(Player);
            return result;
        }
    }
}
