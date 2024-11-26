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
        public const int stateTransformBack = 2;

        private SquidFormProjectile? _squidFormProjectile = null;
        private float _oldOpacityForAnimation = 1f;
        private float _oldWingTime = 0f;
        private bool _landed = false;

        private float _yCameraOffset = 0f;
        private float _yCameraOffsetGoal = 0f;

        public void SetState(int state)
        {
            this.state = state;

            switch (this.state)
            {
                case stateHuman:
                    _squidFormProjectile = null;
                    Player.wingTime = _oldWingTime;

                    Player.opacityForAnimation = 1;
                    SoundHelper.PlayAudio("SwimForm/Exit", 0.2f, 0.2f, 10, 0, Player.Center);
                    _yCameraOffsetGoal = 0;
                    break;

                case stateSquid:
                    Player.mount.Dismount(Player);

                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<SquidFormProjectile>()] == 0)
                    {
                        _squidFormProjectile = (SquidFormProjectile)ProjectileHelper.CreateProjectile(Player, ModContent.ProjectileType<SquidFormProjectile>());
                        _squidFormProjectile.Projectile.velocity.X = Player.direction;
                        _squidFormProjectile.SetInitialParams(Player.velocity);
                    }

                    _oldWingTime = Player.wingTime;

                    Player.opacityForAnimation = 0;
                    SoundHelper.PlayAudio("SwimForm/Enter", 0.3f, 0.2f, 10, 0, Player.Center);
                    break;

                case stateTransformBack:
                    var wepMP = Player.GetModPlayer<WeaponPlayer>();
                    wepMP.CustomWeaponCooldown = 15;

                    if (_squidFormProjectile != null)
                    {
                        _squidFormProjectile.StartDespawn();
                    }
                    break;
            }
        }

        public override void PreUpdate()
        {
            _yCameraOffset = MathHelper.Lerp(_yCameraOffset, _yCameraOffsetGoal, 0.1f);

            switch (state)
            {
                case stateHuman:
                    if (InputHelper.GetInputY() == -1 && !Player.wet && Player.ItemTimeIsZero && Player.grappling[0] == -1)
                    {
                        SetState(stateSquid);
                        return;
                    }

                    break;

                case stateSquid:
                    if (InputHelper.GetInputY() != -1 || Player.wet)
                    {
                        SetState(stateTransformBack);
                        return;
                    }

                    if (Math.Abs(Player.velocity.X) > 4)
                    {
                        Player.velocity.X *= 0.9f;
                    }

                    _yCameraOffsetGoal = PlayerHelper.IsPlayerGrounded(Player) ? 20 : 0;

                    if (!PlayerHelper.IsPlayerGrounded(Player))
                    {
                        _landed = false;
                        _squidFormProjectile?.ResetDrawScale();
                    }
                    else
                    {
                        if (InputHelper.GetInputJumpPressed())
                        {
                            Player.velocity.Y -= 10;
                        }

                        if (Math.Abs(Player.velocity.X) > 1)
                        {
                            if (Main.time % 30 == 0)
                            {
                                SoundHelper.PlayAudio($"SwimForm/slime0{Main.rand.Next(5)}", 0.2f, 0.2f, 10, 0, Player.Center);
                            }
                        }

                        if (!_landed)
                        {
                            _landed = true;

                            if(!PlayerHelper.IsPlayerOntopOfPlatform(Player))
                            {
                                _squidFormProjectile?.LandingEffect();
                            }
                        }
                    }

                    break;

                case stateTransformBack:
                    if (_squidFormProjectile == null)
                    {
                        SetState(stateHuman);
                        return;
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
    }
}
