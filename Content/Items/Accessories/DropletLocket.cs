using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class DropletLocket : BaseAccessory
    {
        public static float MagicDamageBonus => 0.05f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(MagicDamageBonus * 100));

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<DropletLocket>();

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                player.GetDamage(DamageClass.Magic) *= (1 + MagicDamageBonus);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 10)
                .AddIngredient(ItemID.ManaCrystal, 1)
                .AddIngredient(ItemID.WhiteString, 1)
                .Register();
        }
    }
}
