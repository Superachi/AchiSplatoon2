using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Content.Players;
using static AchiSplatoon2.Content.Players.InkWeaponPlayer;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class LootGlobalNPC : BaseGlobalNPC
    {
        private void AddBossLootDisregardingDifficulty(LeadingConditionRule expertRule, int itemID)
        {
            expertRule.OnSuccess(ItemDropRule.Common(itemID));
            expertRule.OnFailedConditions(ItemDropRule.BossBag(itemID));
        }

        public override void OnKill(NPC npc)
        {
            // Lucky drops, affected by lucky color chips
            if (Main.LocalPlayer.whoAmI == Main.myPlayer) // TODO: Might be a non-sensical if statement, needs testing
            {
                var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
                float chipCount = modPlayer.ColorChipAmounts[(int)ChipColor.Green];

                if (chipCount <= 0) { return; }
                float chanceModifier = 1f / (1f + chipCount / modPlayer.GreenChipLootBonusDivider);

                if (Main.rand.NextBool((int)(200f * chanceModifier)))
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.Center, ModContent.ItemType<CannedSpecial>());
                }

                if (Main.rand.NextBool((int)(50f * chanceModifier)))
                {
                    if (Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Center, ItemID.Heart);
                    }
                }

                if (Main.rand.NextBool((int)(50f * chanceModifier)))
                {
                    if (Main.LocalPlayer.statMana < Main.LocalPlayer.statManaMax2)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.Center, ItemID.Star);
                    }
                }
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            switch (npc.type)
            {
                case NPCID.KingSlime:
                case NPCID.EyeofCthulhu:
                case NPCID.QueenBee:
                case NPCID.SkeletronHead:
                case NPCID.Deerclops:
                    npcLoot.Add(notExpertRule);
                    AddBossLootDisregardingDifficulty(notExpertRule, ModContent.ItemType<SheldonLicense>());
                    break;
                case NPCID.Mimic:
                case NPCID.WallofFlesh:
                case NPCID.BigMimicHallow:
                case NPCID.BigMimicCrimson:
                case NPCID.BigMimicCorruption:
                case NPCID.QueenSlimeBoss:
                case NPCID.TheDestroyer:
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                case NPCID.SkeletronPrime:
                    npcLoot.Add(notExpertRule);
                    AddBossLootDisregardingDifficulty(notExpertRule, ModContent.ItemType<SheldonLicenseSilver>());
                    break;
                case NPCID.DukeFishron:
                case NPCID.Plantera:
                case NPCID.EmpressButterfly:
                case NPCID.Golem:
                    npcLoot.Add(notExpertRule);
                    AddBossLootDisregardingDifficulty(notExpertRule, ModContent.ItemType<SheldonLicenseGold>());
                    break;
            }
        }
    }
}
