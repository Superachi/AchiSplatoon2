namespace AchiSplatoon2.Content.Items.Weapons.Throwing
{
    internal class StarterSplatBomb : SplatBomb
    {
        public override int ExplosionRadius { get => 200; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 40;
            Item.knockBack = 6;
        }
    }
}
