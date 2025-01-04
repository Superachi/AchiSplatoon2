using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.TacticoolerProjectiles
{
    internal class TacticoolerHeldProjectile : BaseProjectile
    {
        // State machine
        private const int _stateFlip = 0;
        private const int _stateReady = 1;
        private const int _stateThrow = 2;

        // Visuals
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

            Projectile.Center = Owner.Center;
            Projectile.timeLeft++;

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
                    break;

                case _stateReady:
                    break;

                case _stateThrow:
                    break;
            }
        }

        private void HideWeapon()
        {
            Owner.itemLocation = new Vector2(-1000, -1000);
        }

        private void StateFlip()
        {
            HideWeapon();

            // Perform the flip
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

            if (timeSpentInState > _startDelay)
            {
                AdvanceState();
            }
        }

        private void StateReady()
        {
            if (InputHelper.GetInputMouseLeftHold())
            {
                AdvanceState();
            }
        }

        private void StateThrow()
        {
            if (!IsThisClientTheProjectileOwner()) return;

            CreateChildProjectile<TacticoolerStand>(Owner.Center, Owner.DirectionTo(Main.MouseWorld) * 15, 0, true);
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            Owner.GetModPlayer<WeaponPlayer>().allowSubWeaponUsage = true;
        }
    }
}
