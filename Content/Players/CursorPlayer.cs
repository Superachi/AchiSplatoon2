using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class CursorPlayer : ModPlayer
    {
        public bool cursorHoveredLastFrame = false;

        public override void PostUpdate()
        {
            cursorHoveredLastFrame = Player.cursorItemIconEnabled;
        }
    }
}
