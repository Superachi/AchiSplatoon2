using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
        private const float fallSpeed = 0.2f;
        private const float terminalVelocity = 24f;

        private const float recoilAmount = 5f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
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

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as TrizookaSpecial;

            finalExplosionRadius = (int)(explosionRadius * explosionRadiusModifier);
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            damageBeforePiercing = Projectile.damage;
        }

        private void EmitTrailInkDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust.NewDustPerfect(Projectile.position, ModContent.DustType<BlasterTrailDust>(),
                    Main.rand.NextVector2Circular(1, 1),
                    255, GenerateInkColor(), Main.rand.NextFloat(minScale, maxScale));
            }

            if (Main.rand.NextBool(5))
            {
                var d = Dust.NewDustPerfect(Projectile.position, DustID.AncientLight,
                    Main.rand.NextVector2Circular(5, 5),
                    255, GenerateInkColor(), Main.rand.NextFloat(minScale / 2, maxScale / 2));
                d.noGravity = true;
            }

            if (Main.rand.NextBool(15))
            {
                var d = Dust.NewDustPerfect(Projectile.position, DustID.FireworksRGB,
                    -Projectile.velocity / 2 + Main.rand.NextVector2Circular(3, 3),
                    255, GenerateInkColor(), Main.rand.NextFloat(minScale / 2, maxScale / 2));
                d.noGravity = true;
            }
        }

        protected void Explode()
        {
            Projectile.penetrate = -1;
            Projectile.Resize(finalExplosionRadius, finalExplosionRadius);
            Projectile.tileCollide = false;
            Projectile.position -= Projectile.velocity;
            Projectile.velocity = Vector2.Zero;

            var e = new ExplosionDustModel(_dustMaxVelocity: 25, _dustAmount: 30, _minScale: 1.5f, _maxScale: 3f, _radiusModifier: finalExplosionRadius);
            var s = new PlayAudioModel("BlasterExplosion", _volume: 0.4f, _pitchVariance: 0.3f, _maxInstances: 5, _pitch: -0.6f, _position: Projectile.Center);
            var p = CreateChildProjectile<ExplosionProjectileVisual>(
                position: Projectile.Center,
                velocity: Vector2.Zero,
                damage: 0);

            p.explosionDustModel = e;
            p.playAudioModel = s;
            p.colorOverride = colorOverride;

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

                    EmitTrailInkDust(dustMaxVelocity: 0.5f, amount: 4, minScale: 0.8f, maxScale: 3);

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
