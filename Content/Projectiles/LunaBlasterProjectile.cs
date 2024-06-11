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
    internal class LunaBlasterProjectile : BlasterProjectile
    {
        protected override int ExplosionRadiusAir { get => 280; }
        protected override int ExplosionRadiusTile { get => 150; }
        protected override float ExplosionDelayInit { get => 12f; }
    }
}