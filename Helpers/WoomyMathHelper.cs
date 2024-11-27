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

        public static Vector2 AddRotationToVector2(Vector2 inputVector, float degreesToRotateBy)
        {
            float inputVectorLength = inputVector.Length();
            float originalRadians = inputVector.ToRotation();
            float newRadians = originalRadians + MathHelper.ToRadians(degreesToRotateBy);

            Vector2 newVector = newRadians.ToRotationVector2() * inputVectorLength;
            return newVector;
        }

        public static Vector2 AddRotationToVector2(Vector2 inputVector, float degreesMin, float degreesMax)
        {
            var degrees = Main.rand.NextFloat(degreesMin, degreesMax);
            return AddRotationToVector2(inputVector, degrees);
        }

        public static Vector2 RoundVector2(Vector2 inputVector)
        {
            return new Vector2((int)inputVector.X, (int)inputVector.Y);
        }
    }
}
