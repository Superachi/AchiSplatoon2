using AchiSplatoon2.Content.Items.Weapons.Chargers;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class BambooChargerProjectile : ChargerProjectile
    {
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            float targetLifePercent = target.life / (float)target.lifeMax;
            bool isBoss = target.boss;
            float threshold = isBoss ? BambooMk1Charger.LowLifeTapShotBossThreshold : BambooMk1Charger.LowLifeTapShotThreshold;
            float mult = isBoss ? BambooMk1Charger.LowLifeTapShotDamageBossMult : BambooMk1Charger.LowLifeTapShotDamageMult;

            if (!wasParentChargeMaxed && targetLifePercent <= threshold)
            {
                modifiers.FinalDamage *= mult;
                TripleHitDustBurst(target.Center, playSample: false);
            }
        }
    }
}
