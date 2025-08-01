using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Palettes;
using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Content.Items.Consumables.ShellOutCapsules;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.StaticData.LootTables;
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

                // The only exception when it comes to loot drops
                // See also BossLootTable.cs
                bool isEaterOfWorlds = npc.type == NPCID.EaterofWorldsHead
                    || npc.type == NPCID.EaterofWorldsBody
                    || npc.type == NPCID.EaterofWorldsTail;

                if (!isEaterOfWorlds) return;

                if (Main.expertMode) return;

                var totalSegmentCount =
                        NpcHelper.CountNPCs(NPCID.EaterofWorldsHead)
                    + NpcHelper.CountNPCs(NPCID.EaterofWorldsBody)
                    + NpcHelper.CountNPCs(NPCID.EaterofWorldsTail);

                if (totalSegmentCount == 1)
                {
                    var player = Main.LocalPlayer;

                    void SpawnLoot(int itemType)
                    {
                        int mainItemId = player.QuickSpawnItem(player.GetSource_DropAsItem(), itemType);
                        var item = Main.item[mainItemId];
                        item.Center = npc.Center;
                    }

                    SpawnLoot(ModContent.ItemType<ChipPalette>());
                    SpawnLoot(ModContent.ItemType<BombRush>());
                    SpawnLoot(ModContent.ItemType<ColorChipEmpty>());
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
                    Dust.NewDust(position, 0, 0, DustID.FireworksRGB, velocity.X, velocity.Y, 0, colorChipPlayer.GetColorFromInkPlayer(), Main.rand.NextFloat(0.5f, 1.5f))
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

            if (NpcHelper.IsTargetABossMinion(npc)
                || NpcHelper.IsTargetAProjectile(npc)
                || NpcHelper.IsTargetAWormSegment(npc)
                || npc.friendly
                || npc.SpawnedFromStatue)
            {
                return;
            }

            npcLoot.Add(
                ItemDropRule.Common(
                ModContent.ItemType<InkDroplet>(),
                chanceDenominator: 50,
                minimumDropped: 1,
                maximumDropped: 3));

            npcLoot.Add(
                ItemDropRule.Common(
                ModContent.ItemType<ShellOutCapsule>(),
                chanceDenominator: 500,
                minimumDropped: 3,
                maximumDropped: 3));
        }
    }
}
