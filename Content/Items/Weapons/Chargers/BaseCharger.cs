using AchiSplatoon2.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class BaseCharger : BaseWeapon
    {
        public virtual float[] ChargeTimeThresholds { get => [60f]; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 5;
        }
    }
}
