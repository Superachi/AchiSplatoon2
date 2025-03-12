using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class DropletLocket : BaseAccessory
    {
        public virtual float MagicDamageMod()
        {
            return 0.05f;
        }

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(MagicDamageMod() * 100));

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 28;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<DropletLocket>();

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                player.GetDamage(DamageClass.Magic) *= (1 + MagicDamageMod());
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
