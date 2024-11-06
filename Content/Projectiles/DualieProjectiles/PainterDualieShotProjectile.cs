using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class PainterDualieShotProjectile : DualieShotProjectile
    {
        public override void AfterSpawn()
        {
            base.AfterSpawn();
            var colorMP = GetOwner().GetModPlayer<ColorSettingPlayer>();
            colorOverride = colorMP.IncreaseHueBy(10);
        }
    }
}
