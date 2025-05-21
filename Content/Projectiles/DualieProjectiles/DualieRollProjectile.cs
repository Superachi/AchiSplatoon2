using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class DualieRollProjectile : BaseProjectile
    {
        public float rollDistance;
        public float rollDuration;
        protected virtual bool DisplayDefaultDusts => true;

        private Player owner;
        private DualiePlayer dualieMP;
        private int xDir = 0;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);

            owner = GetOwner();
            dualieMP = owner.GetModPlayer<DualiePlayer>();

            xDir = InputHelper.GetInputX();
            if (xDir != 0)
            {
                owner.velocity.X = xDir * rollDistance;
            }
            else
            {
                owner.velocity.X = -Math.Sign(owner.DirectionTo(Main.MouseWorld).X) * rollDistance;
            }

            PlayRollSound();
            DodgeRollDustBurst(Math.Sign(owner.velocity.X));

            if (IsThisClientTheProjectileOwner())
            {
                if (owner.HeldItem.ModItem is GrizzcoDualie)
                {
                    CreateChildProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<GrizzcoDualieBlastProjectile>(), 800, true);
                    PlayAudio(SoundID.Item14, volume: 0.5f, pitchVariance: 0.1f, maxInstances: 3, pitch: 0.5f);
                }
            }
        }

        public override void AI()
        {
            int fullRotate = 6;
            int rotateSpeed = 4;

            owner.velocity.Y = MathHelper.Max(owner.velocity.Y, 2);
            owner.fullRotation += xDir * rollDistance / rollDuration / fullRotate * rotateSpeed; // 0.3f;
            owner.fullRotationOrigin = new Vector2(10f, 20f);
            DodgeRollDustStream();

            if (Math.Abs(owner.fullRotation) >= fullRotate) Projectile.Kill();
            if (owner.velocity.X == 0) Projectile.Kill();
        }

        protected override void AfterKill(int timeLeft)
        {
            owner.fullRotation = 0;
            dualieMP.postRollCooldown = DualiePlayer.postRollCooldownDefault;
            dualieMP.DisplayRolls();
        }

        protected virtual void PlayRollSound()
        {
            var dualieData = WeaponInstance as BaseDualie;
            PlayAudio(dualieData.RollSample, pitchVariance: 0.1f);
            SoundHelper.PlayAudio(SoundID.Splash, volume: 0.5f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);
        }

        private void DodgeRollDustStream()
        {
            if (!DisplayDefaultDusts) return;

            for (int i = 0; i < 2; i++)
            {
                Rectangle rect = new Rectangle((int)owner.position.X, (int)owner.position.Y, owner.width, owner.height);

                Color color = owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer();

                DustHelper.NewChargerBulletDust(
                    position: Main.rand.NextVector2FromRectangle(rect),
                    velocity: new Vector2(owner.velocity.X / Main.rand.NextFloat(2, 6), 0),
                    color: color,
                    scale: 1f);
            }
        }

        private void DodgeRollDustBurst(int xDirection)
        {
            if (!DisplayDefaultDusts) return;

            for (int i = 0; i < 30; i++)
            {
                Rectangle rect = new Rectangle((int)owner.position.X, (int)owner.position.Y, owner.width, owner.height);

                Color color = owner.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer();
                DustHelper.NewDropletDust(
                    position: Main.rand.NextVector2FromRectangle(rect),
                    velocity: new Vector2(-xDirection * Main.rand.NextFloat(2, 8), Main.rand.NextFloat(0, -3)),
                    color: color,
                    minScale: 1f,
                    maxScale: 2f);
            }
        }
    }
}
