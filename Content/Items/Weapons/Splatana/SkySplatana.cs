using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.GolemSplatanaProjectiles;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.SkySplatanaProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SkySplatana : GolemSplatana
    {
        public override float InkCost { get => 1f; }

        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundID.DD2_BetsyFireballShot; }
        public override SoundStyle ChargeSample { get => SoundID.Item34.WithPitchOffset(0); }

        // Splatana specific
        public override int BaseDamage { get => 10; }
        public override float[] ChargeTimeThresholds { get => [20f]; }
        public override float MaxChargeMeleeDamageMod { get => 2f; }
        public override float MaxChargeRangeDamageMod { get => 2f; }
        public override bool EnableWeakSlashProjectile => false;
        public override int StrongSlashProjectile { get => ModContent.ProjectileType<SkySplatanaStrongSlashProjectile>(); }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 4;

            Item.useTime = UseTime();
            Item.useAnimation = Item.useTime;

            Item.width = 56;
            Item.height = 56;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }

        public override int UseTime()
        {
            return 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SunplateBlock, 30)
                .AddIngredient(ItemID.Feather, 5)
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .Register();
        }
    }
}
