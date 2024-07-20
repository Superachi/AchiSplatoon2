using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stubble.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaMeleeProjectile : BaseProjectile
    {
        private Color bulletColor;
        private int startLifeTime;
        private int swingDirection;

        public bool wasFullyCharged;
        private bool firstHit = false;

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.timeLeft = 160;
            Projectile.extraUpdates = 12;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;

            bulletColor = GenerateInkColor();
            Projectile.velocity = Vector2.Zero;
            swingDirection = GetOwner().direction;
        }

        public override void AI()
        {
            Player p = GetOwner();

            float swingDirOffset = swingDirection == -1 ? 180 : 0;
            float deg = (Projectile.ai[1] + 30 + swingDirOffset) * swingDirection ;
            float rad = MathHelper.ToRadians(deg);
            float distanceFromPlayer = 64f;

            var oldPos = Projectile.position;
            Projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * distanceFromPlayer) - Projectile.width / 2;
            Projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * distanceFromPlayer) - Projectile.height / 2;

            Projectile.ai[1] += 1.1f;

            Color dustColor = GenerateInkColor();
            if (Main.rand.NextBool(10))
            {
                Dust.NewDustPerfect(
                Position: Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.5f, Projectile.height * 0.5f),
                Type: ModContent.DustType<SplatterBulletDust>(),
                Velocity: Vector2.Normalize(Projectile.position - oldPos),
                newColor: dustColor, Scale: 1.0f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            Projectile.knockBack += 5;
            modifiers.HitDirectionOverride = GetOwner().direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (!firstHit && wasFullyCharged)
            {
                firstHit = true;
                DirectHitDustBurst(target.Center);
            }
        }
    }
}
