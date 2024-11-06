using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players;

internal class InventoryPlayer : ModPlayer
{
    protected int heldType = 0;
    protected int oldHeldType = 0;

    public override void PreUpdate()
    {
        UpdateOldHeldTypeToMatchNew();
        UpdateCurrentHeldType();
    }

    public ModItem HeldModItem()
    {
        return Player.HeldItem.ModItem;
    }

    public bool HasHeldItemChanged() => CheckIfHeldItemChanged();

    private bool CheckIfHeldItemChanged()
    {
        return oldHeldType != heldType;
    }

    private void UpdateOldHeldTypeToMatchNew()
    {
        oldHeldType = heldType;
    }

    private void UpdateCurrentHeldType()
    {
        heldType = Player.HeldItem.type;
    }
}
