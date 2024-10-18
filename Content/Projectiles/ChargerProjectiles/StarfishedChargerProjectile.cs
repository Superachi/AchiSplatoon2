using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class StarfishedChargerProjectile : SplatChargerProjectile
    {
        private int npcHits = 0;

        protected override void DustTrail()
        {
            var dust = Dust.NewDustPerfect(
                Position: Projectile.Center,
                Type: DustID.AncientLight,
                Velocity: Main.rand.NextVector2Circular(1, 1),
                newColor: GenerateInkColor(),
                Scale: Main.rand.NextFloat(0.6f, 1.2f));
            dust.noGravity = true;

            if (Main.rand.NextBool(8))
            {
                var dustB = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.AncientLight,
                    Velocity: Main.rand.NextVector2Circular(6, 6),
                    newColor: GenerateInkColor(),
                    Scale: Main.rand.NextFloat(0.8f, 1.6f));
                dustB.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (chargeLevel > 0 && target.life > 0 && npcHits < 5)
            {
                var p = CreateChildProjectile<StarfishedChargerBlast>(Projectile.Center, Vector2.Zero, Projectile.damage * 4, false);
                p.delayUntilBlast += npcHits * 5;
                p.pitchAdd += npcHits * 0.2f;
                p.npcTarget = target.whoAmI;
                p.AfterSpawn();

                npcHits++;
            }
        }
    }
}
