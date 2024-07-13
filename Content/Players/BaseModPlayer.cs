using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Players
{
    internal class BaseModPlayer : ModPlayer
    {
        protected ModItem heldItem;
        protected ModItem oldHeldItem;
        protected bool heldItemChanged;

        protected bool CheckIfHeldItemChanged()
        {
            bool changed = false;
            if (oldHeldItem == null)
            {
                UpdateOldHeldItem();
                changed = true;
            }
            else if (oldHeldItem.Type != heldItem.Type)
            {
                changed = true;
            }

            return changed;
        }

        protected void UpdateOldHeldItem()
        {
            oldHeldItem = heldItem;
        }

        protected bool DoesModPlayerBelongToLocalClient()
        {
            return Player.whoAmI == Main.LocalPlayer.whoAmI;
        }

        protected BaseProjectile CreateProjectileWithWeaponProperties(Player player, int type, BaseWeapon weaponType, bool triggerAfterSpawn = true, Vector2 ? velocity = null, int damage = 0, float knockback = 0f)
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
