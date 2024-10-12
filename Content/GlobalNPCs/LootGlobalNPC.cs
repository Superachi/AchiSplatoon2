using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
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

                if (npc.friendly) { return; }
                if (Main.npcCatchable[npc.type]) { return; }

                float chanceModifier = 1f;
                if (chipCount > 0)
                {
                    chanceModifier = 1f / (1f + chipCount / modPlayer.GreenChipLootBonusDivider);
                    chanceModifier = Math.Max(1f, chanceModifier);

                    if (Main.rand.NextBool((int)(50f * chanceModifier)))
                    {
                        if (Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2)
                        {
                            Item.NewItem(npc.GetSource_Loot(), npc.Center, ItemID.Heart);
                        }
                    }

                    if (Main.rand.NextBool((int)(25f * chanceModifier)))
                    {
                        if (Main.LocalPlayer.statMana < Main.LocalPlayer.statManaMax2)
                        {
                            Item.NewItem(npc.GetSource_Loot(), npc.Center, ItemID.Star);
                        }
                    }
                }

                // Canned special drop chance
                if (Main.rand.NextBool((int)(100f * chanceModifier)))
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.Center, ModContent.ItemType<CannedSpecial>());
                    RareLootDropPlayerFeedback(npc);
                }

                // Sheldon license drop chance
                if (Condition.DownedEarlygameBoss.IsMet() && Main.rand.NextBool((int)(250f * chanceModifier)))
                {
                    int licenseId = !Main.hardMode ? ModContent.ItemType<SheldonLicense>() : ModContent.ItemType<SheldonLicenseSilver>();
                    Item.NewItem(npc.GetSource_Loot(), npc.Center, licenseId);
                    RareLootDropPlayerFeedback(npc);
                }

                // Sub weapon drop chance
                if (Main.rand.NextBool((int)(80f * chanceModifier)))
                {
                    int stackSize;
                    if (!Main.hardMode) stackSize = Main.rand.Next(5, 10);
                    else stackSize = Main.rand.Next(10, 20);

                    var subWeapons = new List<int>
                    {
                        ModContent.ItemType<SplatBomb>(),
                        ModContent.ItemType<BurstBomb>(),
                        ModContent.ItemType<AngleShooter>(),
                        ModContent.ItemType<Sprinkler>(),
                    };

                    int droppedSub = Main.rand.NextFromCollection<int>(subWeapons);
                    Item.NewItem(
                        source: npc.GetSource_Loot(),
                        position: npc.Center,
                        Type: droppedSub,
                        Stack: stackSize
                    );
                }
            }
        }

        private void RareLootDropPlayerFeedback(NPC npc)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();

            for (int i = 0; i < 15; i++)
            {
                Dust dust;
                Vector2 position = npc.Center;
                Vector2 velocity = Main.rand.NextVector2Circular(15, 15);
                dust = Main.dust[
                    Dust.NewDust(position, 0, 0, DustID.FireworksRGB, velocity.X, velocity.Y, 0, modPlayer.ColorFromChips, Main.rand.NextFloat(0.5f, 1.5f))
                    ];
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            SoundHelper.PlayAudio("ItemGet", volume: 0.6f, maxInstances: 1, position: npc.position);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            var dungeonDropChanceDenominator = 16;
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
                case NPCID.HallowBoss: // Empress of Light
                case NPCID.Golem:
                case NPCID.Pumpking:
                case NPCID.SantaNK1:
                    npcLoot.Add(notExpertRule);
                    AddBossLootDisregardingDifficulty(notExpertRule, ModContent.ItemType<SheldonLicenseGold>());
                    break;

                case NPCID.IceQueen:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceStringer>(), 3));
                    break;

                // Dungeon drops
                case NPCID.SkeletonSniper:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Snipewriter5B>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.TacticalSkeleton:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RecycleBrellaMk2>(), dungeonDropChanceDenominator));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrayonBox>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.BoneLee:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SquidClipOns>(), dungeonDropChanceDenominator));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PinkSponge>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.SkeletonCommando:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SBlast91>(), dungeonDropChanceDenominator));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FieryPaintCan>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.Paladin:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloblobberDeco>(), dungeonDropChanceDenominator / 2));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PainBrushNouveau>(), dungeonDropChanceDenominator / 2));
                    break;
                case NPCID.Necromancer:
                case NPCID.NecromancerArmored:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DappleDualieNouveau>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.DiabolistRed:
                case NPCID.DiabolistWhite:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CustomDouserDualie>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.RaggedCaster:
                case NPCID.RaggedCasterOpenCoat:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LightTetraDualie>(), dungeonDropChanceDenominator));
                    break;
            }

            if (npc.type >= NPCID.RustyArmoredBonesAxe && npc.type <= NPCID.HellArmoredBonesSword)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SplashOMaticNeo>(), dungeonDropChanceDenominator * 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SplooshOMaticNeo>(), dungeonDropChanceDenominator * 8));
            }

            // Super palette crafting materials
            if (npc.type == NPCID.MartianSaucer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuperPaletteMiddlePart>(), 3));
            }

            if (npc.type == NPCID.PirateShip)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuperPaletteLeftPart>(), 3));
            }

            if (npc.type == NPCID.Mothron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuperPaletteRightPart>(), 3));
            }
        }
    }
}
