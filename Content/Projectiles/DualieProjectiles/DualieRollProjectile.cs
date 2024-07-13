using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Helpers;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Players;
using Terraria.GameInput;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class DualieRollProjectile : BaseProjectile
    {
        private Player owner;
        private InkDualiePlayer dualieMP;

        public float rollDistance;
        public float rollDuration;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
        }

        public override void ApplyWeaponInstanceData()
        {
            var dualieData = WeaponInstance as TestDualie;
            PlayAudio(dualieData.RollSample, pitchVariance: 0.1f);
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);

            owner = GetOwner();
            dualieMP = owner.GetModPlayer<InkDualiePlayer>();

            var xDir = PlayerInput.Triggers.Current.Right ? 1 : PlayerInput.Triggers.Current.Left ? -1 : 0;

            if (xDir != 0)
            {
                owner.velocity.X = xDir * rollDistance;
            } else
            {
                owner.velocity.X = -Math.Sign(owner.DirectionTo(Main.MouseWorld).X) * rollDistance;
            }
        }

        public override void AI()
        {
            int fullRotate = 6;
            int rotateSpeed = 4;

            owner.velocity.Y = MathHelper.Max(owner.velocity.Y, 10);
            owner.fullRotation += Math.Sign(owner.velocity.X) * rollDistance / rollDuration / fullRotate * rotateSpeed; // 0.3f;
            owner.fullRotationOrigin = new Vector2(10f, 20f);

            if (Math.Abs(owner.fullRotation) >= fullRotate) Projectile.Kill();
            if (owner.velocity.X == 0) Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            owner.fullRotation = 0;
            dualieMP.postRollCooldown = InkDualiePlayer.postRollCooldownDefault;
        }
    }
}
