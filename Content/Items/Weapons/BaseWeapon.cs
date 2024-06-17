using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
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
        public virtual bool AllowSubWeaponUsage { get => true; }

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

        public override bool AltFunctionUse(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return false;
            if (!player.ItemTimeIsZero) return false;
            if (!AllowSubWeaponUsage) return false;

            bool doneSearching = false;
            int[] idsToCheck = {
                    ModContent.ItemType<SplatBomb>(),
                    ModContent.ItemType<BurstBomb>(),
                    ModContent.ItemType<AngleShooter>(),
                    ModContent.ItemType<Sprinkler>(),
                };

            BaseBomb[] subWeaponData = {
                    new SplatBomb(),
                    new BurstBomb(),
                    new AngleShooter(),
                    new Sprinkler(),
                };

            // We use 4 here, as there are 4 ammo slots
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < idsToCheck.Length; j++)
                {
                    if (!doneSearching)
                    {
                        var item = player.inventory[54 + i];
                        // Ammo slots range from 54-57
                        // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory
                        if (item.type == idsToCheck[j])
                        {
                            item.stack--;
                            if (item.stack == 0)
                            {
                                CombatTextHelper.DisplayText($"Used last {item.Name}!", player.Center);
                            }

                            // Calculate angle/velocity
                            float aimAngle = MathHelper.ToDegrees(
                                player.DirectionTo(Main.MouseWorld).ToRotation()
                            );

                            float radians = MathHelper.ToRadians(aimAngle);
                            Vector2 angleVector = radians.ToRotationVector2();
                            Vector2 velocity = angleVector;
                            var source = new EntitySource_ItemUse_WithAmmo(player, item, item.ammo);

                            var modPlayer = player.GetModPlayer<ItemTrackerPlayer>();
                            modPlayer.lastUsedWeapon = (BaseWeapon)Activator.CreateInstance(subWeaponData[j].GetType());

                            // Specifically for the sprinkler, prevent usage if one is already active
                            if (item.type == ModContent.ItemType<Sprinkler>() && player.ownedProjectileCounts[item.shoot] >= 1)
                            {
                                CombatTextHelper.DisplayText("Sprinkler already active!", player.Center);
                                return false;
                            }

                            Projectile.NewProjectile(
                                spawnSource: source,
                                position: player.Center,
                                velocity: velocity * item.shootSpeed,
                                Type: item.shoot,
                                Damage: item.damage,
                                KnockBack: item.knockBack,
                                Owner: Main.myPlayer);

                            player.itemTime = item.useTime;
                            doneSearching = true;
                            break;
                        }
                    }
                }
            }

            if (!doneSearching)
            {
                CombatTextHelper.DisplayText("No sub weapon equipped!", player.Center);
                return false;
            }

            return true;
        }
    }
}
