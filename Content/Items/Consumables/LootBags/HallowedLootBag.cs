using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables.LootBags
{
    [Autoload(true)]
    internal class HallowedLootBag : LargeMimicLootBag
    {
        protected override void OpenLootBag(Player player)
        {
            base.OpenLootBag(player);
        }
    }
}
