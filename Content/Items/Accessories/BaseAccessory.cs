using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Helpers;
using Humanizer;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class BaseAccessory : BaseItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
        }
    }
}
