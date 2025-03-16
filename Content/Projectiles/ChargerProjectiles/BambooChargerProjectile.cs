using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class BambooChargerProjectile : ChargerProjectile
    {
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            float targetLifePercent = target.life / (float)target.lifeMax;
            if (!wasParentChargeMaxed && targetLifePercent < BambooMk1Charger.LowLifeTapShotThreshold)
            {
                modifiers.FinalDamage *= target.boss ? BambooMk1Charger.LowLifeTapShotDamageBossMult : BambooMk1Charger.LowLifeTapShotDamageMult;
            }
        }
    }
}
