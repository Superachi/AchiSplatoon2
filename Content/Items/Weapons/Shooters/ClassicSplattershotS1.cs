using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class ClassicSplattershotS1 : OctoShot
    {
        public override void AddRecipes()
        {
            Recipe recipe = CraftingReqs()
                .AddIngredient(ItemID.OrangeDye)
                .AddIngredient(ItemID.BlueDye)
                .Register();
        }
    }
}
