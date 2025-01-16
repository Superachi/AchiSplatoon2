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

        public string GenerateMarkdown(string path)
        {
            if (Name.Contains("Base") || Name.Contains("Test"))
            {
                return "";
            }

            string markdown = "";

            // Name and description display
            markdown += $"\n<a id=\"{Name.ToLowerInvariant().Replace(" ", "-")}\"></a>\n";
            markdown += $"### {Name}\n\n";

            if (!string.IsNullOrEmpty(Flavor))
            {
                markdown += $"*{FormatInput(Flavor)}*\n\n";
            }

            if (!string.IsNullOrEmpty(Tooltip))
            {
                markdown += $"{FormatInput(Tooltip)}\n\n";
            }

            if (!string.IsNullOrEmpty(UsageHint))
            {
                markdown += $"**{FormatInput(UsageHint)}**\n";
            }

            markdown += "<table>";

            AddMarkdownTableRow(ref markdown, nameof(Category), Category);

            // Weapon stats display

            if (ModdedItem is BaseWeapon weapon)
            {
                if (Damage > 0)
                {
                    AddMarkdownTableRow(ref markdown, nameof(Damage), Damage.ToString());
                }
                else
                {
                    AddMarkdownTableRow(ref markdown, nameof(Damage), "No damage");
                }

                if (Knockback > 0)
                {
                    AddMarkdownTableRow(ref markdown, nameof(Knockback), Knockback.ToString());
                }
                else
                {
                    AddMarkdownTableRow(ref markdown, nameof(Knockback), "No knockback");
                }

                if (Crit > 0)
                {
                    AddMarkdownTableRow(ref markdown, "Bonus crit chance", Crit.ToString() + "%");
                }

                if (Velocity > 1)
                {
                    AddMarkdownTableRow(ref markdown, nameof(Velocity), Velocity.ToString());
                }

                var inkCost = WoomyMathHelper.CalculateChargeInkCost(weapon.InkCost, weapon, true);
                if (inkCost > 0)
                {
                    AddMarkdownTableRow(ref markdown, "Ink usage", inkCost.ToString() + "%");
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
                AddMarkdownTableRow(ref markdown, nameof(Value), valueString);
            }

            AddMarkdownTableRow(ref markdown, nameof(Rarity), RarityToString(Rarity));

            markdown += "\r\n</table>\r\n\n\n";

            // Recipes

            if (Recipes.Count > 0)
            {

                if (Recipes.Count > 1)
                {
                    var i = 0;

                    foreach (var recipe in Recipes)
                    {
                        i++;
                        markdown += $"<details>\r\n  <summary><b>Recipe {i}</b></summary>\r\n";

                        markdown += "<span>\n\n";
                        foreach (var item in recipe.requiredItem)
                        {
                            var link = GenerateVanillaItemLink(item);
                            link = link == item.Name ? GenerateModItemJumpLink(item) : link;

                            markdown += $"- {item.stack}x {link} \n";
                        }
                        markdown += "\n</span>";
                        markdown += "\r\n</details>\r\n";
                    }

                    markdown += "\n";
                }
                else
                {
                    markdown += $"<b>Recipe</b>\r\n";

                    foreach (var item in Recipes[0].requiredItem)
                    {
                        var link = GenerateVanillaItemLink(item);
                        link = link == item.Name ? GenerateModItemJumpLink(item) : link;

                        markdown += $"- {item.stack}x {link} \n";
                    }
                }
            }

            markdown += "---\n";

            return markdown;
        }

        public static void ExportListAsMarkdown(List<ItemData> itemDataList, string path)
        {
            DebugHelper.PrintDebug("Creating item data Markdown...");

            var markdown = "*NOTE: Most, if not all content below has been automatically generated.*\n\n";

            markdown += GenerateTableOfContent(itemDataList);

            foreach (var itemData in itemDataList)
            {
                markdown += itemData.GenerateMarkdown(path);
            }

            File.WriteAllText(path, markdown);

            DebugHelper.PrintDebug("Created the Markdown.");
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

        private string RarityToString(int rarity)
        {
            switch (rarity)
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
    }
}
