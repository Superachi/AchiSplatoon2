using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Items.Weapons.Chargers
{
    internal class BaseCharger : BaseWeapon
    {
        public virtual float[] ChargeTimeThresholds { get => [60f]; }
        public virtual bool ScreenShake { get => true; }
        public virtual int MaxPenetrate { get => 10; }
        public virtual bool DirectHitEffect { get => true; }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.AchiSplatoon.hjson' file.
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.noMelee = true;
            Item.channel = true;
            Item.crit = 5;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<InkAccessoryPlayer>();
                if (accMP.hasTentacleScope)
                {
                    crit += TentacularOcular.BaseCritChance;
                }
            }
        }
    }
}
