using AchiSplatoon2.ExtensionMethods;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters;

internal class AnnakiSplattershotNova : SplattershotNova
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.useTime = 6;
        Item.useAnimation = Item.useTime;

        Item.damage = 22;
        Item.knockBack = 3;
        Item.SetValueHighHardmodeOre();
    }

    public override void AddRecipes()
    {
        AddRecipeAdamantite();
        AddRecipeTitanium();
    }
}