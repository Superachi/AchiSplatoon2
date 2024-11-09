using AchiSplatoon2.Content.Items.Weapons.Brellas;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles.MartianBrellaProjectiles
{
    internal class MartianBrellaPelletProjectile : BaseProjectile
    {
        private readonly float brightness = 0.002f;

        private float angle = 0;
        private float speed = 0;
        private bool hasBounced = false;

        private int lightningDirection = 0;
        private float lightningDirOffset = 20;
        private int lightningDustId = 180;
        private float lightningDustScale = 1f;

        public bool isBigLightning = false;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseBrella)WeaponInstance;

            shootSample = weaponData.ShootSample;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;
            Projectile.timeLeft *= FrameSpeed();
            Projectile.localNPCHitCooldown *= FrameSpeed();
        }

        protected override void AfterSpawn()
        {
            colorOverride = Color.Aqua;
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();

            angle = Projectile.velocity.ToRotation();
            speed = Projectile.velocity.Length() * Main.rand.NextFloat(0.9f, 1.1f);

            lightningDirection = Main.rand.NextBool(2) ? 1 : -1;
            lightningDirOffset *= Main.rand.NextFloat(0.9f, 1.1f);

            if (isBigLightning)
            {
                lightningDustId = Main.rand.NextFromList<int>(181, 112);
                lightningDustScale = 2f;

                Projectile.damage *= 5;
                Projectile.penetrate += 3;

                lightningDirOffset *= 1.5f;
                Projectile.extraUpdates += 2;
                Projectile.timeLeft *= 2;
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, CurrentColor.R * brightness, CurrentColor.G * brightness, CurrentColor.B * brightness);

            if (timeSpentAlive == 6 * FrameSpeed())
            {
                ChangeLightningAngle(0.5f);
            }

            if (timeSpentAlive > 30 && timeSpentAlive % 60 * FrameSpeed() == 0)
            {
                ChangeLightningAngle();
            }

            if (timeSpentAlive == 10)
            {
                var loopCount = 3;
                if (isBigLightning) loopCount *= 2;

                for (int i = 0; i < loopCount; i++)
                {
                    Color dustColor = GenerateInkColor();
                    Dust dust = Dust.NewDustDirect(Position: Projectile.position, Type: lightningDustId, Width: 1, Height: 1, newColor: dustColor, Scale: Main.rand.NextFloat(1.2f, 1.6f));
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity * 2 + Main.rand.NextVector2Circular(8f, 8f);
                }
            }

            if (timeSpentAlive > 10)
            {
                Color dustColor = GenerateInkColor();
                var pelletDust = Dust.NewDustDirect(Position: Projectile.position, Type: lightningDustId, Width: 1, Height: 1, newColor: dustColor, Scale: Main.rand.NextFloat(0.6f, 1f) * lightningDustScale);
                pelletDust.noGravity = true;
                pelletDust.velocity = Main.rand.NextVector2Circular(0.5f, 0.5f);
            }

            Projectile.velocity *= 0.95f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasBounced)
            {
                hasBounced = true;
                ProjectileBounce(oldVelocity, new Vector2(1f, 1f));
                angle = Projectile.velocity.ToRotation();

                BurstDust(10);
                return false;
            }

            return true;
        }

        protected override void AfterKill(int timeLeft)
        {
            if (timeLeft == 0)
            {
                BurstDust(5);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            if (isBigLightning)
            {
                modifiers.SetCrit();
            }
        }

        private void ChangeLightningAngle(float angleChangeMod = 1f)
        {
            angle += MathHelper.ToRadians(lightningDirOffset * lightningDirection) * angleChangeMod;
            Projectile.velocity = angle.ToRotationVector2() * speed;
            lightningDirection *= -1;
            lightningDirOffset *= 1.1f;
        }

        private void BurstDust(int amount)
        {
            for (int i = 0; i < 10; i++)
            {
                Color dustColor = GenerateInkColor();
                Dust dust = Dust.NewDustDirect(Position: Projectile.position, Type: lightningDustId, Width: 1, Height: 1, newColor: dustColor, Scale: Main.rand.NextFloat(1.2f, 1.6f));
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
            }
        }
    }
}
