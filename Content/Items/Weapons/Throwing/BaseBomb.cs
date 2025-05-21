using AchiSplatoon2.Attributes;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    [ItemCategory("Sub weapon", "Throwing")]
    internal class BaseBomb : BaseWeapon
    {
        public override float InkCost { get => 70f; }
        public override float InkRecoveryDelay { get => 60f; }

        public virtual int ExplosionRadius { get => 100; }
        public virtual int MaxBounces { get => 12; }
        public override bool IsSubWeapon => true;

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 20f;
            Item.useTime = 23;
            Item.useAnimation = Item.useTime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Blue;
            Item.ammo = Item.type;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }
    }
}
