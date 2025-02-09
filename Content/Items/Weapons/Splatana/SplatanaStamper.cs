using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaStamper : BaseSplatana
    {
        public override float InkCost { get => 1f; }

        public override SoundStyle ShootSample { get => SoundPaths.SplatanaStamperStrongSlash.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.SplatanaStamperWeakSlash.ToSoundStyle(); }

        public override SoundStyle ChargeSample { get => SoundPaths.SplatanaStamperCharge.ToSoundStyle(); }

        // Splatana specific
        public override int BaseDamage { get => 40; }
        public override float[] ChargeTimeThresholds { get => [26f]; }
        public override float WeakSlashShotSpeed { get => 8f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 5;

            Item.useTime = 26;
            Item.useAnimation = Item.useTime;

            Item.width = 64;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofMight);
    }
}
