using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class CarbonRoller : BaseRoller
    {
        public override float GroundWindUpDelayModifier => 0.4f;
        public override float JumpWindUpDelayModifier => 0.8f;
        public override float RollingSpeedModifier => 1.5f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 72;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<CarbonRollerSwingProjectile>();

            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
