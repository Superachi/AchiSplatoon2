using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles.CorruptionBrellaProjectiles
{
    internal class CorruptionBrellaMeleeProjectile : BrellaMeleeProjectile
    {
        protected override void AfterSpawn()
        {
            colorOverride = Color.LimeGreen;
            base.AfterSpawn();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(BuffID.Venom, 120);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!target.friendly) return true;
            return false;
        }
    }
}
