using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class TrizookaSpecial : BaseSpecial
    {
        public override Vector2? HoldoutOffset() { return new Vector2(-40, -8); }
        public override float MuzzleOffsetPx { get; set; } = 80f;
        public static readonly int ProjPerShot = 3;
        public static readonly int MaxBursts = 3;
        protected override string UsageHintParamA => ProjPerShot.ToString();
        protected override string UsageHintParamB => MaxBursts.ToString();
        public override float SpecialDrainPerTick => 0.2f;
        public override float SpecialDrainPerUse => 0f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 150;
            Item.width = 90;
            Item.height = 44;
            Item.knockBack = 10;
        }
    }
}
