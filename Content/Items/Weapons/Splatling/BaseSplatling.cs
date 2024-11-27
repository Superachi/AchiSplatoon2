using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Splatling
{
    internal class BaseSplatling : BaseWeapon
    {
        public override float InkCost { get => 1f; }
        public override MainWeaponStyle WeaponStyle => MainWeaponStyle.Splatling;

        public override string ShootSample { get => "SplatlingShoot"; }
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
