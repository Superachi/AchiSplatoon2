using Terraria.ModLoader;

namespace AchiSplatoon2.ModSystems
{
    internal class KeybindSystem : ModSystem
    {
        public static ModKeybind SwimFormKeybind { get; private set; }
        public static ModKeybind SubWeaponKeybind { get; private set; }
        public static ModKeybind SpecialWeaponKeybind { get; private set; }
        public static ModKeybind DodgeRollKeybind { get; private set; }
        public static ModKeybind BrushDashKeybind { get; private set; }
        public static ModKeybind CancelWeaponChargeKeybind { get; private set; }

        public override void Load()
        {
            // We localize keybinds by adding a Mods.{ModName}.Keybind.{KeybindName} entry to our localization files. The actual text displayed to English users is in en-US.hjson
            SwimFormKeybind = KeybindLoader.RegisterKeybind(Mod, "Enter swim form", "S");
            SubWeaponKeybind = KeybindLoader.RegisterKeybind(Mod, "Use sub weapon", "Mouse2");
            SpecialWeaponKeybind = KeybindLoader.RegisterKeybind(Mod, "Use special weapon", "Mouse3");
            DodgeRollKeybind = KeybindLoader.RegisterKeybind(Mod, "Dualie dodge roll", "Mouse2");
            BrushDashKeybind = KeybindLoader.RegisterKeybind(Mod, "Brush dash", "Mouse2");
            CancelWeaponChargeKeybind = KeybindLoader.RegisterKeybind(Mod, "Cancel weapon charge", "Mouse2");
        }

        public override void Unload()
        {
            SwimFormKeybind = null;
            SubWeaponKeybind = null;
            SpecialWeaponKeybind = null;
            DodgeRollKeybind = null;
            BrushDashKeybind = null;
            CancelWeaponChargeKeybind = null;
        }
    }
}
