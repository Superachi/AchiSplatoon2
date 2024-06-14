using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Dynamic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons
{
    internal class BaseWeapon : ModItem
    {
        public virtual string ShootSample { get => "SplattershotShoot"; }
        public virtual string ShootWeakSample { get => "SplattershotShoot"; }
        public virtual float MuzzleOffsetPx { get; set; } = 0f;

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Track the item that is being used as the player is about to shoot
            // In BaseProjectile.cs -> Initialize(), this value is referenced in order to get information relevant to a weapon
            var modPlayer = player.GetModPlayer<ItemTrackerPlayer>();
            modPlayer.lastUsedWeapon = (BaseWeapon)Activator.CreateInstance(this.GetType());

            // Adjust the position of the projectile (that is about to spawn) to better match the weapon sprite
            Vector2 weaponOffset = HoldoutOffset() ?? new Vector2(0, 0);
            Vector2 muzzleOffset = Vector2.Add(Vector2.Normalize(velocity) * MuzzleOffsetPx, Vector2.Normalize(velocity) * weaponOffset);

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
    }
}
