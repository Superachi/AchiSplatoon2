using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.TacticoolerProjectiles
{
    internal class TacticoolerCan : BaseProjectile
    {
        private const int _stateSpawn = 0;
        private const int _stateFollow = 1;
        private const int _stateDespawn = 2;

        public Player? playerToFollow;

        private float _drawScale;

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = FrameSpeedMultiply(300);

            ProjectileID.Sets.TrailCacheLength[Type] = 14;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        protected override void AfterSpawn()
        {
            _drawScale = 0;

            Initialize(isDissolvable: false);
            SetState(_stateSpawn);
        }

        public override void AI()
        {
            if (playerToFollow == null || playerToFollow.dead)
            {
                Projectile.Kill();
                return;
            }

            if (_drawScale < 1f)
            {
                _drawScale += FrameSpeedDivide(0.25f);
            }

            switch (state)
            {
                case _stateSpawn:
                    if (timeSpentAlive > FrameSpeedDivide(30))
                    {
                        SetState(_stateFollow);
                    }

                    break;

                case _stateFollow:

                    var goalPosition = playerToFollow.Center;
                    if (playerToFollow.GetModPlayer<SquidPlayer>().IsFlat())
                    {
                        goalPosition.Y += 20;
                    }

                    Projectile.velocity += Projectile.DirectionTo(goalPosition) / 10;

                    if (Projectile.velocity.Length() > 2)
                    {
                        Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 2;
                    }

                    if (Projectile.Distance(goalPosition) < 20)
                    {
                        playerToFollow.AddBuff(ModContent.BuffType<TacticoolerBuff>(), TacticoolerBuff.BaseDuration);

                        playerToFollow.GetModPlayer<HudPlayer>().SetOverheadText("Refreshing!", 90);
                        PlayAudio(SoundPaths.TacticoolerPowerup.ToSoundStyle(), position: Projectile.Center);

                        for (int i = 0; i < 12; i++)
                        {
                            DustHelper.NewDust(
                                position: playerToFollow.Center,
                                velocity: Main.rand.NextVector2Circular(5, 5),
                                dustType: ModContent.DustType<SplatterBulletDust>(),
                                color: playerToFollow.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer(),
                                scale: 2,
                                data: new(gravity: -0.1f));
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            var d = Dust.NewDustPerfect(
                                Position: playerToFollow.Center,
                                Type: DustID.AncientLight,
                                Velocity: Main.rand.NextVector2CircularEdge(10, 10),
                                newColor: CurrentColor,
                                Scale: 2f);
                            d.noGravity = true;
                            d.noLight = true;
                        }

                        Projectile.Kill();
                    }
                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(Color.White, Projectile.rotation, _drawScale);

            return false;
        }
    }
}
