using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.DocGeneration;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Test
{
    [DeveloperContent]
    internal class DocumentationStaff : BaseWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<BaseProjectile>(),
                singleShotTime: 60,
                shotVelocity: 1);

            Item.damage = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DebugHelper.PrintWarning("Generating documentation data...");
            DebugHelper.PrintInfo("Selecting items to include in the documentation...");

            List<ItemData> itemDataList = new();

            var i = -1;
            while (i < 10000)
            {
                i++;

                var modItem = ItemLoader.GetItem(i);

                if (modItem != null)
                {
                    var modName = ItemLoader.GetItem(i).Mod.Name;

                    if (modName == nameof(AchiSplatoon2))
                    {
                        string? itemName = ItemLoader.GetItem(i).DisplayName.Value;

                        bool isBaseItem = itemName.ToLowerInvariant().Contains("base");
                        bool hasDevAttribute = Attribute.GetCustomAttribute(modItem.GetType(), typeof(DeveloperContentAttribute)) != null;

                        if (isBaseItem || hasDevAttribute)
                        {
                            DebugHelper.PrintDebug($"Skipping {itemName}");
                            continue;
                        }

                        var itemData = new ItemData(modItem);
                        itemDataList.Add(itemData);
                    }
                }
            }

            string detailFilePath = Path.Combine("D:\\Documents\\Repos\\WoomyModPages\\pages\\items");
            ItemData.ExportItemsAsDetailPages(itemDataList, detailFilePath);

            // Copy over item image files
            string modItemsDirectory = @"C:\Users\psclk\Documents\My Games\Terraria\tModLoader\ModSources\AchiSplatoon2\Content\Items\";
            string woomyItemImagesDirectory = @"D:\Documents\Repos\WoomyModPages\assets\images\Items";
            FileHandler.CopyDirectory(modItemsDirectory, woomyItemImagesDirectory, true, ".png", true);

            string categoryFilePath = Path.Combine("D:\\Documents\\Repos\\WoomyModPages\\pages\\item_categories");
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseSplattershot));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseBlaster));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseStringer));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseBrella));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseBrush));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseCharger));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseDualie));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseRoller));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseSlosher));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseSpecial));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseSplatana));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseSplatling));
            ItemData.ExportWeaponsAsCategoryPage(itemDataList, categoryFilePath, typeof(BaseBomb));

            stopwatch.Stop();

            DebugHelper.PrintInfo($"Finished generating docs (time: {stopwatch.ElapsedMilliseconds}ms)");

            return false;
        }
    }
}
