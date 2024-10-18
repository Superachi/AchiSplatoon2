using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class BaseModPlayer : ModPlayer
    {
        protected int heldType = 0;
        protected int oldHeldType = 0;

        public override void PreUpdate()
        {
            if (CheckIfHeldItemChanged())
            {
                HeldItemChangeTrigger();
            }

            UpdateOldHeldTypeToMatchNew();
            UpdateCurrentHeldType();
        }

        protected virtual void HeldItemChangeTrigger()
        {
        }

        protected bool DoesModPlayerBelongToLocalClient()
        {
            return Player.whoAmI == Main.LocalPlayer.whoAmI;
        }

        public bool IsPlayerGrounded()
        {
            return Player.velocity.Y == 0 && Player.oldVelocity.Y == 0;
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

        protected ModItem HeldModItem()
        {
            return Player.HeldItem.ModItem;
        }

        public bool HasHeldItemChanged() => CheckIfHeldItemChanged();

        private bool CheckIfHeldItemChanged()
        {
            return oldHeldType != heldType;
        }

        private void UpdateOldHeldTypeToMatchNew()
        {
            oldHeldType = heldType;
        }

        private void UpdateCurrentHeldType()
        {
            heldType = Player.HeldItem.type;
        }
    }
}
