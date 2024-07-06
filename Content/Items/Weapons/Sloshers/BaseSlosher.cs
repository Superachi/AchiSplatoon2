using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.SlosherProjectiles;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Sloshers
{
    internal class BaseSlosher : BaseWeapon
    {
        public override string ShootSample { get => "SlosherShoot"; }
        public override string ShootWeakSample { get => "SlosherShootAlt"; }
        public virtual float ShotGravity { get => 0.12f; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<SlosherMainProjectile>(),
                singleShotTime: 30,
                shotVelocity: 8f
            );
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<InkAccessoryPlayer>();
                if (accMP.hasSteelCoil)
                {
                    damage *= AdamantiteCoil.DamageReductionMod;
                }
            }
        }
    }
}
