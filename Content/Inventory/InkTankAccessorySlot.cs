using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.EnumsAndConstants;

namespace AchiSplatoon2.Content.Inventory
{
    internal class InkTankAccessorySlot : BaseAccessorySlot
    {
        public static LocalizedText InkTankText { get; private set; }

        public override string FunctionalTexture => TexturePaths.InkTankSlot;

        public override bool DrawVanitySlot => false;
        public override bool DrawDyeSlot => false;

        public override void SetupContent()
        {
            InkTankText = Mod.GetLocalization($"{nameof(InkTankAccessorySlot)}.Functional");
        }

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.ModItem is InkTank;

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => item.ModItem is InkTank;

        public override bool IsEnabled() => true;

        public override bool IsHidden() => false;

        public override void OnMouseHover(AccessorySlotType context)
        {
            // This will modify the hover text while an item is not in the slot
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                case AccessorySlotType.VanitySlot:
                    Main.hoverItemName = InkTankText.Value;
                    break;
            }
        }
    }
}
