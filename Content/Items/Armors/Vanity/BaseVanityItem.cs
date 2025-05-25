using AchiSplatoon2.ExtensionMethods;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.Vanity
{
    [Autoload(false)]
    internal class BaseVanityItem : BaseItem
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.vanity = true;
            Item.SetValueVanity();
        }
    }
}
