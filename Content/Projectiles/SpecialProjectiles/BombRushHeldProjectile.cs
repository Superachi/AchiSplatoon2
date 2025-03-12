using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Gores;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.ModConfigs;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class BombRushHeldProjectile : BaseProjectile
    {
        // State machine
        private const int _stateFlip = 0;
        private const int _stateReady = 1;
        private const int _stateDespawn = 2;

        // Visuals
        private bool _flipDone = false;

        // Audio
        private SlotId? _bombRushJingle = null;
        private float _oldMusicVolume = 1f;
        private bool _jingleDisabledViaConfig = false;

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

            _jingleDisabledViaConfig = ModContent.GetInstance<ClientConfig>().DisableBombRushJingle;
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

                    Owner.GetModPlayer<StatisticsPlayer>().bombRushesUsed++;
                    break;

                case _stateReady:
                    Owner.fullRotation = 0;
                    break;

                case _stateDespawn:
                    weaponPlayer.allowSubWeaponUsage = false;

                    SoundHelper.StopSoundIfActive(_bombRushJingle);
                    PlayAudio(SoundPaths.BombRushActivate.ToSoundStyle(), volume: 0.5f, position: Owner.Center);
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

            // Bomb rush activation sound
            if (timeSpentAlive == 15)
            {
                PlayAudio(SoundPaths.BombRushActivate.ToSoundStyle(), volume: 0.5f, position: Owner.Center);
            }

            // Set the main music volume low, and play the jingle
            if (timeSpentInState > _startDelay)
            {
                _oldMusicVolume = Main.musicVolume;

                var soundPath = Owner.GetModPlayer<StatisticsPlayer>().bombRushesUsed % 2 == 0 ? SoundPaths.BombRushJingleAlt : SoundPaths.BombRushJingle;
                var style = soundPath.ToSoundStyle() with
                {
                    Volume = 1f
                };

                if (!_jingleDisabledViaConfig)
                {
                    Main.musicVolume = 0.001f;
                    _bombRushJingle = SoundEngine.PlaySound(style);
                }

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

            Owner.AddBuff(ModContent.BuffType<BombRushBuff>(), 2);

            if (timeSpentInState == 2)
            {
                Owner.GetModPlayer<WeaponPlayer>().allowSubWeaponUsage = true;
            }

            if (!_jingleDisabledViaConfig && timeSpentInState % 23 == 0)
            {
                var g = Gore.NewGoreDirect(Terraria.Entity.GetSource_None(), Owner.Center + new Vector2(0, -20), Vector2.Zero, ModContent.GoreType<MusicNoteGore>(), 1);
                g.velocity = new Vector2(-Owner.direction * Main.rand.NextFloat(0.4f, 0.6f), Main.rand.NextFloat(-1f, -1.5f));
                g.scale = 0;
            }
        }

        private void StateDespawn()
        {
            if (timeSpentInState > 15)
            {
                Owner.GetModPlayer<SpecialPlayer>().UnreadySpecial();
                Owner.GetModPlayer<WeaponPlayer>().allowSubWeaponUsage = true;

                Projectile.Kill();
            }
        }

        private void ResetMusic()
        {
            Main.musicVolume = _oldMusicVolume;
            SoundHelper.StopSoundIfActive(_bombRushJingle);
        }

        public override void OnKill(int timeLeft)
        {
            ResetMusic();
        }
    }
}
