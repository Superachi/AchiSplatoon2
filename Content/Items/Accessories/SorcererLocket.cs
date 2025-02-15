using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class SorcererLocket : DropletLocket
    {
        public override float MagicDamageMod()
        {
            return 0.15f;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<SorcererLocket>();

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                player.GetDamage(DamageClass.Magic) *= (1 + MagicDamageMod());
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DropletLocket>(), 1)
                .AddIngredient(ItemID.SorcererEmblem, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
