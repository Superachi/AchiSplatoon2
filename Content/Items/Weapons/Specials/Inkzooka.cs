using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class Inkzooka : BaseSpecial
    {
        public static readonly int MaxBursts = 5;
        protected override string UsageHintParamA => MaxBursts.ToString();
        public override float SpecialDrainPerTick => 0.2f;
        public override float SpecialDrainPerUse => 0f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 120;
            Item.width = 94;
            Item.height = 28;
            Item.knockBack = 8;
        }
    }
}
