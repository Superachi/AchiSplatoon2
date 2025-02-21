using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing;

internal class Torpedo : BaseBomb
{
    public override int ExplosionRadius { get => 150; }
    public int PelletCount { get => 10; }
    public float PelletDamageMod { get => 0.25f; }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shoot = ModContent.ProjectileType<TorpedoProjectile>();
        Item.damage = 40;
        Item.knockBack = 6;
        Item.width = 26;
        Item.height = 26;

        Item.useTime = 36;
        Item.useAnimation = Item.useTime;
    }
}
