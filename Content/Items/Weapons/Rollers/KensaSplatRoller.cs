using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class KensaSplatRoller : SplatRoller
    {
        public override float GroundAttackVelocityModifier => 1.5f;
        public override float JumpAttackVelocityModifier => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 200;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<KensaSplatRollerSwingProjectile>();

            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
