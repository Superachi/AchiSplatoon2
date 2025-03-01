using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Attributes;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    [OrderWeapon]
    internal class OrderSplatana : BaseSplatana
    {
        public override float InkCost { get => 1.5f; }

        public override SoundStyle ShootSample { get => SoundPaths.SplatanaStamperStrongSlash.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SplatanaStamperWeakSlash.ToSoundStyle(); }
        public override SoundStyle ChargeSample { get => SoundPaths.SplatanaStamperCharge.ToSoundStyle(); }

        // Splatana specific
        public override int BaseDamage { get => 8; }
        public override float[] ChargeTimeThresholds { get => [60f]; }
        public override float WeakSlashShotSpeed { get => 6f; }
        public override float MaxChargeMeleeDamageMod { get => 2f; }
        public override float MaxChargeLifetimeMod { get => 2f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.crit = 0;
            Item.knockBack = 3;

            Item.useTime = 26;
            Item.useAnimation = Item.useTime;

            Item.width = 64;
            Item.height = 64;

            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Emerald);
    }
}
