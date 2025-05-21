using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables
{
    internal class InkTankDroplet : BaseItem
    {
        private Color _inkColor = Color.Orange;
        private int _timeSpentAlive = 0;
        private float _alphaMod = 1f;
        private readonly float _lightAmount = 0.005f;
        private readonly float _lifeTime = 1500;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IgnoresEncumberingStone[Item.type] = true;
            ItemID.Sets.IsAPickup[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;

            Item.rare = ItemRarityID.Expert;
        }

        public override void OnSpawn(IEntitySource source)
        {
            _alphaMod = 0.8f;
            _inkColor = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer();
            SoundHelper.PlayAudio(SoundID.Item154, 0.5f, 0.2f, 10, 0.8f, Main.LocalPlayer.Center);

            for (int i = 0; i < 10; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: Item.Center,
                    Type: DustID.FireworksRGB,
                    Velocity: Main.rand.NextVector2CircularEdge(5, 5),
                    newColor: _inkColor,
                    Scale: Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
                d.velocity *= Main.rand.NextFloat(0.8f, 1.2f);
            }
        }

        public override bool? UseItem(Player player)
        {
            return Consume(player);
        }

        public override bool OnPickup(Player player)
        {
            Consume(player);
            SoundHelper.PlayAudio(SoundID.Item87, 0.5f, 0.2f, 10, 0.2f, Main.LocalPlayer.Center);

            return false;
        }

        private bool Consume(Player player)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
                inkTankPlayer.HealInk(inkTankPlayer.InkAmountFinalMax / 2);

                if (player.HasAccessory<HoneyInkTank>())
                {
                    player.AddBuff(BuffID.Honey, 180);
                }

                return true;
            }

            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            var lightCol = _inkColor * _lightAmount;
            Lighting.AddLight(Item.position, lightCol.R, lightCol.G, lightCol.B);

            _timeSpentAlive++;
            if (_timeSpentAlive > _lifeTime)
            {
                _alphaMod -= 0.05f;
                if (_alphaMod <= 0)
                {
                    Item.TurnToAir(true);
                    Item.active = false;
                }
            }

            if (_timeSpentAlive % 10 == 0)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustPerfect(
                        Position: Item.Center + Main.rand.NextVector2Circular(15, 15),
                        Type: ModContent.DustType<SplatterBulletLastingDust>(),
                        Velocity: new Vector2(0, -Main.rand.NextFloat(3, 5)),
                        newColor: _inkColor,
                        Scale: 1.0f);
                }

                if (Main.rand.NextBool(5))
                {
                    var dust = Dust.NewDustPerfect(
                        Position: Item.Center + Main.rand.NextVector2Circular(15, 15),
                        Type: DustID.AncientLight,
                        Velocity: Vector2.Zero,
                        newColor: _inkColor,
                        Scale: 1.0f);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.noLightEmittence = true;
                }
            }
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange = 200;
        }

        public override bool GrabStyle(Player player)
        {
            Item.velocity += Item.Center.DirectionTo(player.Center);

            if (Item.velocity.Length() > 5f)
            {
                Item.velocity *= 0.9f;
            }

            return true;
        }

        public override bool CanPickup(Player player)
        {
            return base.CanPickup(player);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return ColorHelper.ColorWithAlpha255(_inkColor) * _alphaMod;
        }
    }
}
