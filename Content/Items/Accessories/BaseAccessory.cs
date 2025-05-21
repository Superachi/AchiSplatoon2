using AchiSplatoon2.Attributes;

namespace AchiSplatoon2.Content.Items.Accessories
{
    [ItemCategory("Accessory", "")]
    internal class BaseAccessory : BaseItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
        }
    }
}
