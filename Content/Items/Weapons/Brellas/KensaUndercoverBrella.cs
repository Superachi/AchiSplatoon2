using AchiSplatoon2.Content.Projectiles.BrellaProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Brellas
{
    internal class KensaUndercoverBrella : UndercoverBrella
    {
        public override int ShieldLife => 300;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 44;
            Item.knockBack = 3;

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
