using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaStamper : BaseSplatana
    {
        public override string ShootSample { get => "Splatana/StamperStrongSlash"; }
        public override string ShootWeakSample { get => "Splatana/StamperWeakSlash"; }
        public override string ChargeSample { get => "Splatana/StamperCharge"; }

        // Splatana specific
        public override int BaseDamage { get => 50; }
        public override float[] ChargeTimeThresholds { get => [26f]; }
        public override float WeakSlashShotSpeed { get => 10f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 5;

            Item.useTime = 22;
            Item.useAnimation = Item.useTime;

            Item.width = 64;
            Item.height = 64;

            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofMight);
    }
}
