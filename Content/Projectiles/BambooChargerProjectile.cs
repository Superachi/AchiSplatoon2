using Terraria;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BambooChargerProjectile : SplatChargerProjectile
    {
        protected override bool ShakeScreenOnChargeShot { get => false; }
        protected override int MaxPenetrate { get => 1; }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Empty override. This is just to prevent the direct hit effect from spawning (since bamboo doesn't get this in the original game)
        }
    }
}