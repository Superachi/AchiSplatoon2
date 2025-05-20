using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Consumables.ColorVials
{
    internal class BaseVial : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                ApplyVialEffect(player);
            }

            return true;
        }

        public override void RightClick(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                ApplyVialEffect(player);
            }
        }

        public override bool ConsumeItem(Player player)
        {
            SoundHelper.PlayAudio(SoundID.Shimmer1, volume: 0.5f);
            return false;
        }

        protected virtual void ApplyVialEffect(Player player)
        {
            var inkColorPlayer = player.GetModPlayer<InkColorPlayer>();
            inkColorPlayer.SetSingleColor(Color.White);
        }
    }
}
