using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class PrincessPalette : ChipPalette
    {
        protected override int PaletteCapacity { get => 8; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 32;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }
    }
}
