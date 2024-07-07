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

        public float CalculateDamageMod(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

                // Apply main weapon bonus here
                bool hasMainWeaponBonus = false;
                if (player.HeldItem.ModItem is BaseWeapon)
                {
                    var heldItem = (BaseWeapon)player.HeldItem.ModItem;

                    // If we DO have a sub weapon bonus AND its of type damage
                    if (heldItem.BonusSub != SubWeaponType.None && heldItem.BonusType == SubWeaponBonusType.Damage)
                    {
                        // Check if we match the sub weapon type
                        int bonusId = (int)heldItem.BonusSub - 1;
                        if (subWeaponItemIDs[bonusId] == Item.type)
                        {
                            hasMainWeaponBonus = true;
                        }
                    }
                }

                // Regardless, always check for the sub power emblem
                return modPlayer.CalculateSubDamageBonusModifier(hasMainWeaponBonus);
            }

            return 1f;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= CalculateDamageMod(player);
        }
    }
}
