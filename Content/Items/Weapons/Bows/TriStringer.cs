using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class TriStringer : BaseStringer
    {
        public override Vector2? HoldoutOffset() { return new Vector2(0, 2); }
        public override float MuzzleOffsetPx { get; set; } = 50f;
        public override float ShotgunArc { get => 8f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 24;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipePostEOC();
    }
}
