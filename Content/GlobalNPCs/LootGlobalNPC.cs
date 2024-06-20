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

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class LootGlobalNPC : BaseGlobalNPC
    {
        private void AddBossLootDisregardingDifficulty(LeadingConditionRule expertRule, int itemID)
        {
            expertRule.OnSuccess(ItemDropRule.Common(itemID));
            expertRule.OnFailedConditions(ItemDropRule.BossBag(itemID));
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // Every enemy has a chance to just a special charge up potion
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChargePotion>(), chanceDenominator: 200));

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
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SheldonLicense>()));
                    break;
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
