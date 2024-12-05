using AchiSplatoon2.Content.EnumsAndConstants;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class JetSquelcher : BaseSplattershot
    {
        public override float InkCost { get => 1.5f; }

        public override SoundStyle ShootSample { get => SoundPaths.JetSquelcherShoot.ToSoundStyle(); }
        public override int ShotGravityDelay => 30;
        public override int ShotExtraUpdates { get => 8; }
        public override float MuzzleOffsetPx { get; set; } = 56f;
        public override Vector2? HoldoutOffset() { return new Vector2(-2, 4); }
        public override float AimDeviation { get => 2f; }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 28;
            Item.width = 64;
            Item.height = 36;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes() => AddRecipePostMechBoss(true, ItemID.SoulofSight);
    }
}
