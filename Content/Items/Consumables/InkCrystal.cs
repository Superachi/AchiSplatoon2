using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.CraftingMaterials;

namespace AchiSplatoon2.Content.Items.Consumables
{
    internal class InkCrystal : BaseItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(InkTankPlayer.ValuePerCrystal, InkTankPlayer.InkCrystalsMax);

        public override void SetDefaults()
        {
            Item.consumable = true;

            Item.width = 20;
            Item.height = 24;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool? UseItem(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
                var success = inkTankPlayer.ConsumeInkCrystal();

                if (success) ConsumeEffect(player);
                return success;
            }

            return false;
        }

        private void ConsumeEffect(Player player)
        {
            SoundHelper.PlayAudio(SoundID.Item165, 0.2f, 0.2f, 10, 0f, player.Center);
            SoundHelper.PlayAudio(SoundID.Item25, 0.8f, 0.1f, 10, 0f, player.Center);
            SoundHelper.PlayAudio(SoundID.Item66, 0.2f, 0.1f, 10, 0f, player.Center);

            var color = player.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
            color = ColorHelper.ColorWithAlpha255(color);

            int loopAmount = 72;
            for (int i = 1; i < loopAmount; i++)
            {
                var offset = new Vector2(100, 0);
                offset = WoomyMathHelper.AddRotationToVector2(offset, i * 360 / loopAmount);

                var d = Dust.NewDustDirect(
                    player.Center + offset,
                    0,
                    0,
                    DustID.RainbowTorch,
                    newColor: ColorHelper.IncreaseHueBy(i * 360 / loopAmount + (int)Main.time % 360, color),
                    Scale: Main.rand.NextFloat(1f, 2f));

                d.velocity = d.position.DirectionTo(player.Center) * 3 * (i % 2 == 0 ? 1 : -1);
                d.velocity = WoomyMathHelper.AddRotationToVector2(d.velocity, 0);
                d.noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 5)
                .AddIngredient(ItemID.CrystalShard, 10)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddIngredient(ItemID.SoulofLight, 1)
                .Register();
        }
    }
}
