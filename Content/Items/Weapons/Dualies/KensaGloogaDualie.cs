using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class KensaGloogaDualie : GloogaDualie
    {
        public override string RollSample { get => "Dualies/GloogaDualieRoll"; }
        public override float PostRollDamageMod { get => 2f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 9,
                shotVelocity: 6f);

            Item.damage = 56;
            Item.width = 40;
            Item.height = 30;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
