namespace AchiSplatoon2.Content.Prefixes.ChargerPrefixes
{
    internal class BombasticPrefix : BaseChargerPrefix
    {
        public override bool LosePiercingModifier => true;
        public override bool ExplosiveModifier => true;
        public override float DamageModifier => 0.3f;
        public override float ChargeSpeedModifier => 0f;
    }
}
