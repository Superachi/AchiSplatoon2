using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class BaseBomb : BaseWeapon
    {
        public virtual int ExplosionRadius { get => 100; }
        public virtual int MaxBounces { get => 12; }
        public override bool IsSubWeapon => true;
        public override bool AllowSubWeaponUsage { get => false; }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 20f;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useTime = 30;
            Item.useAnimation = Item.useTime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
            Item.ammo = Item.type;
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                if (player.GetModPlayer<AccessoryPlayer>().hasHypnoShades)
                {
                    return HypnoShades.BombUseTimeMod;
                }
            }

            return base.UseTimeMultiplier(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                if (player.GetModPlayer<AccessoryPlayer>().hasHypnoShades)
                {
                    player.itemAnimation = (int)(Item.useTime * HypnoShades.BombUseTimeMod);
                }
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
