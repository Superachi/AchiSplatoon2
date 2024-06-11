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
        protected override float RequiredChargeTime { get => 20f; }
        protected override string ShootSample { get => "BambooChargerShoot"; }
        protected override string ShootWeakSample { get => "BambooChargerShootWeak"; }
        protected override bool ShakeScreenOnChargeShot { get => false; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = 1;
        }
    }
}