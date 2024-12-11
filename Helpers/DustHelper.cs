using Terraria;
using AchiSplatoon2.Content.Dusts.CustomDataObjects;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Dusts;

namespace AchiSplatoon2.Helpers
{
    internal class DustHelper
    {
        public static Dust NewDust(Vector2 position, int dustType, Vector2? velocity = null, Color? color = null, float scale = 1f, BaseDustData? data = null)
        {
            var d = Dust.NewDustPerfect(
                position,
                dustType,
                velocity ?? Vector2.Zero,
                newColor: color ?? Color.White,
                Scale: scale);

            BaseDustData newData = data ?? new BaseDustData();
            d.customData = newData;

            if (newData.gravity == 0)
            {
                d.noGravity = true;
            }

            if (newData.emitLight)
            {
                d.noLight = false;
                d.noLightEmittence = false;
            }

            return d;
        }

        public static Dust NewSplatterBulletDust(Vector2 position, Vector2 velocity, Color color, float scale)
        {
            return NewDust(
                position: position,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: velocity,
                color: color,
                scale: scale,
                data: new(scaleIncrement: -0.25f));
        }

        public static Dust NewSplatterBulletDust(Vector2 position, Vector2 velocity, Color color, float minScale, float maxScale)
        {
            return NewDust(
                position: position,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: velocity,
                color: color,
                scale: Main.rand.NextFloat(minScale, maxScale));
        }

        public static Dust NewDropletDust(Vector2 position, Vector2 velocity, Color color, float scale)
        {
            return NewDust(
                position: position,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: velocity,
                color: color,
                scale: scale,
                data: new(scaleIncrement: -0.05f, gravity: 0.1f));
        }

        public static Dust NewDropletDust(Vector2 position, Vector2 velocity, Color color, float minScale, float maxScale)
        {
            return NewDropletDust(
                position: position,
                velocity: velocity,
                color: color,
                scale: Main.rand.NextFloat(minScale, maxScale));
        }

        public static Dust NewChargerBulletDust(Vector2 position, Vector2 velocity, Color color, float scale)
        {
            return NewDust(
                position: position,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: velocity,
                color: color,
                scale: scale,
                data: new(scaleIncrement: -0.08f));
        }

        public static Dust NewChargerBulletDust(Vector2 position, Vector2 velocity, Color color, float minScale, float maxScale)
        {
            return NewChargerBulletDust(
                position: position,
                velocity: velocity,
                color: color,
                scale: Main.rand.NextFloat(minScale, maxScale));
        }

        public static Dust NewLastingDust(Vector2 position, Vector2 velocity, Color color, float scale)
        {
            return NewDust(
                position: position,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: velocity,
                color: color,
                scale: scale,
                data: new(scaleIncrement: -0.03f, frictionMult: 0.9f));
        }

        public static Dust NewLastingDust(Vector2 position, Vector2 velocity, Color color, float minScale, float maxScale)
        {
            return NewLastingDust(
                position: position,
                velocity: velocity,
                color: color,
                scale: Main.rand.NextFloat(minScale, maxScale));
        }
    }
}
