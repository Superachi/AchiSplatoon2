using AchiSplatoon2.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class ItemTrackerPlayer : ModPlayer
    {
        public BaseWeapon lastUsedWeapon;
    }
}
