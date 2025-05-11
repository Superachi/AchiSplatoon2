using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.StaticData.LootTables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

                if (recipe.TryGetResult(ModdedItem.Item.type, out Item _))
                {
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

        private string? GetImageUri(ModItem moddedItem)
        {
            var className = moddedItem.GetType().Name;

            var categoryAttribute = Attribute.GetCustomAttribute(moddedItem.GetType(), typeof(ItemCategoryAttribute)) as ItemCategoryAttribute;
            if (categoryAttribute == null)
            {
                DebugHelper.PrintError($"Failed to generate Image URI string: {nameof(categoryAttribute)} was null for {moddedItem.Name}");
                return null;
            }

            return $"{categoryAttribute.DirectorySuffix}/{className}";
        }

        public string GenerateWeaponDetailHTML()
        {
            if (!(ModdedItem is BaseWeapon weapon))
            {
                DebugHelper.PrintError($"Tried to generate weapon detail HTML for {Name}, but it doesn't inherit from {nameof(BaseWeapon)}");
                return "";
            }

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

            string? imageUri = GetImageUri(ModdedItem);
            if (imageUri == null)
            {
                return "";
            }

            string html = "";
            html += "---\r\n---\r\n" +
                "\r\n{%" +
                "\r\n  include weapon-details-page/stats.html" +
                $"\r\n  name=\"{Name}\"" +
                $"\r\n  image_uri=\"{imageUri}\"" +
                $"\r\n  category=\"{Category}\"" +
                $"\r\n  flavor_text=\"{FormatInput(Flavor)}\"" +
                $"\r\n  usage_hint=\"{FormatInput(UsageHint)}\"" +
                $"\r\n  damage=\"{Damage}\"" +
                $"\r\n  knockback=\"{Knockback}\"" +
                $"\r\n  velocity=\"{Velocity}\"" +
                $"\r\n  ink_usage=\"{GetWeaponInkCost(ModdedItem)}\"" +
                $"\r\n  value=\"{valueString}\"" +
                $"\r\n  rarity=\"{RarityToString()}\"" +
                "\r\n%}";

            // Recipes
            var recipeHtml = "";
            if (Recipes.Count > 0)
            {
                var i = 0;

                foreach (var recipe in Recipes)
                {
                    i++;

                    var tileName = "None required";

                    var tileList = recipe.requiredTile;
                    if (tileList.Count > 0)
                    {
                        var tile = recipe.requiredTile.First();
                        bool _ = TileID.Search.TryGetName(tile, out tileName);
                    }

                    recipeHtml += "      {%\r\n        include weapon-details-page/recipe-header " +
                        $"\r\n        recipe_number=\"{i}\"" +
                        $"\r\n        tile_name=\"{tileName}\"" +
                        "\r\n      %}\r\n      ";

                    foreach (var item in recipe.requiredItem)
                    {
                        var link = GetVanillaItemUrl(item);
                        link = link == "" ? GetModItemPageUrl(item) : link;

                        recipeHtml += "{%\r\n        include weapon-details-page/recipe-ingredient " +
                            $"\r\n        amount=\"{item.stack}\"" +
                            $"\r\n        item_name=\"{item.Name}\"" +
                            $"\r\n        item_url=\"{link}\"" +
                            "\r\n      %}\r\n      ";
                    }

                    recipeHtml += "<br>\r\n\r\n";
                }
            }
            else
            {
                recipeHtml = "      ❌<i> There are no known crafting recipes.</i>";
            }

            // Droptable
            var lootTableHtml = "";
            var lootTableIndices = LootTables.FindIndicesById(ModdedItem.Item.type);

            if (lootTableIndices.Count() > 0)
            {
                foreach(var index in lootTableIndices)
                {
                    var npcName = Lang.GetNPCNameValue(index.NpcId);
                    var bagName = Lang.GetItemNameValue(index.TreasureBagId);
                    var sourceName = "";
                    var sourceUrl = "";

                    if (npcName != null)
                    {
                        sourceName = $"{npcName}";
                        sourceUrl = GetVanillaNPCUrl(index.NpcId);
                    }
                    else if (bagName != null)
                    {
                        sourceName = $"{bagName}";
                        sourceUrl = GetVanillaItemUrl(ModdedItem.Item) ?? GetModItemPageUrl(ModdedItem.Item);
                    }

                    string amount = index.MinimumDropped.ToString();
                    if (index.MinimumDropped != index.MaximumDropped)
                    {
                        amount = $"{index.MinimumDropped} - {index.MaximumDropped}";
                    }

                    string chance = (1f / index.ChanceDenominator * 100).ToString("0.0");

                    lootTableHtml += "      {%\r\n        include weapon-details-page/drop-table-index.html" +
                    $"\r\n        source_name=\"{sourceName}\"" +
                    $"\r\n        source_url=\"{sourceUrl}\"" +
                    $"\r\n        amount=\"{amount}\"" +
                    $"\r\n        chance=\"{chance}\"" +
                    "\r\n      %}\r\n      ";
                }
            }
            else
            {
                lootTableHtml = "❌<i> There are no known drop locations.</i>";
            }

            html += "\r\n\r\n<br>\r\n<h1 style=\"text-align: center;\">How to obtain</h1>\r\n<div class=\"card-group\">";

            // Crafting section
            html += "\r\n  <div class=\"card\">\r\n    ";
            html += "<div class=\"card-header text-center\">\r\n      <b>Crafting</b>\r\n    </div>\r\n\r\n    <div class=\"card-body\">\r\n" + $"{recipeHtml}" + "\r\n    </div>\r\n  </div>";

            // Loot section
            html += "\r\n\r\n  <div class=\"card\">\r\n    ";
            html += "<div class=\"card-header text-center\">\r\n      <b>Looting from enemy/treasure bag</b>\r\n    </div>\r\n\r\n    <div class=\"card-body\">\r\n      " + $"{lootTableHtml}" + "\r\n    </div>\r\n  </div>";

            html += "\r\n</div>";
            return html;
        }

        public static void ExportItemsAsDetailPages(List<ItemData> itemDataList, string path)
        {
            DebugHelper.PrintInfo("Creating item detail pages...");

            foreach (var itemData in itemDataList)
            {
                if (itemData.ModdedItem is not BaseWeapon)
                {
                    DebugHelper.PrintDebug($"Skipping {itemData.Name}: it doesn't inherit from {nameof(BaseWeapon)}");
                    continue;
                }

                var html = itemData.GenerateWeaponDetailHTML();
                var className = itemData.ModdedItem.GetType().Name;
                var newPath = Path.Combine(path, className + ".html");
                File.WriteAllText(newPath, html);
            }

            DebugHelper.PrintInfo("Created the HTML.");
        }

        public static void ExportWeaponsAsCategoryPage(List<ItemData> itemDataList, string path, Type baseItemType)
        {
            DebugHelper.PrintInfo("Creating item category pages...");

            List<ItemData> filteredList = new();
            foreach (var itemData in itemDataList)
            {
                if (!itemData.ModdedItem.GetType().IsSubclassOf(baseItemType))
                {
                    continue;
                }

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

            var templateStart = "---\r\nsubtitle: " + $"{baseTypeCategoryAttribute.PluralizeCategory()}" + "\r\nlayout: category-page\r\n---\r\n\r\n<div class=\"card-header\">\r\n  <div class=\"col\"><b>" + $"{baseTypeCategoryAttribute.PluralizeCategory()}" + "</b></div>\r\n</div>\r\n\r\n<div class=\"card-body\">\r\n  <details open>\r\n    <summary><b>Show/hide</b></summary>\r\n    <table class=\"table\">\r\n      {% include category-page/category-table-head.html %}\r\n      <tbody>\r\n";
            var templateEnd = "      </tbody>\r\n    </table>\r\n  </details>\r\n</div>";
            var html = templateStart + includeLines + templateEnd;

            var newPath = Path.Combine(path, baseTypeCategoryAttribute.DirectorySuffix + ".html");
            File.WriteAllText(newPath, html);

            DebugHelper.PrintInfo("Created the HTML.");
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

        private string GetVanillaNPCUrl(int npcId)
        {
            var npcName = Lang.GetNPCNameValue(npcId);
            if (npcName != null)
            {
                return $"https://terraria.wiki.gg/wiki/{npcName.Replace(" ", "_")}";
            }

            return "";
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

            if (!string.IsNullOrEmpty(input) && !input.EndsWith(".") && !input.EndsWith("!") && !input.EndsWith("?"))
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
