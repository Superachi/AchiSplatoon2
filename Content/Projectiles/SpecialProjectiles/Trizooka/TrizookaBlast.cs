using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.Trizooka
{
    internal class TrizookaBlast : BlastProjectile
    {
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.FinalDamage *= WoomyMathHelper.CalculateZookaDamageModifier(target);
        }
    }
}
