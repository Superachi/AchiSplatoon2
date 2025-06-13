using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Attributes;

namespace AchiSplatoon2.Content.Items.Accessories.RushAttacks
{
    [ItemCategory("Accessory", "RushAttacks")]
    internal class UrchinEmblem : BaseAccessory
    {
        public static int GetAttackDamage()
        {
            if (!BossConditions.DownedEvilBoss.IsMet())
            {
                return 15;
            }

            if (!Condition.Hardmode.IsMet())
            {
                return 25;
            }

            if (!Condition.DownedPlantera.IsMet())
            {
                return 50;
            }

            return 80;
        }

        public static int AttackCooldown => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 30;
            Item.height = 30;
            Item.SetValuePreEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.TryEquipAccessory<UrchinEmblem>();
            }
        }
    }
}
