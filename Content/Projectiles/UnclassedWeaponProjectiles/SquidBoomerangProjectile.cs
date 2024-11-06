using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles
{
    internal class SquidBoomerangProjectile : BaseProjectile
    {
        private float returnSpeed = 0f;

        private const int stateFlyOut = 0;
        private const int stateReturn = 1;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;

            DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -20;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        protected override void AfterSpawn()
        {
            Initialize();

            Projectile.rotation += MathHelper.ToRadians(Main.rand.Next(360));
            Projectile.scale = 0.5f;

            SoundHelper.PlayAudio(SoundID.DD2_MonkStaffSwing, volume: 0.5f, pitch: 0.3f);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            switch (state)
            {
                case stateFlyOut:
                    break;

                case stateReturn:
                    Projectile.tileCollide = false;
                    break;
            }
        }

        public override void AI()
        {
            if (Projectile.scale < 1)
            {
                Projectile.scale += 0.05f;
            }

            switch (state)
            {
                case stateFlyOut:
                    Projectile.velocity *= 0.995f;

                    if (timeSpentInState > 60)
                    {
                        AdvanceState();
                        return;
                    }

                    break;

                case stateReturn:
                    if (returnSpeed < 10) returnSpeed += 0.1f;

                    Projectile.velocity *= 0.95f;
                    Projectile.position += Projectile.DirectionTo(GetOwner().Center) * returnSpeed;

                    if (Projectile.position.Distance(GetOwner().Center) < 32)
                    {
                        Projectile.Kill();
                        return;
                    }

                    break;
            }

            Projectile.rotation += MathHelper.ToRadians(-9);
            DustStream();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (state == stateFlyOut)
            {
                AdvanceState();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (state == stateFlyOut)
            {
                DustBurst();
                ProjectileBounce(oldVelocity, new Vector2(0.9f, 0.9f));
                AdvanceState();

                int soundId = Main.rand.Next(8);
                SoundHelper.PlayAudio($"Voice/InklingGirl/Voice_SquidGirl_Damage_0{soundId}", volume: 0.5f, pitchVariance: 0.2f);
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }

        private void DustStream()
        {
            if (Main.rand.NextBool(4)) return;

            Dust.NewDustPerfect(
                Position: Projectile.Center + new Vector2(0, -5) + Main.rand.NextVector2Circular(30, 30),
                Type: ModContent.DustType<ChargerBulletDust>(),
                Velocity: Projectile.velocity / 5,
                newColor: Color.OrangeRed,
                Scale: 1.2f);
        }

        private void DustBurst()
        {
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustPerfect(
                    Position: Projectile.Center + new Vector2(0, -5),
                    Type: ModContent.DustType<SplatterBulletLastingDust>(),
                    Velocity: Main.rand.NextVector2CircularEdge(5, 5),
                    newColor: Color.OrangeRed,
                    Scale: 0.8f);
                dust.velocity *= Main.rand.NextFloat(0.8f, 1.2f);
            }
        }
    }
}
