using AchiSplatoon2.Content.Items.Weapons.Shooters;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles
{
    internal class Dot52GalProjectile : SplattershotProjectile
    {
        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as Dot52Gal;
            Projectile.damage = weaponData.DamageOverride;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.DamageVariationScale *= 0;

            if (target.type != NPCID.DungeonGuardian)
            {
                modifiers.Defense *= 0;
            }
        }
    }
}
