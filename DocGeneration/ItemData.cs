using AchiSplatoon2.Helpers;
using Newtonsoft.Json;
using System.IO;
using Terraria.ModLoader;
using System.Collections.Generic;
using AchiSplatoon2.Content.Items.Weapons;
using Terraria;
using System.Text.RegularExpressions;
using Terraria.ID;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Items.Accessories;
using System;
using AchiSplatoon2.Attributes;
using NVorbis.Contracts;
using System.ComponentModel;
using System.Linq;

namespace AchiSplatoon2.DocGeneration
{
    public class ItemData
    {
        public ModItem ModdedItem { get; set; }

        public string Name { get; set; }
        public int Damage { get; set; }
        public float Knockback { get; set; }
        public float Crit { get; set; }
        public int UseTime { get; set; }
        public float Velocity { get; set; }
        public float InkCost { get; set; }

        public int Rarity { get; set; }
        public ItemMoneyValue Value { get; set; }

        public string Tooltip { get; set; }
        public string UsageHint { get; set; }
        public string Flavor { get; set; }

        public List<Recipe> Recipes { get; set; } = new();
        public string Category { get; set; }

        public ItemData(ModItem modItem)
        {
            ModdedItem = modItem;

            var item = modItem.Item;
            if (item == null)
            {
                DebugHelper.PrintError($"{nameof(ItemData)} - modItem.Item is null!");
                return;
            }

            Name = item.Name;
            Damage = item.damage;
            Knockback = item.knockBack;
            Crit = item.crit;
            UseTime = item.useTime;
            Velocity = item.shootSpeed;

            Rarity = item.rare;
            Value = new ItemMoneyValue
            {
                Copper = item.value % 100,
                Silver = item.value / 100 % 100,
                Gold = item.value / 10000 % 100,
                Platinum = item.value / 1000000
            };

            Tooltip = modItem.GetLocalization(nameof(Tooltip)).Value;

            UsageHint = modItem.GetLocalization(nameof(UsageHint)).Value;
            if (UsageHint.Contains(nameof(UsageHint)))
            {
                UsageHint = "";
            }

            Flavor = modItem.GetLocalization(nameof(Flavor)).Value;
            if (Flavor.Contains(nameof(Flavor)))
            {
                Flavor = "";
            }

            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.TryGetResult(ModdedItem.Item.type, out Item _)) {
                    Recipes.Add(recipe);
                }
            }

