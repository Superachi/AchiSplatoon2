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
        private InkColor primaryColor = InkColor.Red;
        private InkColor secondaryColor = InkColor.Red;
        private int primaryHighest = 0;
        private int secondaryHighest = 0;

        public void Initialize()
        {
            // Check the highest color chip amounts, set the ink color to match the top 2
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

            if (IsThisClientTheProjectileOwner()) {
                var t = "";
                for (int i = 0; i < modPlayer.ColorChipAmounts.Length; i++)
                {
                    int value = modPlayer.ColorChipAmounts[i];

                    // Only consider the color if we have any chips for it
                    if (value > 0)
                    {
                        // Change the primary color if we see a new highest count
                        if (value > primaryHighest)
                        {
                            // If we've no other colors, make the secondary color match the primary one
                            if (secondaryHighest == 0)
                            {
                                secondaryColor = (InkColor)i;
                                secondaryHighest = value;
                            }
                            // If we do, mark the previous primary color as the secondary color
                            else
                            {
                                secondaryColor = primaryColor;
                                secondaryHighest = primaryHighest;
                            }

                            primaryColor = (InkColor)i;
                            primaryHighest = value;
                        }
                        // What if we don't have the highest count?
                        else if (primaryColor == secondaryColor || value > secondaryHighest)
                        {
                            secondaryColor = (InkColor)i;
                            secondaryHighest = value;
                        }
                    }
                    t += $"{modPlayer.ColorChipAmounts[i]},";
                }
                //Main.NewText($"Chips: {t}");
                //Main.NewText($"Primary: {primaryColor}, Chips: {primaryHighest}");
                //Main.NewText($"Secondary: {secondaryColor}, Chips: {secondaryHighest}");
            }
        }

        public Color GenerateInkColor()
        {
            // If there are two color chips being considered, add a bias towards the color that we have more chips of
            var amount = 0.5f;
            if (primaryHighest != secondaryHighest) { amount = 0.35f; }
            return ColorHelper.LerpBetweenInkColors(primaryColor, secondaryColor, amount);
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
