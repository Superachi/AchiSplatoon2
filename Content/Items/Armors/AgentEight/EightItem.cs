using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Armors.AgentEight
{
    [Autoload(false)]
    internal class EightItem : BaseItem
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.vanity = true;
            Item.SetValueVanity();
        }
    }
}
