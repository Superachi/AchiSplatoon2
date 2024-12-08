using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneLaserProjectile : BaseProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 3;

            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            PlayShootSound();
            dissolvable = false;

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void AI()
        {
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            new OnHitEffect().ApplyEffect(this, target, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        protected override void CreateDustOnSpawn()
        {
        }

        protected override void CreateDustOnDespawn()
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Position: Projectile.position,
                    Type: DustID.RainbowTorch,
                    Width: 1,
                    Height: 1,
                    newColor: CurrentColor,
                    Scale: Main.rand.NextFloat(1.2f, 1.6f));
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }
        }

        protected override void PlayShootSound()
        {
            PlayAudio(SoundID.Item91, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: 1);
            PlayAudio(SoundID.Item115, volume: 0.1f, pitchVariance: 0f, maxInstances: 5, pitch: 1f);
            PlayAudio(SoundID.Item39, volume: 0.1f, pitchVariance: 0.2f, maxInstances: 10, pitch: 0f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRectangle = texture.Frame(Main.projFrames[Projectile.type], frameX: Projectile.frame); // The sourceRectangle says which frame to use.
            Vector2 origin = sourceRectangle.Size() * new Vector2(1, 0.5f);

            var len = Projectile.oldPos.Length - 1;
            for (int i = 0; i < len / 2; i++)
            {
                var posDiff = Projectile.position - Projectile.oldPos[i];
                var alphaMod = ((float)i / len) * 0.8f;

                DrawProjectile(ColorHelper.ColorWithAlpha255(CurrentColor), Projectile.oldRot[i], scale: 1.5f, alphaMod: alphaMod, considerWorldLight: false, positionOffset: posDiff);
            }

            return false;
        }
    }
}
