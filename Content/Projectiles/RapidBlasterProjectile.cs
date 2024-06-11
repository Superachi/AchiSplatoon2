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
    internal class RapidBlasterProjectile : BlasterProjectile
    {
        protected override int explosionRadiusAir { get => 160; }
        protected override int explosionRadiusTile { get => 120; }

    }
}