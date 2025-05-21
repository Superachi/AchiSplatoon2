using AchiSplatoon2.Content.Items.Accessories.Palettes;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using AchiSplatoon2.Content.EnumsAndConstants;

namespace AchiSplatoon2.Content.Inventory
{
    internal class PaletteAccessorySlot : BaseAccessorySlot
    {
        public static LocalizedText PaletteText { get; private set; }

        public override string FunctionalTexture => TexturePaths.PaletteSlot;

        public override bool DrawVanitySlot => false;
        public override bool DrawDyeSlot => false;

        public override void SetupContent()
        {
            PaletteText = Mod.GetLocalization($"{nameof(PaletteAccessorySlot)}.Functional");
        }

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.ModItem is ChipPalette;

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => item.ModItem is ChipPalette;

        public override bool IsEnabled() => true;

        public override bool IsHidden() => false;

        public override void OnMouseHover(AccessorySlotType context)
        {
            // This will modify the hover text while an item is not in the slot
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                case AccessorySlotType.VanitySlot:
                    Main.hoverItemName = PaletteText.Value;
                    break;
            }
        }
    }
}
