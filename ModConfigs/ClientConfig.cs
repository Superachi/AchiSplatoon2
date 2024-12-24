using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AchiSplatoon2.ModConfigs
{
    internal class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

         // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category. 
                          // [Label("$Some.Key")] // A label is the text displayed next to the option. This should usually be a short description of what it does. By default all ModConfig fields and properties have an automatic label translation key, but modders can specify a specific translation key.
                          // [Tooltip("$Some.Key")] // A tooltip is a description showed when you hover your mouse over the option. It can be used as a more in-depth explanation of the option. Like with Label, a specific key can be provided.
        
        [Header("UI")]

        [DefaultValue(false)]
        public bool HideInkTankPercentage;

        [Header("Audio")]

        [DefaultValue(false)]
        public bool UseOnlyVanillaSounds;

        [DefaultValue(false)]
        public bool SilentPearlDrone;

        [DefaultValue(false)]
        public bool DisableBombRushJingle;
    }
}
