using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AchiSplatoon2.Content.Players
{
    internal class InkItemPlayer : BaseModPlayer
    {
        public bool DoesPlayerCarryItem(int itemId)
        {
            for (int i = 0; i < 58; i ++)
            {
                if (Player.inventory[i].type == itemId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
