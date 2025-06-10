using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables.LootBags
{
    [Autoload(true)]
    internal class CorruptLootBag : LargeMimicLootBag
    {
        protected override void OpenLootBag(Player player)
        {
            base.OpenLootBag(player);
        }
    }
}
