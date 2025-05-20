using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class SubWeaponPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            bool leftClicked = InputHelper.GetInputMouseLeftHold();
            bool rightClicked = InputHelper.GetInputSubWeaponHold();

            if (rightClicked)
            {
                if (Player.HeldItem.ModItem is BaseBomb)
                {
                    SearchAndUseSubWeapon(Player, Player.HeldItem);
                }
                else
                {
                    SearchAndUseSubWeapon(Player);
                }
            }
            else if (leftClicked && Player.HeldItem.ModItem is BaseBomb)
            {
                SearchAndUseSubWeapon(Player, Player.HeldItem);
            }
        }

        private void SearchAndUseSubWeapon(Player player, Item? heldItem = null)
        {
            if (!InputHelper.IsPlayerAllowedToUseItem(player)) return;

            // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory

            Item? firstItemMatch;
            if (heldItem == null)
            {
                firstItemMatch = InventoryHelper.FirstInInventoryRange<BaseBomb>(Player, 0, 58);
                if (firstItemMatch == null) return;
            }
            else
            {
                if (heldItem.ModItem is not BaseBomb) return;
                firstItemMatch = heldItem;
            }

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
