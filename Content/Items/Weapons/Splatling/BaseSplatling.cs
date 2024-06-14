using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class BaseSplatling : BaseWeapon
    {
        public virtual float[] ChargeTimeThresholds { get => [30f, 60f]; }
        public virtual float BarrageVelocity { get; set; } = 10f;
        public virtual int BarrageShotTime { get; set; } = 5;
        public virtual int BarrageMaxAmmo { get; set; } = 20;

        public override void SetDefaults()
        {
            Item.noMelee = true;
            Item.channel = true;
        }
    }
}
