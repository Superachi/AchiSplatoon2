using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria;
using Terraria.Audio;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    [ItemCategory("Splatling", "Splatling")]
    internal class BaseSplatling : BaseWeapon
    {
        public override float InkCost { get => 1.5f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Splatling;

        public override SoundStyle ShootSample { get => SoundPaths.SplatlingShoot.ToSoundStyle(); }

        public virtual float[] ChargeTimeThresholds { get => [30f, 60f]; }
        public virtual float BarrageVelocity { get; set; } = 10f;
        public virtual int BarrageShotTime { get; set; } = 5;
        public virtual int BarrageMaxAmmo { get; set; } = 20;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.noMelee = true;
            Item.channel = true;
        }
    }
}
