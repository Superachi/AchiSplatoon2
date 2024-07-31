using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class CarbonRollerDeco : CarbonRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 95;
            Item.knockBack = 3;
            Item.shoot = ModContent.ProjectileType<CarbonDecoSwingProjectile>();

            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
