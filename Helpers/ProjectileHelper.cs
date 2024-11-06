using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal static class ProjectileHelper
    {
        public static BaseProjectile CreateProjectileWithWeaponProperties(Player player, int type, BaseWeapon weaponType, bool triggerSpawnMethods = true, Vector2? velocity = null, int damage = 0, float knockback = 0f)
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

            if (triggerSpawnMethods) proj.RunSpawnMethods();
            return proj;
        }
    }
}
