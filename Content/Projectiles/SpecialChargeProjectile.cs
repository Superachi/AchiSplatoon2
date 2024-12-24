using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Test;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SpecialChargeProjectile : BaseProjectile
    {
        public Player target = Main.LocalPlayer;
        public float chargeValue = 0;

        private float _turnWideness;
        private SpecialPlayer _specialPlayer = new SpecialPlayer();

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.extraUpdates = 12;
            Projectile.timeLeft = FrameSpeedMultiply(600);
            Projectile.tileCollide = false;
        }

        protected override void AfterSpawn()
        {
            _turnWideness = 200f;
            _specialPlayer = Owner.GetModPlayer<SpecialPlayer>();
            Projectile.velocity = -Projectile.DirectionTo(Owner.Center) * 5;
            Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, -30, 30);
            Initialize(isDissolvable: false);

            var soundStyle = Main.rand.NextBool(2) ? SoundPaths.SpecialChargeCreate1.ToSoundStyle() : SoundPaths.SpecialChargeCreate2.ToSoundStyle();
            PlayAudio(soundStyle, volume: 0.3f, pitchVariance: 0.3f, maxInstances: 5, position: Projectile.Center);
        }

        public override void AI()
        {
            var goalPosition = Owner.Center;
            if (Owner.GetModPlayer<SquidPlayer>().IsFlat())
            {
                goalPosition.Y += 20;
            }

            Projectile.velocity += Projectile.DirectionTo(goalPosition) / _turnWideness;

            if (_turnWideness > 1)
            {
                if (timeSpentAlive < FrameSpeedMultiply(30))
                {
                    _turnWideness -= 0.01f / FrameSpeed();
                }
                else
                {
                    _turnWideness -= 10f / FrameSpeed();
                }
            }

            if (Projectile.velocity.Length() > 2)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 2;
            }

            if (Projectile.Distance(goalPosition) < 10)
            {
                Collect();
            }

            if (timeSpentAlive > FrameSpeedMultiply(1))
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.AncientLight,
                    Velocity: Vector2.Zero,
                    newColor: CurrentColor,
                    Scale: 0.6f);
                d.noGravity = true;
                d.noLight = true;
            }
        }

        private void Collect()
        {
            // For testing
            if (Owner.HeldItem.ModItem is SpecialChargeGun) chargeValue = 5;

            _specialPlayer.IncrementSpecialCharge(chargeValue);

            PlayAudio(SoundPaths.SpecialChargeGain.ToSoundStyle(), volume: 0.05f, pitch: -1 + _specialPlayer.SpecialPercentage, maxInstances: 5);

            var position = Owner.Center;
            if (Owner.GetModPlayer<SquidPlayer>().IsFlat())
            {
                position.Y += 20;
            }

            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: position,
                    Type: DustID.AncientLight,
                    Velocity: Main.rand.NextVector2CircularEdge(10, 10),
                    newColor: CurrentColor,
                    Scale: 1.2f);
                d.noGravity = true;
                d.noLight = true;
            }

            Projectile.Kill();
        }
    }
}
