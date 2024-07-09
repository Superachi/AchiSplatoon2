using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Helpers
{
    internal class WoomyMathHelper
    {
        public static Vector2 DegreesToVector(float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            return radians.ToRotationVector2();
        }

        public static int FloatToPercentage(float value)
        {
            return (int)(value * 100f);
        }
    }
}
