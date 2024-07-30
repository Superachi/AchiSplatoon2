using AchiSplatoon2.Content.Projectiles.RollerProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class SplatRoller : BaseRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 20;
            Item.knockBack = 4;
        }
    }
}
