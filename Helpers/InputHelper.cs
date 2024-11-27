using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace AchiSplatoon2.Helpers
{
    internal class InputHelper
    {
        public static Vector2 GetInputVector()
        {
            // Source: tModLoader discord -> https://discord.com/channels/103110554649894912/534215632795729922/1239780417066762260
            Vector2 result = Vector2.Zero;

            result.X = GetInputX();
            result.Y = GetInputY();
            result.Normalize();

            return result;
        }

        public static bool IsAnyKeyPressed()
        {
            return PlayerInput.GetPressedKeys().Count > 0;
        }

        public static int GetInputX()
        {
            return PlayerInput.Triggers.Current.Right ? 1 : PlayerInput.Triggers.Current.Left ? -1 : 0;
        }

        public static int GetInputY()
        {
            return PlayerInput.Triggers.Current.Up ? 1 : PlayerInput.Triggers.Current.Down ? -1 : 0;
        }

        public static int GetInputXPressed()
        {
            return PlayerInput.Triggers.JustPressed.Right ? 1 : PlayerInput.Triggers.JustPressed.Left ? -1 : 0;
        }

        public static bool GetInputJump()
        {
            return PlayerInput.Triggers.Current.Jump;
        }

        public static bool GetInputJumpPressed()
        {
            return PlayerInput.Triggers.JustPressed.Jump;
        }

        public static bool GetInputMouseLeftHold()
        {
            return PlayerInput.Triggers.Current.MouseLeft;
        }

        public static bool GetInputRightClicked()
        {
            return PlayerInput.Triggers.JustPressed.MouseRight;
        }
    }
}
