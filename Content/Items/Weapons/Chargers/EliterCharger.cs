using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    internal class EliterCharger : SplatCharger
    {
        public override string ShootSample { get => "EliterChargerShoot"; }
        public override string ShootWeakSample { get => "EliterChargerShootWeak"; }
        public override float[] ChargeTimeThresholds { get => [75f]; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 92;
            Item.height = 32;
            Item.damage = 99;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes() => AddRecipePostSkeletron();

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 2);
        }
    }
}
