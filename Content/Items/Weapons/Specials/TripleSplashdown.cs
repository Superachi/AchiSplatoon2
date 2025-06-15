namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class TripleSplashdown : Splashdown
    {
        public override float RechargeCostPenalty { get => 120f; }
        public override int ExplosionRadius => 450;
        public override bool SummonFists => true;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 300;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 12;
        }
    }
}
