using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaWiperDeco : SplatanaWiper
    {
        public override int BaseDamage { get => 20; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipePalladium();
    }
}
