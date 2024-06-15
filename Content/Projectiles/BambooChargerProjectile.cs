using AchiSplatoon2.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BambooChargerProjectile : SplatChargerProjectile
    {
        protected override bool ShakeScreenOnChargeShot { get => false; }
        protected override int MaxPenetrate { get => 1; }
    }
}