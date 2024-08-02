using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KrakonSplatRoller : SplatRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 110;
            Item.knockBack = 5;
            Item.shoot = ModContent.ProjectileType<KrakonSwingProjectile>();

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipeCobalt();
    }
}
