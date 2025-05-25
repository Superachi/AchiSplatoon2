using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal class RandomHelper
    {
        public static int GetRandomWeaponWithIngredient(List<int> requiredIngredients)
        {
            var weapons = new List<int>();

            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.createItem.ModItem is not BaseWeapon)
                {
                    continue;
                }

                bool elligible = true;
                foreach (var ingredient in requiredIngredients)
                {
                    if (!recipe.ContainsIngredient(ingredient))
                    {
                        elligible = false;
                        break;
                    }
                }

                if (elligible)
                {
                    weapons.Add(recipe.createItem.type);
                }
            }

            var result = Main.rand.NextFromCollection(weapons);
            return result;
        }

        public static int GetRandomColorChip()
        {
            var list = new List<int>();

            list.Add(ModContent.ItemType<ColorChipRed>());
            list.Add(ModContent.ItemType<ColorChipBlue>());
            list.Add(ModContent.ItemType<ColorChipGreen>());
            list.Add(ModContent.ItemType<ColorChipYellow>());
            list.Add(ModContent.ItemType<ColorChipAqua>());
            list.Add(ModContent.ItemType<ColorChipPurple>());

            return Main.rand.NextFromCollection(list);
        }

        public static int GetRandomPurifiedWeapon()
        {
            var list = new List<int>();

            list.Add(ModContent.ItemType<ShimmerOrderBlaster>());
            list.Add(ModContent.ItemType<ShimmerOrderBrella>());
            list.Add(ModContent.ItemType<ShimmerOrderBrush>());
            list.Add(ModContent.ItemType<ShimmerOrderCharger>());
            list.Add(ModContent.ItemType<ShimmerOrderDualie>());

            list.Add(ModContent.ItemType<ShimmerOrderRoller>());
            list.Add(ModContent.ItemType<ShimmerOrderShot>());
            list.Add(ModContent.ItemType<ShimmerOrderSplatana>());
            list.Add(ModContent.ItemType<ShimmerOrderSplatling>());
            list.Add(ModContent.ItemType<ShimmerOrderStringer>());

            list.Add(ModContent.ItemType<ShimmerOrderSlosher>());

            return Main.rand.NextFromCollection(list);
        }

        public static int GetRandomHardmodeOreWeapon()
        {
            var oreWeapons = new List<int>();

            // Find all weapons that are crafted using silver sheldon licences and hardmode ores
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.createItem.ModItem is not BaseWeapon)
                {
                    continue;
                }

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

                oreWeapons.Add(recipe.createItem.type);
            }

            var result = Main.rand.NextFromCollection(oreWeapons);
            return result;
        }

        public static int GetRandomMainWeaponBoosterAccessory()
        {
            var list = new List<int>();

            list.Add(ModContent.ItemType<AdamantiteCoil>());
            list.Add(ModContent.ItemType<CrayonBox>());
            list.Add(ModContent.ItemType<FieryPaintCan>());
            list.Add(ModContent.ItemType<FreshQuiver>());
            list.Add(ModContent.ItemType<MarinatedNecklace>());
            list.Add(ModContent.ItemType<PinkSponge>());
            list.Add(ModContent.ItemType<SquidClipOns>());
            list.Add(ModContent.ItemType<TentacularOcular>());

            return Main.rand.NextFromCollection(list);
        }
    }
}
