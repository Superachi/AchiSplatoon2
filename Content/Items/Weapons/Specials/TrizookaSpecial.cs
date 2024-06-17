using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class TrizookaSpecial : BaseSpecial
    {
        public override string ShootSample { get => "SplattershotShoot"; }
        public override Vector2? HoldoutOffset() { return new Vector2(4, 0); }
        public override float MuzzleOffsetPx { get; set; } = 44f;

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            Item.useAnimation = Item.useTime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
        }
    }
}
