using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Throwing;

internal class Torpedo : BaseBomb
{
    public override float InkCost { get => 60f; }
    public override int ExplosionRadius { get => 160; }
    public int PelletCount { get => 8; }
    public float PelletDamageMod { get => 0.4f; }

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
