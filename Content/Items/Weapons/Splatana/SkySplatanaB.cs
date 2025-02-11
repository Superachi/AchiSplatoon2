using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SkySplatanaB : SkySplatana
    {
        public override int BaseDamage { get => 60; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 6;
            Item.crit = 10;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipeChlorophyteUpgrade(true, ModContent.ItemType<SkySplatana>());
    }
}
