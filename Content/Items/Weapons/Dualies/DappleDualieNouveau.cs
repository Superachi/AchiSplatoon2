using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class DappleDualieNouveau : DappleDualie
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 30;
            Item.shootSpeed = 3.5f;
            Item.crit = 5;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeOrichalcum();
    }
}
