using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class RangeBlasterProjectile : BlasterProjectile
    {
        private const int addedUpdate = 2;
        private const int explosionRadiusAir = 180;
        private const int explosionRadiusTile = 90;
        private const float explosionTime = 6f;
        private const float explosionDelay = 20f * addedUpdate;
    }
}