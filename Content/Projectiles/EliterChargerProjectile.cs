using AchiSplatoon2.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class EliterChargerProjectile : SplatChargerProjectile
    {
        protected override float RequiredChargeTime { get => 75f; }
        protected override SoundStyle ShootSample { get => new SoundStyle("AchiSplatoon2/Content/Assets/Sounds/EliterChargerShoot"); }
    }
}