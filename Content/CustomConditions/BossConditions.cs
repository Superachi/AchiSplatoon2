using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AchiSplatoon2.Content.CustomConditions
{
    internal class BossConditions
    {
        public static readonly Condition DownedEvilBoss = new("Conditions.DownedEvilBoss", () => Condition.DownedEaterOfWorlds.IsMet() || Condition.DownedBrainOfCthulhu.IsMet());
    }
}
