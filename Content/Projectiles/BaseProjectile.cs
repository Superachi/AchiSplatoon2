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

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseProjectile : ModProjectile
    {
        protected InkColor inkColor = InkColor.Blue;
        protected bool isVisible = false;
        protected float delayUntilVisible = 0f;

        public void Initialize(InkColor color, bool visible, float visibleDelay)
        {
            inkColor = color;
            isVisible = visible;
            delayUntilVisible = visibleDelay;
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
