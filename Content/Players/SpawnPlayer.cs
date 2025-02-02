using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class SpawnPlayer : ModPlayer
    {
        // Useful info: https://www.youtube.com/watch?v=K3MpL3r2mjQ&list=PL2j68jF83kP1ffowiMIwbTndwLgmxGFpd&index=10
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            var starterItem = new Item();
            starterItem.SetDefaults(ModContent.ItemType<OrderBag>(), false);
            yield return starterItem;
        }

        public override void OnEnterWorld()
        {
            ChatHelper.SendModNoticeToThisClient($"Hi {Player.name}! Thanks for playing with my mod. Type {ColorHelper.TextWithFunctionalColor("/woomyWiki")} to visit the wiki!");
        }
    }
}
