using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Content.Items.Consumables.LootBags;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Unclassed;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.StaticData;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.GlobalNPCs
{
    internal class LootGlobalNPC : BaseGlobalNPC
    {
        private ColorChipPlayer colorChipPlayer => Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();

        public override void OnKill(NPC npc)
        {
            // Lucky drops, affected by lucky color chips
            if (Main.LocalPlayer.whoAmI == Main.myPlayer) // TODO: Might be a non-sensical if statement, needs testing
            {
                float chipCount = colorChipPlayer.ColorChipAmounts[(int)ChipColor.Green];

                if (npc.friendly
                    || npc.SpawnedFromStatue
                    || Main.npcCatchable[npc.type]
                    || NpcHelper.IsTargetAProjectile(npc)
                    || NpcHelper.IsTargetABossMinion(npc))
                {
                    return;
                }

                float chanceModifier = 1f;
                if (chipCount > 0)
                {
                    chanceModifier = 1f / (1f + chipCount / colorChipPlayer.GreenChipLootBonusDivider);
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
                if (Main.rand.NextBool((int)(200f * chanceModifier)))
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
            }
        }

        private void RareLootDropPlayerFeedback(NPC npc)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust;
                Vector2 position = npc.Center;
                Vector2 velocity = Main.rand.NextVector2Circular(15, 15);
                dust = Main.dust[
                    Dust.NewDust(position, 0, 0, DustID.FireworksRGB, velocity.X, velocity.Y, 0, colorChipPlayer.GetColorFromChips(), Main.rand.NextFloat(0.5f, 1.5f))
                    ];
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            SoundHelper.PlayAudio(SoundPaths.ItemGet.ToSoundStyle(), volume: 0.6f, maxInstances: 1, position: npc.position);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            foreach (var loot in LootTables.List)
            {
                if (npc.type == loot.NpcId)
                {
                    loot.RegisterDirectDrop(npcLoot);
                }
            }

            // Super palette crafting materials
            if (npc.type == NPCID.MartianSaucerCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MartianBrella>(), 3));
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

            #region Dungeon (post-plantera)

            var dungeonDropChanceDenominator = 8;
            switch (npc.type)
            {
                case NPCID.SkeletonSniper:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Snipewriter5B>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.TacticalSkeleton:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RecycleBrellaMk2>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.SkeletonCommando:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SBlast91>(), dungeonDropChanceDenominator));
                    break;
                case NPCID.Paladin:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PainBrushNouveau>(), dungeonDropChanceDenominator / 2));
                    break;
            }

            if (npc.type >= NPCID.RustyArmoredBonesAxe && npc.type <= NPCID.HellArmoredBonesSword)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SplashOMaticNeo>(), dungeonDropChanceDenominator * 10));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SplooshOMaticNeo>(), dungeonDropChanceDenominator * 10));
            }

            #endregion

            #region Pumpkin moon

            switch (npc.type)
            {
                case NPCID.MourningWood:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CustomDouserDualie>(), 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloblobberDeco>(), 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CustomWellstring>(), 15));
                    break;

                case NPCID.Pumpking:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpookyBrush>(), 3));
                    break;
            }

            #endregion

            #region Frost moon

            switch (npc.type)
            {
                case NPCID.Everscream:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Squeezer>(), 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LightTetraDualie>(), 10));
                    break;

                case NPCID.SantaNK1:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Explosher>(), 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CustomHydraSplatling>(), 10));
                    break;

                case NPCID.IceQueen:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceStringer>(), 3));
                    break;
            }

            #endregion

            #region Rare drops

            if (npc.type == NPCID.Squid)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<SquidBoomerang>(),
                    chanceDenominator: 10));

                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<InkDroplet>(),
                    chanceDenominator: 5,
                    minimumDropped: 1,
                    maximumDropped: 5));
            }

            if (npc.FullName.ToLowerInvariant().Contains("hornet") && npc.type != NPCID.VortexHornet && npc.type != NPCID.VortexHornetQueen)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HoneyScepter>(), 100));
            }

            if (npc.type == NPCID.Eyezor)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<LaserAddon>(),
                    chanceDenominator: 3));
            }

            #endregion
        }
    }
}
