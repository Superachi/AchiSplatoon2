using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.ExtensionMethods;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class Splashdown : BaseSpecial
    {
        public override float SpecialDrainPerTick => 0.6f;
        public override float RechargeCostPenalty { get => 80f; }
        public virtual int ExplosionRadius => 600;
        public virtual bool SummonFists => false;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 200;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 8;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(player, ref damage);
        }
    }
}
