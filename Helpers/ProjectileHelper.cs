using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using System;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Helpers
{
    internal static class ProjectileHelper
    {
        public static BaseProjectile CreateProjectileWithWeaponProperties(Player player, int type, BaseWeapon weaponType, bool triggerAfterSpawn = true, Vector2? velocity = null, int damage = 0, float knockback = 0f)
        {
            Vector2 position = player.Center;
            if (velocity == null) velocity = Vector2.Zero;

            // Spawn the projectile
            var p = Projectile.NewProjectileDirect(
                spawnSource: new EntitySource_ItemUse(player, player.HeldItem),
                position: position,
                velocity: (Vector2)velocity,
                type: type,
                damage: damage,
                knockback: knockback,
                owner: player.whoAmI);
            var proj = p.ModProjectile as BaseProjectile;

            // Config variables after spawning
            proj.WeaponInstance = (BaseWeapon)Activator.CreateInstance(weaponType.GetType());
            proj.itemIdentifier = weaponType.ItemIdentifier;

            if (triggerAfterSpawn) proj.AfterSpawn();
            return proj;
        }
    }
}
