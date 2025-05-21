using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    [OrderWeapon]
    internal class OrderShot : Splattershot
    {
        public override SoundStyle ShootSample { get => SoundPaths.OrderShotShoot.ToSoundStyle(); }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.shootSpeed = 4.5f;
            Item.useTime = 12;
            Item.useAnimation = Item.useTime;
            Item.damage = 8;
            Item.knockBack = 0f;
            Item.SetValuePreEvilBosses();
        }

        public override void AddRecipes() => AddRecipeOrder();
    }
}
