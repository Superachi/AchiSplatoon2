namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class TripleSplashdown : Splashdown
    {
        public override float RechargeCostPenalty { get => 100f; }
        public override int ExplosionRadius => 500;
        public override bool SummonFists => true;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 400;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 12;
        }
    }
}
