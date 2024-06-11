using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BlasterProjectile : BaseProjectile
    {
        private const int addedUpdate = 2;
        private const int explosionRadiusAir = 160;
        private const int explosionRadiusTile = 80;
        private const float explosionTime = 6f;
        private const float explosionDelay = 20f * addedUpdate;
        private int state = 0;

        public override void SetDefaults()
        {
            Initialize(color: InkColor.Pink, visible: false, visibleDelay: 24f);

            Projectile.extraUpdates = addedUpdate;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            var sample = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterShoot"); 
            var shootSound = sample with
            {
                Volume = 0.2f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(shootSound);
        }

        private void EmitRandomInkDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f)
        {
            for (int i = 0; i < amount; i++)
            {
                Color dustColor = ColorHelper.GenerateInkColor(inkColor);

                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SplatterBulletDust>(),
                    new Vector2(Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity), Main.rand.NextFloat(-dustMaxVelocity, dustMaxVelocity)),
                    255, dustColor, Main.rand.NextFloat(minScale, maxScale));
            }
        }

        private void EmitFireDust(int amount = 1)
        {
            var pos = Projectile.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
            var speedRand = 5;
            for (int i = 0; i < amount; i++)
            {
                Dust.NewDust(pos, Projectile.width, Projectile.height, DustID.Torch,
                    Main.rand.NextFloat(-speedRand, speedRand), Main.rand.NextFloat(-speedRand, speedRand));
            }
        }

        private void ExplodeBig()
        {
            // Audio
            var soundStyle = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterExplosion");
            var explosionSound = soundStyle with
            {
                Volume = 0.2f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(explosionSound);

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.tileCollide = false;
                Projectile.Resize(explosionRadiusAir, explosionRadiusAir);
                Projectile.velocity = Vector2.Zero;
            }

            // Visual
            EmitRandomInkDust(dustMaxVelocity: 5, amount: 30, minScale: 1, maxScale: 2);
            EmitFireDust(amount: 15);
        }

        private void ExplodeSmall()
        {
            // Audio
            var soundStyle = new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/BlasterExplosionLight");
            var explosionSound = soundStyle with
            {
                Volume = 0.1f,
                PitchVariance = 0.1f,
                MaxInstances = 3
            };
            SoundEngine.PlaySound(explosionSound);

            // Gameplay
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.damage /= 2;
                Projectile.tileCollide = false;
                Projectile.Resize(explosionRadiusTile, explosionRadiusTile);
                Projectile.velocity = Vector2.Zero;
            }

            // Visual
            EmitRandomInkDust(dustMaxVelocity: 5, amount: 15, minScale: 1, maxScale: 2);
            EmitFireDust(amount: 5);
        }

        private void AdvanceState()
        {
            state++;
            Projectile.ai[0] = 0;
        }

        private void SetState(int stateId)
        {
            state = stateId;
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            // Face direction
            Projectile.rotation = Projectile.velocity.ToRotation();

            switch (state)
            {
                case 0:
                    EmitRandomInkDust(dustMaxVelocity: 0.2f, amount: 2, minScale: 1, maxScale: 2);

                    if (Projectile.ai[0] >= explosionDelay)
                    {
                        ExplodeBig();
                        AdvanceState();
                    }
                    break;
                case 1:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
                case 2:
                    if (Projectile.ai[0] >= explosionTime)
                    {
                        Projectile.Kill();
                    }
                    break;
            }

            Projectile.ai[0] += 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, System.Int32 damageDone)
        {
            if (state == 0)
            {
                ExplodeBig();
                AdvanceState();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == 0)
            {
                ExplodeSmall();
                SetState(2);
            }
            return false;
        }
    }
}