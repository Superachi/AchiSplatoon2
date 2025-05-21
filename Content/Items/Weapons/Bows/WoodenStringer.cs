using AchiSplatoon2.Content.EnumsAndConstants;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    internal class WoodenStringer : BaseStringer
    {
        public override float InkCost { get => 2f; }
        public override SoundStyle ShootSample { get => SoundPaths.ReefluxShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.ReefluxShootWeak.ToSoundStyle(); }

        public virtual float[] ChargeTimeThresholds { get => [25f, 50f]; }
        public override float ShotgunArc { get => 8f; }
        public override int ProjectileCount { get => 1; }
        public override bool AllowStickyProjectiles { get => false; }
        public override Vector2 MuzzleOffset => new Vector2(24f, 0);
        public override Vector2? HoldoutOffset() { return new Vector2(2, 2); }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 26;
            Item.height = 64;
            Item.damage = 20;
            Item.knockBack = 1f;
            Item.crit = 0;
            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes() => AddRecipeWood();
    }
}
