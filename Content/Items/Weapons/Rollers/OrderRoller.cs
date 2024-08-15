using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class OrderRoller : BaseRoller
    {
        public override float GroundWindUpDelayModifier => 1.5f;
        public override float GroundAttackVelocityModifier => 0.9f;
        public override float JumpWindUpDelayModifier => 2f;
        public override float JumpAttackDamageModifier => 1.3f;
        public override float JumpAttackVelocityModifier => 1.2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 10;
            Item.knockBack = 2;
            Item.shoot = ModContent.ProjectileType<OrderSwingProjectile>();

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
