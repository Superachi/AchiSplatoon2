using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Bows
{
    [OrderWeapon]
    internal class OrderStringer : TriStringer
    {
        public override SoundStyle ShootSample { get => SoundPaths.OrderStringerShoot.ToSoundStyle(); }
        public override SoundStyle ShootWeakSample { get => SoundPaths.OrderStringerShootWeak.ToSoundStyle(); }

        public override float ShotgunArc { get => 6f; }
        public override int ProjectileCount { get => 3; }
        public override bool AllowStickyProjectiles { get => false; }
        public override Vector2? HoldoutOffset() { return new Vector2(-4, 2); }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 74;
            Item.damage = 14;
            Item.knockBack = 2f;
            Item.crit = 0;
            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipeOrder(ItemID.Sapphire);
    }
}
