using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria.Audio;
using AchiSplatoon2.ModSystems;
using System.Linq;
using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Test;
using AchiSplatoon2.Content.Items.Consumables.ShellOutCapsules;

namespace AchiSplatoon2.Content.CustomNPCs
{
    [AutoloadHead]
    internal class SheldonNPC : ModNPC
    {
	    public override LocalizedText DisplayName => this.GetLocalization("Ammo Knight");
        private string ShopName => "SheldonShop";

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 20;
            NPC.height = 20;
            NPC.aiStyle = 7;
            NPC.defense = 15;
            NPC.lifeMax = 250;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.AttackFrameCount[NPC.type] = 1;
            NPCID.Sets.DangerDetectRange[NPC.type] = 500;
            NPCID.Sets.AttackType[NPC.type] = 1;
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
            AnimationType = 22;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (var i = 0; i < 255; i++)
            {
                Player player = Main.player[i];

                foreach (Item item in player.inventory)
                {
                    if (item.ModItem is BaseWeapon)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Sheldon",
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "Hints";
        }

        private bool TryFormatKeybindText(ModKeybind keybind, out string result)
        {
            var keyString = keybind.GetAssignedKeys().FirstOrDefault();

            if (keyString != null)
            {
                result = ColorHelper.TextWithKeybindColor(keyString);
                return true;
            }

            result = "(Unbound control input!)";
            return false;
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = ShopName;
                return;
            }

            var hints = new List<string>();

            // Neutral hints
            bool success;

            // Ink tank upgrade hint
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<InkTank>()))
            {
                hints.Add($"Did you know you can upgrade your {TextHelper.ItemEmoji<InkTank>()}? You can combine it with {TextHelper.ItemEmoji(ItemID.FallenStar)} and {TextHelper.ItemEmoji(ItemID.DemoniteBar)} or {TextHelper.ItemEmoji(ItemID.CrimtaneBar)}, increasing its capacity and allowing you to fire ink underwater!");
            }

