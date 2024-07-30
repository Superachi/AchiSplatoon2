using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class DynamoRoller : BaseRoller
    {
        public override float GroundWindUpDelayModifier => 2.1f;
        public override float GroundAttackVelocityModifier => 1.5f;
        public override float JumpWindUpDelayModifier => 2.8f;
        public override float JumpAttackDamageModifier => 2f;
        public override float JumpAttackVelocityModifier => 2f;
        public override float RollingAccelModifier => 1f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 180;
            Item.knockBack = 6;
            Item.shoot = ModContent.ProjectileType<DynamoRollerSwingProjectile>();

            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
