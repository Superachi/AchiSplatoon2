using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class BaseBomb : BaseWeapon
    {
        public virtual int ExplosionRadius { get => 100; }
        public virtual int MaxBounces { get => 10; }
        public override bool IsSubWeapon => true;
        public override bool AllowSubWeaponUsage { get => false; }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 18f;
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

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Apply the sub power emblem accessory bonus here
            // ALSO apply a main weapon's sub damage bonus here, so it shows up in the tooltip
            // It should not double-dip the damage
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
                if (modPlayer.hasSubPowerEmblem)
                {
                    damage *= InkWeaponPlayer.subPowerMultiplier;
                }

                if (player.HeldItem.ModItem is BaseWeapon)
                {
                    var item = (BaseWeapon)player.HeldItem.ModItem;
                    if (item.BonusSub == SubWeaponType.None) { return; }

                    int bonusId = (int)item.BonusSub - 1;
                    if (item.BonusType == SubWeaponBonusType.Damage && subWeaponItemIDs[bonusId] == Item.type)
                    {
                        damage *= 1 + subDamageBonus;
                    }
                }
            }
        }
    }
}
