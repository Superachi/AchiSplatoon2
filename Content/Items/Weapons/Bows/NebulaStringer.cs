using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class NebulaStringer : BaseStringer
    {
        public override Vector2? HoldoutOffset() { return new Vector2(0, 2); }
        public override Vector2 MuzzleOffset => new Vector2(30f, 0);

        public override float[] ChargeTimeThresholds { get => [45, 90f]; }
        public override float ShotgunArc { get => 20f; }
        public override int ProjectileCount { get => 5; }
        public override bool AllowStickyProjectiles { get => false; }

        public override int ProjectileType => ModContent.ProjectileType<NebulaStringerProjectile>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.shoot = ModContent.ProjectileType<NebulaStringerCharge>();
            Item.damage = 220;
            Item.width = 34;
            Item.height = 74;
            Item.knockBack = 3;
            Item.SetValueEndgame();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.LunarCraftingStation)
                .AddIngredient(ItemID.FragmentNebula, 18)
                .Register();
        }
    }
}
