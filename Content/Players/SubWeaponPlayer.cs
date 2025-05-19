using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class SubWeaponPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (InputHelper.GetInputSubWeaponHold())
            {
                SearchAndUseSubWeapon(Player, Player.HeldItem);
            }
        }

        private void SearchAndUseSubWeapon(Player player, Item heldItem)
        {
            var weaponPlayer = player.GetModPlayer<WeaponPlayer>();
            if (CursorHelper.CursorHasInteractable())
            {
                if (weaponPlayer.CustomWeaponCooldown < 30)
                {
                    player.GetModPlayer<WeaponPlayer>().CustomWeaponCooldown = 30;
                }
                return;
            }

            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return;
            if (!player.ItemTimeIsZero) return;

            if (weaponPlayer.CustomWeaponCooldown > 0) return;
            if (!weaponPlayer.allowSubWeaponUsage) return;
            if (weaponPlayer.isBrushRolling || weaponPlayer.isBrushAttacking) return;
            if (Player.OwnsModProjectileWithType(ModContent.ProjectileType<BrushSwingProjectile>()))
            {
                return;
            }

            if (player.GetModPlayer<SquidPlayer>().IsSquid()) return;
            if (player.GetModPlayer<DualiePlayer>().isRolling || player.GetModPlayer<DualiePlayer>().postRollCooldown > 0) return;

            if (player.HasBuff(BuffID.Cursed)) return;
            if (player.HasBuff(BuffID.Frozen)) return;
            if (player.HasBuff(BuffID.Stoned)) return;

            if (Main.hoverItemName != "") return;

            // Ammo slots range from 54-58
            // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory
            var firstItemMatch = InventoryHelper.FirstInInventoryRange<BaseBomb>(Player, 54, 58);
            if (firstItemMatch == null) return;

            var subWeapon = (BaseBomb)firstItemMatch.ModItem;
            var baseInkCost = WoomyMathHelper.CalculateWeaponInkCost(subWeapon, player);

            var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
            if (!inkTankPlayer.HasEnoughInk(baseInkCost))
            {
                return;
            }

            inkTankPlayer.ConsumeInk(baseInkCost);

            // Calculate throw angle and spawn projectile
            float aimAngle = MathHelper.ToDegrees(
                player.DirectionTo(Main.MouseWorld).ToRotation()
            );

            float radians = MathHelper.ToRadians(aimAngle);
            Vector2 angleVector = radians.ToRotationVector2();
            Vector2 velocity = angleVector;

            var p = ProjectileHelper.CreateProjectileWithWeaponProperties(
                player: player,
                type: firstItemMatch.shoot,
                damage: firstItemMatch.damage,
                knockback: firstItemMatch.knockBack,
                velocity: velocity * firstItemMatch.shootSpeed,
                weaponType: (BaseWeapon)firstItemMatch.ModItem,
                triggerSpawnMethods: false);

            p.Projectile.position = player.Center;
            p.itemIdentifier = firstItemMatch.type;
            p.weaponSourcePrefix = firstItemMatch.prefix;
            p.RunSpawnMethods();

            var useTime = firstItemMatch.useTime;
            if (player.GetModPlayer<AccessoryPlayer>().HasAccessory<HypnoShades>())
            {
                useTime = (int)(useTime * HypnoShades.BombUseTimeMult);
            }

            player.itemTime = useTime;
            player.itemAnimation = 0;
        }
    }
}
