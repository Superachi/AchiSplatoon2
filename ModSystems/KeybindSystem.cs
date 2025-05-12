using Terraria.ModLoader;

namespace AchiSplatoon2.ModSystems
{
    internal class KeybindSystem : ModSystem
    {
        public static ModKeybind SwimFormKeybind { get; private set; }

        public override void Load()
        {
            // We localize keybinds by adding a Mods.{ModName}.Keybind.{KeybindName} entry to our localization files. The actual text displayed to English users is in en-US.hjson
            SwimFormKeybind = KeybindLoader.RegisterKeybind(Mod, "Enter Swim Form", "S");
        }

        public override void Unload()
        {
            SwimFormKeybind = null;
        }
    }
}
