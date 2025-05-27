using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Content.CustomNPCs;
using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.Content.Items.Consumables.ColorVials;
using AchiSplatoon2.Content.Items.Consumables.ColorVials.SingleColors;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class ShopGlobalNPC : BaseGlobalNPC
    {
        public static List<int> ShopItemMoonPhases { get; private set; } = new List<int>();

        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Mechanic)
            {
                shop.Add<ChargedBattery>(Condition.Hardmode);
            }

            if (shop.NpcType == NPCID.Painter)
            {
                shop.Add<PainterDualie>(Condition.DownedEarlygameBoss);

                shop.Add<ConfigVial>();
                shop.Add<OrderVial>();
                shop.Add<OrangeVial>();
                shop.Add<BlueVial>();
                shop.Add<PinkVial>();
                shop.Add<GreenVial>();
            }

            if (shop.NpcType == ModContent.NPCType<SheldonNPC>())
            {
                // Sub weapons
                shop.Add<BurstBomb>(Condition.DownedEarlygameBoss);
                shop.Add<PointSensor>(Condition.DownedEarlygameBoss);
                shop.Add<SplatBomb>(BossConditions.DownedEvilBoss);
                shop.Add<AngleShooter>(BossConditions.DownedEvilBoss);
                shop.Add<Sprinkler>(Condition.DownedSkeletron);
                shop.Add<InkMine>(Condition.DownedSkeletron);
                shop.Add<Torpedo>(Condition.Hardmode);

                // Pre evil bosses
                var list = new List<int>()
                {
                    ModContent.ItemType<OrderBrush>(),
                    ModContent.ItemType<OrderDualie>(),
                    ModContent.ItemType<OrderBrella>(),
                    ModContent.ItemType<OrderRoller>(),
                    ModContent.ItemType<OrderCharger>(),
                    ModContent.ItemType<OrderBlaster>(),
                };

                SpreadItemsBetweenMoonPhases(shop, list, BossConditions.NotDownedEvilBoss);

                // Post evil bosses
                list = new List<int>()
                {
                    ModContent.ItemType<Splattershot>(),
                    ModContent.ItemType<Inkbrush>(),
                    ModContent.ItemType<SplatCharger>(),
                    ModContent.ItemType<MiniSplatling>(),
                    ModContent.ItemType<SplatRoller>(),
                    ModContent.ItemType<Slosher>(),
                    ModContent.ItemType<RapidBlaster>(),
                    ModContent.ItemType<Blaster>(),
                };

                SpreadItemsBetweenMoonPhases(shop, list, BossConditions.DownedEvilBoss);

                // Post WoF
                list = new List<int>()
                {
                    ModContent.ItemType<TentatekSplattershot>(),
                    ModContent.ItemType<InkbrushNouveau>(),
                    ModContent.ItemType<ZFSplatCharger>(),

                    ModContent.ItemType<ZinkMiniSplatling>(),
                    ModContent.ItemType<KrakonSplatRoller>(),
                    ModContent.ItemType<SplatanaWiperDeco>(),
                };

                SpreadItemsBetweenMoonPhases(shop, list, Condition.Hardmode);

                // Post 1st mech
                list = new List<int>()
                {
                    ModContent.ItemType<SorellaSplatBrella>(),
                    ModContent.ItemType<SlosherDeco>(),
                    ModContent.ItemType<RoyalHeavySplatling>(),

                    ModContent.ItemType<DarkTetraDualie>(),
                    ModContent.ItemType<LunaBlaster>(),
                    ModContent.ItemType<TriStringerInkline>(),
                };

                SpreadItemsBetweenMoonPhases(shop, list, Condition.DownedMechBossAny);

                // Post all mechs
                list = new List<int>()
                {
                    ModContent.ItemType<ClassicSplattershotS1>(),
                    ModContent.ItemType<EnperrySplatDualie>(),
                    ModContent.ItemType<GoldDynamoRoller>(),
                    ModContent.ItemType<JetSquelcher>(),

                    ModContent.ItemType<ClassicSplattershotS2>(),
                    ModContent.ItemType<Wellstring>(),
                    ModContent.ItemType<PainBrush>(),
                    ModContent.ItemType<SBlast92>(),
                };

                SpreadItemsBetweenMoonPhases(shop, list, Condition.DownedMechBossAll);

                // Post cultist
                list = new List<int>()
                {
                    ModContent.ItemType<KensaSplattershotJr>(),
                    ModContent.ItemType<KensaOctobrush>(),
                    ModContent.ItemType<KensaRapidBlaster>(),
                    ModContent.ItemType<KensaReefluxStringer>(),

                    ModContent.ItemType<KensaSplatDualie>(),
                    ModContent.ItemType<KensaSplatRoller>(),
                    ModContent.ItemType<KensaUndercoverBrella>(),
                    ModContent.ItemType<KensaMiniSplatling>(),
                };

                SpreadItemsBetweenMoonPhases(shop, list, Condition.DownedCultist);
            }
        }

        public static void SpreadItemsBetweenMoonPhases(NPCShop shop, List<int> items, Condition? extraCondition)
        {
            var listSize = Math.Min(items.Count, 8);

            foreach (int item in items)
            {
                ShopItemMoonPhases.Add(item);
                var itemMoonPhaseListIndex = ShopItemMoonPhases.IndexOf(item);

                var condition = new Condition("MatchingMoonPhase", () => itemMoonPhaseListIndex % listSize == Main.moonPhase);

                if (extraCondition != null)
                {
                    condition = new Condition("MatchingMoonPhaseAndExtraCond", () => itemMoonPhaseListIndex % listSize == Main.moonPhase && extraCondition.IsMet());
                }

                shop.Add(item, condition);
            }
        }
    }
}
