using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KrakonSplatRoller : BaseRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 100;
            Item.knockBack = 5;
        }
    }
}
