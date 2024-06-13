using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseProjectile : ModProjectile
    {
        public InkColor inkColor = InkColor.Blue;

        public void Initialize()
        {
            // Check the highest color chip amount, set the ink color to match it
            var highest = 0;
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            int[] colorChipAmounts = modPlayer.ColorChipAmounts;
            if (IsThisClientTheProjectileOwner()) {
                if (colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Red] > highest)
                {
                    highest = colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Red];
                    inkColor = InkColor.Red;
                }
                if (colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Blue] > highest)
                {
                    highest = colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Blue];
                    inkColor = InkColor.Blue;
                }
                if (colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Yellow] > highest)
                {
                    highest = colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Yellow];
                    inkColor = InkColor.Yellow;
                }
                if (colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Purple] > highest)
                {
                    highest = colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Purple];
                    inkColor = InkColor.Purple;
                }
                if (colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Green] > highest)
                {
                    highest = colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Green];
                    inkColor = InkColor.Green;
                }
                if (colorChipAmounts[(int)InkWeaponPlayer.ChipColor.Aqua] > highest)
                {
                    inkColor = InkColor.Aqua;
                }
            }
        }

        /// <summary>
        /// This check makes sure that the current player (aka client) is the owner of the projectile
        /// This is used for example to make sure not all clients/server spawns ammo when the projectile is destroyed
        /// See OnKill method here: http://docs.tmodloader.net/docs/stable/class_mod_projectile.html
        /// </summary>
        /// <returns></returns>
        public bool IsThisClientTheProjectileOwner()
        {
            return Main.myPlayer == Projectile.owner;
        }

        protected void SyncProjectilePosWithPlayer(Player owner)
        {
            Projectile.position = owner.Center;
        }

        protected void SyncProjectilePosWithWeaponBarrel(Vector2 position, Vector2 velocity, BaseWeapon weaponData)
        {
            Vector2 weaponOffset = weaponData.HoldoutOffset() ?? new Vector2(0, 0);
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * weaponData.MuzzleOffsetPx;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                Projectile.position += muzzleOffset;
            }
        }

        protected void PlayerItemAnimationFaceCursor(Player owner)
        {
            // Change player direction depending on what direction the charger is held when charging
            var mouseDirRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
            var mouseDirDegrees = MathHelper.ToDegrees(mouseDirRadians);

            if (mouseDirDegrees >= -90 && mouseDirDegrees <= 90)
            {
                owner.direction = 1;
                owner.itemRotation = mouseDirRadians;
            }
            else
            {
                owner.direction = -1;
                owner.itemRotation = MathHelper.ToRadians((mouseDirDegrees + 180) % 360);
            }

            owner.itemAnimation = owner.itemAnimationMax;
            owner.itemTime = owner.itemTimeMax;
        }

        protected static void PlayAudio(string soundPath, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f)
        {
            var sample = new SoundStyle($"AchiSplatoon2/Content/Assets/Sounds/{soundPath}");
            var chargeSound = sample with
            {
                Volume = volume,
                PitchVariance = pitchVariance,
                MaxInstances = maxInstances,
                Pitch = pitch,
            };
            SoundEngine.PlaySound(chargeSound);
        }
    }
}