            SetCategory();
        }

        public static void ExportListAsJson(List<ItemData> itemDataList, string path)
        {
            DebugHelper.PrintDebug("Creating item data JSON...");
            File.WriteAllText(path, JsonConvert.SerializeObject(itemDataList));
            DebugHelper.PrintDebug("Created the JSON.");
        }

        public string GenerateItemTableHTML()
        {
            string html = "---\r\n---\r\n";

            // Name and description display
            var className = ModdedItem.GetType().Name;
            var folderSuffix = ModdedItem.GetType().Namespace
                .Replace("AchiSplatoon2.Content.", "")
                .Replace(".", "/");

            html += "<img src=\"{{ site.baseurl }}/assets/images/" + folderSuffix + "/" + className + ".png\" class=\"mx-auto d-block\">\n";
            html += $"\n<a id=\"{Name.ToLowerInvariant().Replace(" ", "-")}\"></a>\n";
            html += $"### {Name}\n\n";

            if (!string.IsNullOrEmpty(Flavor))
            {
                html += $"*{FormatInput(Flavor)}*\n\n";
            }

            if (!string.IsNullOrEmpty(Tooltip))
            {
                html += $"{FormatInput(Tooltip)}\n\n";
            }

            if (!string.IsNullOrEmpty(UsageHint))
            {
                html += $"**{FormatInput(UsageHint)}**\n";
            }

            html += "<table class=\"table\">";

            AddMarkdownTableRow(ref html, nameof(Category), Category);

            // Weapon stats display

            if (ModdedItem is BaseWeapon weapon)
            {
                if (Damage > 0)
                {
                    AddMarkdownTableRow(ref html, nameof(Damage), Damage.ToString());
                }
                else
                {
                    AddMarkdownTableRow(ref html, nameof(Damage), "No damage");
                }

                if (Knockback > 0)
                {
                    AddMarkdownTableRow(ref html, nameof(Knockback), Knockback.ToString());
                }
                else
                {
                    AddMarkdownTableRow(ref html, nameof(Knockback), "No knockback");
                }

                if (Crit > 0)
                {
                    AddMarkdownTableRow(ref html, "Bonus crit chance", Crit.ToString() + "%");
                }

                if (Velocity > 1)
                {
                    AddMarkdownTableRow(ref html, nameof(Velocity), Velocity.ToString());
                }

                var inkCost = GetWeaponInkCost(ModdedItem);
                if (inkCost > 0)
                {
                    AddMarkdownTableRow(ref html, "Ink usage", inkCost.ToString() + "%");
                }
            }

            // Value display

            var valueString = "";
            if (Value.Platinum > 0)
            {
                valueString += $"{Value.Platinum} platinum ";
            }

            if (Value.Gold > 0)
            {
                valueString += $"{Value.Gold} gold ";
            }

            if (Value.Silver > 0)
            {
                valueString += $"{Value.Silver} silver ";
            }

            if (Value.Copper > 0)
            {
                valueString += $"{Value.Copper} copper";
            }

            if (!string.IsNullOrEmpty(valueString))
            {
                AddMarkdownTableRow(ref html, nameof(Value), valueString);
            }

            AddMarkdownTableRow(ref html, nameof(Rarity), RarityToString());

            html += "\r\n</table>\r\n\n";

            // Recipes

            if (Recipes.Count > 0)
            {
                if (Recipes.Count > 1)
                {
                    var i = 0;

                    foreach (var recipe in Recipes)
                    {
                        i++;
                        html += $"<details>\r\n  <summary><b>Recipe {i}</b></summary>\r\n";

                        html += "  <span>\n";
                        foreach (var item in recipe.requiredItem)
                        {
                            var link = GetVanillaItemUrl(item);
                            link = link == "" ? GetModItemPageUrl(item) : link;

                            html += $"    <li>\r\n      {item.stack}x <b><a href=\"{link}\">{item.Name}</a></b>\r\n    </li>\r\n";
                        }
                        html += "\n  </span>";
                        html += "\r\n</details>\r\n";
                    }

                    html += "\n";
                }
                else
                {
                    html += $"<b>Recipe</b>\r\n";

                    foreach (var item in Recipes[0].requiredItem)
                    {
                        var link = GetVanillaItemUrl(item);
                        link = link == "" ? GetModItemPageUrl(item) : link;

                        html += $"    <li>\r\n      {item.stack}x <b><a href=\"{link}\">{item.Name}</a></b>\r\n    </li>\r\n";
                    }
                }
            }

            html += "---\n";

            return html;
        }

        public static void ExportItemsAsDetailPages(List<ItemData> itemDataList, string path)
        {
            DebugHelper.PrintDebug("Creating item detail pages...");

            foreach (var itemData in itemDataList)
            {
                if (itemData.Name.Contains("Base") || itemData.Name.Contains("Test"))
                {
                    continue;
                }

                var html = itemData.GenerateItemTableHTML();
                var className = itemData.ModdedItem.GetType().Name;
                var newPath = Path.Combine(path, className + ".html");
                File.WriteAllText(newPath, html);
            }

            DebugHelper.PrintDebug("Created the HTML.");
        }

        public static void ExportWeaponsAsCategoryPage(List<ItemData> itemDataList, string path, Type baseItemType)
        {
            DebugHelper.PrintDebug("Creating item category pages...");

            List<ItemData> filteredList = new();
            foreach (var itemData in itemDataList)
            {
                if (!itemData.ModdedItem.GetType().IsSubclassOf(baseItemType))
                {
                    continue;
                }

                DebugHelper.PrintInfo($"Printing include line for {itemData.Name}");

                if (ShouldItemBeExcluded(itemData))
                {
                    continue;
                }

                filteredList.Add(itemData);
            }

            filteredList = filteredList.OrderBy(x => x.Rarity).ThenBy(x => x.Damage).ToList();

            var includeLines = "";
            foreach (var itemData in filteredList)
            {
                var categoryAttribute = Attribute.GetCustomAttribute(itemData.ModdedItem.GetType(), typeof(ItemCategoryAttribute)) as ItemCategoryAttribute;
                if (categoryAttribute == null)
                {
                    DebugHelper.PrintError($"Failed to create category page: {nameof(categoryAttribute)} was null");
                    return;
                }

                var className = itemData.ModdedItem.GetType().Name;

                var categoryLayout = "category-page/category-row.html";
                includeLines += "      {% include " + categoryLayout +
                    $" name=\"{itemData.Name}\" " +
                    $"damage=\"{itemData.Damage}\" knockback=\"{itemData.Knockback}\" " +
                    $"ink_usage=\"{GetWeaponInkCost(itemData.ModdedItem)}\" " +
                    $"rarity=\"{itemData.RarityToString()}\" " +
                    $"internal_name=\"{className}\" " +
                    $"image_uri=\"{categoryAttribute.DirectorySuffix}/{className}\"" + " %}\r\n";
            }

            var baseTypeCategoryAttribute = Attribute.GetCustomAttribute(baseItemType, typeof(ItemCategoryAttribute)) as ItemCategoryAttribute;
            if (baseTypeCategoryAttribute == null)
            {
                DebugHelper.PrintError($"Failed to create category page: {nameof(baseTypeCategoryAttribute)} was null");
                return;
            }

            var templateStart = "---\r\nlayout: category-page\r\n---\r\n\r\n<div class=\"card-header\">\r\n  <div class=\"col\"><b>" + $"{baseTypeCategoryAttribute.PluralizeCategory()}" + "</b></div>\r\n</div>\r\n\r\n<div class=\"card-body\">\r\n  <details open>\r\n    <summary><b>Show/hide</b></summary>\r\n    <table class=\"table\">\r\n      {% include category-page/category-table-head.html %}\r\n      <tbody>\r\n";
            var templateEnd = "      </tbody>\r\n    </table>\r\n  </details>\r\n</div>";
            var html = templateStart + includeLines + templateEnd;

            var newPath = Path.Combine(path, baseTypeCategoryAttribute.DirectorySuffix + ".html");
            File.WriteAllText(newPath, html);

            DebugHelper.PrintDebug("Created the HTML.");
        }

        private static bool ShouldItemBeExcluded(ItemData itemData)
        {
            if (itemData.Name.Contains("Base") || itemData.Name.Contains("Test"))
            {
                return true;
            }

            if (Attribute.GetCustomAttribute(itemData.ModdedItem.GetType(), typeof(DeveloperContentAttribute)) != null)
            {
                return true;
            }

            if (Attribute.GetCustomAttribute(itemData.ModdedItem.GetType(), typeof(ItemCategoryAttribute)) == null)
            {
                return true;
            }

            return false;
        }

        private void AddMarkdownTableRow(ref string markdown, string fieldName, string fieldValue)
        {
            markdown += $"\r\n  <tr>\r\n    <th>{fieldName}</th>\r\n    <td>{fieldValue}</td>\r\n  </tr>";
        }

        private string GetVanillaItemUrl(Item item)
        {
            if (ItemLoader.GetItem(item.type) == null)
            {
                return $"https://terraria.wiki.gg/wiki/{item.Name.Replace(" ", "_")}";
            }

            return "";
        }

        private string GenerateVanillaItemLink(Item item)
        {
            var url = GetVanillaItemUrl(item);

            if (string.IsNullOrEmpty(url))
            {
                return item.Name;
            }

            return $"[{item.Name}]({url})";
        }

        private string GetModItemJumpUrl(Item item)
        {
            if (ItemLoader.GetItem(item.type) != null)
            {
                return $"#{item.Name.ToLowerInvariant().Replace(" ", "-")}";
            }

            return "";
        }

        private string GetModItemPageUrl(Item item)
        {
            var className = item.ModItem.GetType().Name;

            if (ItemLoader.GetItem(item.type) != null)
            {
                return $"./{className}";
            }

            return "";
        }

        private string GenerateModItemJumpLink(Item item)
        {
            var url = GetModItemJumpUrl(item);

            if (string.IsNullOrEmpty(url))
            {
                return item.Name;
            }

            return $"[{item.Name}]({url})";
        }

        private static string GenerateTableOfContent(List<ItemData> itemDataList)
        {
            var markdown = "### Table of content:\n\n";
            foreach (var item in itemDataList)
            {
                markdown += $"- [{item.Name}](#{item.Name.ToLowerInvariant().Replace(" ", "-")})\n";
            }
            return markdown;
        }

        private string FormatInput(string input)
        {
            string pattern = @"\{\d+\}";
            input = Regex.Replace(input, pattern, "X");

            if (!input.EndsWith(".") && !input.EndsWith("!") && !input.EndsWith("?"))
            {
                input += ".";
            }

            return input;
        }

        private string RarityToString()
        {
            switch (Rarity)
            {
                case ItemRarityID.Master:
                    return "Fiery Red";
                case ItemRarityID.Expert:
                    return "Rainbow";
                case ItemRarityID.Quest:
                    return "Quest";
                case ItemRarityID.Gray:
                    return "Gray";
                case ItemRarityID.White:
                    return "White";
                case ItemRarityID.Blue:
                    return "Blue";
                case ItemRarityID.Green:
                    return "Green";
                case ItemRarityID.Orange:
                    return "Orange";
                case ItemRarityID.LightRed:
                    return "Light Red";
                case ItemRarityID.Pink:
                    return "Pink";
                case ItemRarityID.LightPurple:
                    return "Light Purple";
                case ItemRarityID.Lime:
                    return "Lime";
                case ItemRarityID.Yellow:
                    return "Yellow";
                case ItemRarityID.Cyan:
                    return "Cyan";
                case ItemRarityID.Red:
                    return "Red";
                case ItemRarityID.Purple:
                    return "Purple";
                default:
                    return "Unknown";
            }
        }

        private void SetCategory()
        {
            Category = "Item";

            if (ModdedItem is BaseWeapon weapon)
            {
                Category = weapon.WeaponStyle.ToString();

                switch (ModdedItem)
                {
                    case BaseSpecial:
                        Category = "Special weapon";
                        break;
                    case BaseBomb:
                        Category = "Sub weapon";
                        break;
                }

                return;
            }

            if (ModdedItem is BaseAccessory)
            {
                Category = "Accessory";
            }
        }

        private static float GetWeaponInkCost(ModItem modItem)
        {
            if (modItem is BaseWeapon weapon)
            {
                return WoomyMathHelper.CalculateChargeInkCost(weapon.InkCost, weapon, true);
            }

            return 0f;
        }
    }
}
