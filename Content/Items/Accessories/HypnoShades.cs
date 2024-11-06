using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class HypnoShades : BaseAccessory
    {
        public static float BombDamageBonus = 1f;
        public static float BombUseTimeMod = 0.6f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(BombDamageBonus * 100));

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.hasHypnoShades = true;
                modPlayer.subPowerMultiplier += BombDamageBonus;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sunglasses, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .Register();
        }
    }
}
