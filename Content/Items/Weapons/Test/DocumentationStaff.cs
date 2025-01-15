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
            //var assembly = Assembly.GetExecutingAssembly();
            //var lister = new ClassLister(assembly);
            //var types = lister.GetTypesInNamespace("AchiSplatoon2.Content.Items");

            //var itemData = new ItemData();
            //itemData.GetItemDataFromTypes(types);

            List<ItemData> itemDataList = new();

            var i = 0;
            while (i < 10000)
            {
                var modItem = ItemLoader.GetItem(i);

                if (modItem != null)
                {
                    var modName = ItemLoader.GetItem(i).Mod.Name;

                    if (modName == nameof(AchiSplatoon2))
                    {
                        var itemName = ItemLoader.GetItem(i).DisplayName;
                        var itemData = new ItemData(modItem);
                        itemDataList.Add(itemData);
                    }
                }

                i++;
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
