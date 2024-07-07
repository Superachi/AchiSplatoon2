using AchiSplatoon2.Content.Items.Weapons.Throwing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class BaseBombProjectile : BaseProjectile
    {
        protected override bool FallThroughPlatforms => false;
        protected override bool EnablePierceDamageFalloff { get => false; }
        protected bool hasExploded = false;
        protected int explosionRadius;
        protected int finalExplosionRadius;

        protected float airFriction = 0.995f;
        protected float terminalVelocity = 10f;

        protected Color glowColor;
        protected float brightness = 0.002f;
        protected float drawScale = 1f;

        public override void AfterSpawn()
        {
            Initialize();
            PlayAudio("Throwables/SplatBombThrow");
            glowColor = GenerateInkColor();

            BaseBomb weaponData = (BaseBomb)weaponSource;
            explosionRadius = weaponData.ExplosionRadius;
            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    modifiers.FinalDamage /= 3;
                }
            }
        }

        protected void Detonate()
        {
            Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            EmitBurstDust(dustMaxVelocity: 25, amount: 20, minScale: 1.5f, maxScale: 3, radiusModifier: finalExplosionRadius);
            StopAudio("Throwables/SplatBombFuse");
            PlayAudio("Throwables/SplatBombDetonate", volume: 0.6f, pitchVariance: 0.2f, maxInstances: 5);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!hasExploded)
            {
                DrawProjectile(glowColor, Projectile.rotation, drawScale, false);
                return false;
            }
            return true;
        }
    }
}
