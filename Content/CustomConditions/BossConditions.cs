using Terraria;

namespace AchiSplatoon2.Content.CustomConditions
{
    internal class BossConditions
    {
        public static readonly Condition DownedEvilBoss = new("Conditions.DownedEvilBoss", () => Condition.DownedEaterOfWorlds.IsMet() || Condition.DownedBrainOfCthulhu.IsMet());

        public static readonly Condition NotDownedEvilBoss = new("Conditions.NotDownedEvilBoss", () => !DownedEvilBoss.IsMet());
    }
}
