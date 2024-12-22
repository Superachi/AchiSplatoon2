using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AchiSplatoon2.Content.Items.Accessories;

namespace AchiSplatoon2.Content.Players
{
    internal class SubWeaponPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (InputHelper.GetInputMouseRightHold())
            {
                SearchAndUseSubWeapon(Player, Player.HeldItem);
            }
        }

        private void SearchAndUseSubWeapon(Player player, Item heldItem)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return;
            if (!player.ItemTimeIsZero) return;
            if (!player.GetModPlayer<WeaponPlayer>().allowSubWeaponUsage) return;
            if (player.GetModPlayer<SquidPlayer>().IsSquid()) return;
            if (player.GetModPlayer<DualiePlayer>().isRolling) return;

            if (player.HasBuff(BuffID.Cursed)) return;
            if (player.HasBuff(BuffID.Frozen)) return;
            if (player.HasBuff(BuffID.Stoned)) return;

            if (Main.hoverItemName != "") return;

            // Ammo slots range from 54-57
            // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory
            var firstItemMatch = InventoryHelper.FirstInInventoryRange<BaseBomb>(Player, 54, 57);
            if (firstItemMatch == null) return;

            var subWeapon = (BaseBomb)firstItemMatch.ModItem;
            var baseInkCost = WoomyMathHelper.CalculateWeaponInkCost(subWeapon, player);
            var discount = 0f;

            if (heldItem.ModItem is BaseWeapon heldWeapon)
            {
                if (heldWeapon.BonusSub != SubWeaponType.None)
                {
                    SubWeaponType currentlyCheckedSub = (SubWeaponType)subWeapon.Type;
                    if (heldWeapon.BonusType == SubWeaponBonusType.Discount && currentlyCheckedSub == heldWeapon.BonusSub)
                    {
                        discount = baseInkCost * heldWeapon.SubBonusAmount;
                    }
                }
            }

            var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
            if (!inkTankPlayer.HasEnoughInk(baseInkCost - discount))
            {
                return;
            }

            inkTankPlayer.ConsumeInk(baseInkCost - discount);

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
