using AchiSplatoon2.Attributes;
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

            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "WoomyMod - Item data.md");

            ItemData.ExportListAsMarkdown(itemDataList, filePath);
            stopwatch.Stop();

            DebugHelper.PrintInfo($"Finished generating docs (time: {stopwatch.ElapsedMilliseconds}ms)");

            return false;
        }
    }
}
