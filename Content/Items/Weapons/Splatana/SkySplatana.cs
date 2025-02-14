using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles.SkySplatanaProjectiles;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SkySplatana : GolemSplatana
    {
        public override float InkCost { get => 0.8f; }

        public override SoundStyle ShootSample { get => SoundPaths.Silence.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundID.Item1; }
        public override SoundStyle ChargeSample { get => SoundID.DD2_DarkMageAttack; }

        // Splatana specific
        public override int BaseDamage { get => 18; }
        public override float[] ChargeTimeThresholds { get => [24f]; }
        public override float MaxChargeMeleeDamageMod { get => 3f; }

        public override int MeleeEnergyProjectile { get => ModContent.ProjectileType<SkySplatanaMeleeEnergyProjectile>(); }
        public override bool EnableWeakSlashProjectile => false;
        public override bool EnableStrongSlashProjectile => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 4;
            Item.crit = 5;

            Item.useTime = 20;
            Item.useAnimation = Item.useTime;

            Item.width = 56;
            Item.height = 56;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
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
