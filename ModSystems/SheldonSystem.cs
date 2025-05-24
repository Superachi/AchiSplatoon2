using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.ModSystems
{
    public class SheldonSystem : ModSystem
    {
        public static int HintCycle => _hintCycle;
        private static int _hintCycle = 0;

        public static void IncrementHintCycle()
        {
            _hintCycle++;
        }

        public override void Load()
        {
            _hintCycle = Main.rand.Next(10);
        }
    }
}
