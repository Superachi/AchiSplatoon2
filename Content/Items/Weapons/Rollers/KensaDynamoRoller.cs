using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KensaDynamoRoller : DynamoRoller
    {
        public override float GroundAttackVelocityModifier => 2f;
        public override float JumpAttackVelocityModifier => 2.5f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 320;
            Item.knockBack = 8;
            Item.shoot = ModContent.ProjectileType<KensaDynamoRollerSwingProjectile>();

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
