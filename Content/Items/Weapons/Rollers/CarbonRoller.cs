using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class CarbonRoller : BaseRoller
    {
        public override float GroundWindUpDelayModifier => 0.4f;
        public override float GroundAttackVelocityModifier => 0.9f;
        public override float JumpWindUpDelayModifier => 0.8f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.2f;
        public override float RollingAccelModifier => 3f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 32;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<CarbonSwingProjectile>();

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes() => AddRecipePostBee();
    }
}