            if (!Condition.DownedEyeOfCthulhu.IsMet())
            {
                // Order weapons
                hints.Add($"You can craft more Order weapons after obtaining some {TextHelper.ItemEmoji(ItemID.SilverBar)} or {TextHelper.ItemEmoji(ItemID.TungstenBar)}, a gem, like {TextHelper.ItemEmoji(ItemID.Amethyst)}, and some {TextHelper.ItemEmoji<InkDroplet>()}!");
            }
            else
            {
                // Pearl drone hint
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ColorChipAqua>()))
                {
                    hints.Add($"Are you looking for " + ColorHelper.TextWithPearlColor("Pearl Drone") + $"? You can use a {TextHelper.ItemEmoji<PearlDroneStaff>()}, obtained after splatting the fearsome Eye of Cthulhu!");
                }

                // Unique weapons
                hints.Add($"There are some items out there that elude even my own research! What I HAVE learned is that {TextHelper.ItemEmoji<InkDroplet>()} can be used for many things...");
            }

            if (!BossConditions.DownedEvilBoss.IsMet())
            {
                // Sub keybind
                success = TryFormatKeybindText(KeybindSystem.SubWeaponKeybind, out string subKeybindText);

                if (success)
                {
                    hints.Add($"You can use your sub weapon by pressing {subKeybindText}! A {TextHelper.ItemEmoji<StarterSplatBomb>()} for example, consumes a lot of ink, but potentially damages many targets within its range!");
                }
                else
                {
                    hints.Add($"Don't forget to use your sub weapons! If you bind a key for it in your settings, you can easily use it even when not selected directly.");
                }

                success = TryFormatKeybindText(KeybindSystem.SpecialWeaponKeybind, out string specialKeybindText);

                // Special keybind
                if (success)
                {
                    hints.Add($"You can use your special weapon by pressing {subKeybindText}! However, you need to make sure you've splatted enough enemies to fill your special guage.");
                }
                else
                {
                    hints.Add($"Don't forget to use your special weapons! If you bind a key for it in your settings, you can easily use it even when not selected directly.");
                }

                // Swim form
                success = TryFormatKeybindText(KeybindSystem.SwimFormKeybind, out string swimFormKeybindText);

                if (success)
                {
                    hints.Add($"You can enter swim form by pressing {swimFormKeybindText}. If you remain still in this state, you recover ink much faster!");
                }
                else
                {
                    hints.Add($"You can enter swim form to recover ink faster! However, you do need to bind a key for it in your settings.");
                }

                // Inkzooka
                hints.Add($"Have you tried the {TextHelper.ItemEmoji<Inkzooka>()} yet? It's great in a pinch, as it unleashes fast whirlpools that pierce enemies and terrain, dealing massive damage!");
            }
            else
            {
                if (!Condition.Hardmode.IsMet())
                {
                    // Meteor progression hint
                    var bossName = Condition.CorruptWorld.IsMet() ? "Eater of Worlds" : "Brain of Cthulhu";

                    hints.Add($"Now that you've splatted the {bossName}, you could look for a landed meteor! {TextHelper.ItemEmoji(ItemID.MeteoriteBar)} can be used to craft powerful weapons, and even variations of {TextHelper.ItemEmoji<ColorChipEmpty>()}!");
                }

                // Mobility chip hint
                hints.Add($"Besides splatting enemies, there are other ways to charge your special guage! {TextHelper.ItemEmoji<ColorChipBlue>()} can charge up the guage while running with a roller or brush out.");

                // Shell-Out Capsule hint
                if (!Main.LocalPlayer.HasItem(ModContent.ItemType<ShellOutCapsule>()))
                {
                    hints.Add($"Do you happen to have too many {TextHelper.ItemEmoji<SheldonLicense>()}? You can craft them into {TextHelper.ItemEmoji<ShellOutCapsule>()}! These can drop practically anything! ...If you're lucky.");
                }
            }

            if (!Condition.DownedQueenBee.IsMet())
            {
                hints.Add($"The Queen Bee is a formiddable foe, but splatting her grants you access to weapons like the {TextHelper.ItemEmoji<SplatanaWiper>()}, and she even has something that  " + ColorHelper.TextWithPearlColor("Pearl") + " will certainly like...");

                hints.Add($"Have you been to the Jungle yet? There are some materials there that you could use to upgrade your {TextHelper.ItemEmoji<InkTank>()}!");
            }

            // Hardmode
            if (Condition.Hardmode.IsMet())
            {
                // Mimic hint
                hints.Add($"It's worth hunting down mimics in the underground! Small mimics drop {TextHelper.ItemEmoji<MainSaverEmblem>()} and {TextHelper.ItemEmoji<SubSaverEmblem>()}, while large ones can drop {TextHelper.ItemEmoji<LastDitchEffortEmblem>()} and other accessories!");

                // Shimmer hint
                hints.Add($"I've received word that there is a liquid that has transformative effects on items! I wonder if it could work with your weapons?");

                hints.Add($"If you have some {TextHelper.ItemEmoji<SheldonLicense>()} laying around, you can combine them with {TextHelper.ItemEmoji(ItemID.SoulofLight)} and {TextHelper.ItemEmoji(ItemID.SoulofNight)} to create {TextHelper.ItemEmoji<SheldonLicenseSilver>()}!");

                if (!Condition.DownedMechBossAny.IsMet())
                {
                    // Ore hint
                    hints.Add($"Don't skip out on mining valuable ore! I'm talking about things like {TextHelper.ItemEmoji(ItemID.CobaltOre)}. You can actually use those to make stronger versions of weapons you might be familiar with!");
                }
                else
                {
                    hints.Add($"Have you visited the Mechanic recently? I heard she salvaged a battery from the mechanical boss you splatted.");
                }

                if (Condition.DownedMechBossAll.IsMet())
                {
                    hints.Add($"According to my research, you should be able to upgrade some of your existing weapons using {TextHelper.ItemEmoji(ItemID.ChlorophyteBar)}! For example, you can upgrade the {TextHelper.ItemEmoji<JetSquelcher>()} into a pair of {TextHelper.ItemEmoji<DualieSquelcher>()}!");
                }
            }

            if (hints.Count == 0)
            {
                hints.Add("I've nothing left to say! That's... very rare.");
            }

            var hintsWithNumber = new List<string>();
            int i = 1;
            foreach(var hint in hints)
            {
                hintsWithNumber.Add(ColorHelper.TextWithKeybindColor($"{i}/{hints.Count}") + "\n" + hint);
                i++;
            }

            if (hintsWithNumber.Count == 1)
            {
                Main.npcChatText = hintsWithNumber[0];
            }
            else
            {
                Main.npcChatText = hintsWithNumber[SheldonSystem.HintCycle % (hintsWithNumber.Count)];
            }

            SheldonSystem.IncrementHintCycle();
        }

        public override void AddShops()
        {
            var shop = new NPCShop(Type, ShopName)
                .Add(ModContent.ItemType<SplattershotJr>())
                .Add(ModContent.ItemType<StarterSplatBomb>())
                .Add(ModContent.ItemType<Inkzooka>())
                .Add(ModContent.ItemType<InkTank>())
                .Add(ModContent.ItemType<InkDroplet>());

            shop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            base.ModifyActiveShop(shopName, items);
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<SheldonNPC>());
            var playerName = Main.LocalPlayer.name;

            var defaultQuotes = new List<string>()
            {
                $"Hello, hello, {playerName}! What are you in the market for today?",
                $"Welcome to Ammo Knights! What'll it be?",
                $"Looking for some new weapons? You've come to the right place!",
            };

            var voiceSamples = new List<SoundStyle>()
            {
                SoundPaths.SheldonChat1.ToSoundStyle(),
                SoundPaths.SheldonChat2.ToSoundStyle()
            };

            SoundHelper.PlayAudio(Main.rand.NextFromCollection(voiceSamples), 0.5f, pitchVariance: 0.1f, position: NPC.Center);

            return Main.rand.NextFromCollection(defaultQuotes);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 15;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Bullet;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<Inkbrush>());
        }
    }
}
