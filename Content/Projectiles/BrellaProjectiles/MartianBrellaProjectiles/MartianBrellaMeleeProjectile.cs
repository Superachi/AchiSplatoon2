using Terraria;
using AchiSplatoon2.Helpers;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles.MartianBrellaProjectiles
{
    internal class MartianBrellaMeleeProjectile : BrellaMeleeProjectile
    {
        private int lightningCooldown = 0;

        public override void AI()
        {
            base.AI();
            if (lightningCooldown > 0)
            {
                lightningCooldown--;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (target.damage > 0)
            {
                CreateBigLightning(damageMod: 0.5f);
            }
        }

        protected override void BlockProjectileEffect(Projectile deflectedProjectile)
        {
            CreateBigLightning(damageMod: 1f);
        }

        private void CreateBigLightning(float damageMod = 1f)
        {
            if (lightningCooldown > 0) return;
            lightningCooldown = 20;

            var brellaItemData = GetOwner().HeldItem;
            int damage = (int)(brellaItemData.damage * damageMod);
            var velocity = GetOwner().DirectionTo(Main.MouseWorld) * 10;

            var p = CreateChildProjectile<MartianBrellaPelletProjectile>(Projectile.Center, velocity, damage, triggerAfterSpawn: false);
            p.isBigLightning = true;
            p.AfterSpawn();

            SoundHelper.PlayAudio(SoundID.Item72, 1f, pitchVariance: 0.2f, maxInstances: 5, pitch: -0.2f, position: Projectile.Center);
            SoundHelper.PlayAudio(SoundID.Item98, 0.3f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0f, position: Projectile.Center);
        }
    }
}
