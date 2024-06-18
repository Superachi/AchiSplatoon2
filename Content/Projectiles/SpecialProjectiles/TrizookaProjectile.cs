using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using Microsoft.Xna.Framework;
using Terraria.Graphics.CameraModifiers;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class TrizookaProjectile : BaseProjectile
    {
        protected override bool CountDamageForSpecialCharge { get => false; }

        private int state = 0;

        private const float explosionRadius = 200;
        private int finalExplosionRadius;
        private const float explosionTime = 6f;
        protected int damageBeforePiercing;

        private const float delayUntilFall = 12f;
        private const float fallSpeed = 0.3f;
        private const float terminalVelocity = 12f;

        private const float recoilAmount = 5f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        protected float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();

            TrizookaSpecial weaponData = (TrizookaSpecial)weaponSource;
            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
            damageBeforePiercing = Projectile.damage;

            // Randomize trajectory
            Projectile.velocity += Main.rand.NextVector2Circular(3, 3);
            Projectile.velocity *= Main.rand.NextFloat(0.8f, 1.2f);
        }

        private void EmitTrailInkDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f)
        {
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = GenerateInkColor();
                Dust.NewDustPerfect(Projectile.position, ModContent.DustType<BlasterTrailDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor, Main.rand.NextFloat(minScale, maxScale));
            }
        }

        protected void Explode()
        {
            Projectile.penetrate = -1;
            Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            EmitBurstDust(dustMaxVelocity: 25, amount: 20, minScale: 1.5f, maxScale: 3, radiusModifier: finalExplosionRadius);
            PlayAudio("BlasterExplosion", volume: 0.3f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.25f);
            AdvanceState();
        }

        private void AdvanceState()
        {
            state++;
            Timer = 0;
        }

        public override void AI()
        {
            switch (state)
            {
                case 0:
                    if (Timer >= delayUntilFall * FrameSpeed())
                    {
                        Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y + fallSpeed, -terminalVelocity, terminalVelocity);

                        if (Projectile.velocity.Y >= 0)
                        {
                            Projectile.velocity.X *= 0.98f;
                        }
                    }

                    EmitTrailInkDust(dustMaxVelocity: 0.2f, amount: 4, minScale: 1, maxScale: 3);

                    if (Timer > 300)
                    {
                        AdvanceState();
                    }
                    break;
                case 1:
                    if (Timer >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
            }

            Timer += 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (state == 0 && Projectile.penetrate <= 1) { Explode(); }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == 0) { Explode(); }
            return false;
        }
    }
}
