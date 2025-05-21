using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Inventory
{
    internal class BaseAccessorySlot : ModAccessorySlot
    {
        public override bool IsEnabled()
        {
            return false;
        }

        public override bool IsHidden()
        {
            return true;
        }
    }
}
