using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
    }
}
