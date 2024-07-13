using AchiSplatoon2.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using Terraria.DataStructures;
using AchiSplatoon2.Content.Players;
using System.Runtime.InteropServices.Marshalling;

namespace AchiSplatoon2.Content.Items.Weapons.Dualies
{
    internal class TestDualie : BaseWeapon
    {
        public virtual float ShotGravity { get => 0.1f; }
        public virtual int ShotGravityDelay { get => 0; }
        public virtual int ShotExtraUpdates { get => 4; }
        public override float AimDeviation { get => 1f; }
        public override string ShootSample { get => "SplattershotShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // Dualie specific
        public virtual string RollSample { get => "Dualies/SplatDualieRoll"; }
        public virtual float PostRollAttackSpeedMod { get => 0.9f; }
        public virtual int MaxRolls { get => 2; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<DualieShotProjectile>(),
                singleShotTime: 5,
                shotVelocity: 10f);

            Item.damage = 20;
            Item.width = 50;
            Item.height = 36;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Red;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            base.Shoot(player, source, position, velocity, type, damage, knockback);
            var dualieMP = player.GetModPlayer<InkDualiePlayer>();
            if (dualieMP.isTurret) player.itemTime = (int)(player.itemTime * PostRollAttackSpeedMod);
            return false;
        }
    }
}
