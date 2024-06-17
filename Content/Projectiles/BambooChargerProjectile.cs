namespace AchiSplatoon2.Content.Projectiles
{
    internal class BambooChargerProjectile : SplatChargerProjectile
    {
        protected override bool ShakeScreenOnChargeShot { get => false; }
        protected override int MaxPenetrate { get => 1; }
    }
}