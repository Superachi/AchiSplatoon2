using AchiSplatoon2.Content.Players;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Consumables
{
    internal class CannedSpecial : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 1);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
                modPlayer.IncrementSpecialPoints(modPlayer.SpecialPointsMax);
                return true;
            }
            return false;
        }
    }
}
