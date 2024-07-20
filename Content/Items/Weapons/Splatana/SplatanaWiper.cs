using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Projectiles.SplatanaProjectiles;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Splatana
{
    internal class SplatanaWiper : BaseSplatana
    {
        public override int BaseDamage { get => 8; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = DisplayDamage(BaseDamage);
            Item.knockBack = 4;

            Item.useTime = 15;
            Item.useAnimation = Item.useTime;

            Item.width = 62;
            Item.height = 52;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
