using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.BrushProjectiles;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.ModSystems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

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

        public static bool IsSwimFormHeld()
        {
            return KeybindSystem.SwimFormKeybind.Current;
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

        public static bool GetInputJumpReleased()
        {
            return PlayerInput.Triggers.JustReleased.Jump;
        }

        public static bool GetInputMouseLeftPressed()
        {
            return PlayerInput.Triggers.JustPressed.MouseLeft;
        }

        public static bool GetInputMouseLeftHold()
        {
            return PlayerInput.Triggers.Current.MouseLeft;
        }

        public static bool GetInputRightClicked()
        {
            return PlayerInput.Triggers.JustPressed.MouseRight;
        }

        public static bool GetInputMouseRightHold()
        {
            return PlayerInput.Triggers.Current.MouseRight;
        }

        public static bool GetInputDualieDodgePressed()
        {
            return KeybindSystem.DodgeRollKeybind.JustPressed;
        }

        public static bool GetInputCancelWeaponChargePressed()
        {
            return KeybindSystem.CancelWeaponChargeKeybind.JustPressed;
        }

        public static bool GetInputBrushDashHold()
        {
            return KeybindSystem.BrushDashKeybind.Current;
        }

        public static bool GetInputSubWeaponHold()
        {
            return KeybindSystem.SubWeaponKeybind.Current;
        }

        public static bool GetInputSpecialWeaponPressed()
        {
            return KeybindSystem.SpecialWeaponKeybind.JustPressed;
        }

        public static bool IsPlayerAllowedToUseItem(Player player)
        {
            var weaponPlayer = player.GetModPlayer<WeaponPlayer>();

            if (CursorHelper.CursorHasInteractable())
            {
                if (weaponPlayer.CustomWeaponCooldown < 30)
                {
                    player.GetModPlayer<WeaponPlayer>().CustomWeaponCooldown = 30;
                }
                return false;
            }

            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return false;
            if (!player.ItemTimeIsZero) return false;

            if (weaponPlayer.CustomWeaponCooldown > 0) return false;
            if (!weaponPlayer.allowSubWeaponUsage) return false;
            if (weaponPlayer.isBrushRolling || weaponPlayer.isBrushAttacking) return false;
            if (player.OwnsModProjectileWithType(ModContent.ProjectileType<BrushSwingProjectile>()))
            {
                return false;
            }

            if (player.GetModPlayer<SquidPlayer>().IsSquid()) return false;
            if (player.GetModPlayer<DualiePlayer>().isRolling || player.GetModPlayer<DualiePlayer>().postRollCooldown > 0) return false;

            if (PlayerHelper.IsPlayerImmobileViaDebuff(player)) return false;

            if (Main.hoverItemName != "") return false;

            return true;
        }
    }
}
