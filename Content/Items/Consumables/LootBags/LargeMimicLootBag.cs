using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.ExtensionMethods;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables.LootBags
{
    [Autoload(false)]
    internal class LargeMimicLootBag : BaseLootBag
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.SetValueLowHardmodeOre();
        }

        protected int RollRandomWeapon()
        {
            var oreWeapons = new List<int>();

            // Find all weapons that are crafted using silver sheldon licences and hardmode ores
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (!recipe.ContainsIngredient(ModContent.ItemType<SheldonLicenseSilver>()))
                {
                    continue;
                }

                if (!recipe.ContainsIngredient(ItemID.CobaltBar)
                && !recipe.ContainsIngredient(ItemID.PalladiumBar)
                && !recipe.ContainsIngredient(ItemID.MythrilBar)
                && !recipe.ContainsIngredient(ItemID.OrichalcumBar)
                && !recipe.ContainsIngredient(ItemID.AdamantiteBar)
                && !recipe.ContainsIngredient(ItemID.TitaniumBar))
                {
                    continue;
                }

                if (recipe.createItem.ModItem is not BaseWeapon)
                {
                    continue;
                }

                oreWeapons.Add(recipe.createItem.type);
            }

            var result = Main.rand.NextFromCollection(oreWeapons);
            return result;
        }

        protected override void OpenLootBag(Player player)
        {
            player.QuickSpawnItem(player.GetSource_DropAsItem(), RollRandomWeapon());
        }
    }
}
