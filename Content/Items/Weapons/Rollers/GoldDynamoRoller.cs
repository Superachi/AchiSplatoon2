using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class GoldDynamoRoller : DynamoRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 160;
            Item.knockBack = 8;
            Item.shoot = ModContent.ProjectileType<GoldDynamoRollerSwingProjectile>();

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofFright);
    }
}
