using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using AchiSplatoon2.Content.Projectiles.SpecialProjectiles.TrizookaProjectiles;
using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria.ID;
using AchiSplatoon2.Content.Items.Weapons.Specials;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles.InkzookaProjectiles
{
    internal class InkzookaHeldProjectile : TrizookaHeldProjectile
    {
        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();

            shotsRemaining = Inkzooka.MaxBursts;
            shotDelay = 56;
            startDelay = 36;
        }

        public override void AI()
        {
            holdOffsetDefault = new Vector2(0, 0);
            base.AI();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = drawPosition - Main.screenPosition + holdOffset;
            Texture2D texture = TextureAssets.Item[itemIdentifier].Value;

            Rectangle sourceRectangle = texture.Frame(horizontalFrames: 1);
            Vector2 origin = sourceRectangle.Size() / 2f + new Vector2(-8 * drawDirection, 8);

            // The light value in the world
            var lightInWorld = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
            var finalColor = new Color(lightInWorld.R, lightInWorld.G, lightInWorld.G);

            SpriteBatch spriteBatch = Main.spriteBatch;

            Main.EntitySpriteDraw(
                texture,
                position,
                sourceRectangle,
                finalColor,
                Projectile.rotation + MathHelper.ToRadians(rotationOffset),
                origin,
                drawScale,
                drawDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        protected override void PlayShootSound()
        {
            var volumeMod = 1f;
            PlayAudio(SoundID.Item80, volume: 0.5f * volumeMod, position: Owner.Center);
            PlayAudio(SoundID.Splash, volume: 0.5f * volumeMod, pitch: 0.5f, position: Owner.Center);
            PlayAudio(SoundPaths.TrizookaLaunch.ToSoundStyle(), volume: 0.2f * volumeMod, position: Owner.Center);
            PlayAudio(SoundPaths.TrizookaLaunchWet.ToSoundStyle(), volume: 0.4f * volumeMod, position: Owner.Center);
        }

        protected override void CreateProjectiles(Vector2 shotPosition)
        {
            var p = CreateChildProjectile<InkzookaProjectile>(
                position: shotPosition,
                velocity: mouseDirection * 14,
                damage: Projectile.damage);

            p.colorOverride = CurrentColor;
        }
    }
}
