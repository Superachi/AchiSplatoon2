using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneMinion : BaseProjectile
    {
        private int idleTime = 0;
        private bool hasDroppedItem = false;

        private const int stateIdle = 0;
        private const int stateFollowPlayer = 1;
        private const int stateAttack = 2;
        private const int stateDropItem = 3;

        private int speechCooldownCurrent = 0;
        private int speechCooldownMax = 600;
        private int speechDisplayTime = 0;
        private float speechScale = 0f;
        private string speechText = "";
        private string ownerName = "";

        private string shoutSample = $"Voice\\Pearl\\Shout";
        private int shoutSampleCount = 3;
        private string talkSample = $"Voice\\Pearl\\ShortTalk";
        private int talkSampleCount = 5;

        private int sprinklerCooldown;
        private int sprinklerCooldownMax = 20;
        private int burstBombCooldown;
        private int burstBombCooldownMax = 600;
        private int healCooldown;
        private int healCooldownMax = 7200;

        private NPC? foundTarget = null;

        public override void SetStaticDefaults()
        {
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 24;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 34;
            Projectile.tileCollide = false;

            // These below are needed for a minion weapon
            Projectile.friendly = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
        }

        public override bool? CanCutTiles() => false;

        public override bool MinionContactDamage() => false;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(
                inkColor: Color.White,
                rotation: Projectile.rotation,
                scale: 1,
                alphaMod: 1,
                considerWorldLight: false,
                additiveAmount: 0f,
                flipSpriteSettings: Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            sprinklerCooldown = sprinklerCooldownMax;
            burstBombCooldown = burstBombCooldownMax;
            healCooldown = healCooldownMax;
            ownerName = GetOwner().name;
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);
            idleTime = 0;

            switch (state)
            {
                case stateIdle:
                    break;

                case stateFollowPlayer:
                    break;

                case stateAttack:
                    break;

                case stateDropItem:
                    hasDroppedItem = false;
                    TriggerDialoguePearlHealing();
                    break;
            }
        }

        private void DeductCooldowns()
        {
            if (speechCooldownCurrent > 0) speechCooldownCurrent--;
            if (speechDisplayTime > 0) speechDisplayTime--;

            if (sprinklerCooldown > 0) sprinklerCooldown--;
            if (burstBombCooldown > 0) burstBombCooldown--;

            if (healCooldown > 0) healCooldown--;
        }

        public override void AI()
        {
            if (!CheckPlayerActive(GetOwner()))
            {
                return;
            }

            DeductCooldowns();

            // Disc scratch sound
            if (timeSpentAlive == 15 || timeSpentAlive == 32)
            {
                SoundHelper.PlayAudio(SoundID.Item166, volume: 0.3f, maxInstances: 10, position: Projectile.Center);
            }

            // Greet the player
            if (timeSpentAlive == 44)
            {
                TriggerDialoguePearlAppears();
            }

            if (timeSpentAlive % 20 == 0)
            {
                FindTarget(800f);
                if (foundTarget != null) SetState(stateAttack);
            }

            if (healCooldown <= 0 && GetOwner().statLife < GetOwner().statLifeMax2 * 0.75f)
            {
                SetState(stateDropItem);
            }

            switch (state)
            {
                case stateIdle:
                    StateIdle();
                    break;
                case stateFollowPlayer:
                    StateFollowPlayer();
                    break;
                case stateAttack:
                    StateAttack();
                    break;
                case stateDropItem:
                    StateDropItem();
                    break;
            }

            RenderVisuals();
        }

        private void StateIdle()
        {
            if (Vector2.Distance(Projectile.Center, GetOwner().Center) > 150)
            {
                SetState(stateFollowPlayer);
                return;
            }

            float yHeight = -60 + 30 * (float)Math.Sin(timeSpentAlive / 22);
            var goalPosition = GetOwner().Center + new Vector2(GetOwner().direction * 60, yHeight);
            var distanceToGoal = Vector2.Distance(Projectile.Center, goalPosition);
            var goalDirection = Projectile.Center.DirectionTo(goalPosition);

            if (distanceToGoal > 20)
            {
                var inertia = 40f;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + goalDirection * distanceToGoal / 15) / inertia;
            } else
            {
                Projectile.velocity *= 0.9f;
            }

            if (IsThisClientTheProjectileOwner())
            {
                if (Projectile.velocity.Length() >= 1 || InputHelper.IsAnyKeyPressed())
                {
                    idleTime = 0;
                }
                else
                {
                    idleTime++;
                }
            }

            if (idleTime >= 60 * 30)
            {
                TriggerDialoguePearlIdle();
                idleTime -= 60 * 60;
            }
        }

        private void StateFollowPlayer()
        {
            var goalPosition = GetOwner().Center + new Vector2(GetOwner().direction * 60, -60);
            var distanceToGoal = Vector2.Distance(Projectile.Center, goalPosition);
            var goalDirection = Projectile.Center.DirectionTo(goalPosition);

            // Default movement parameters (here for attacking)
            float speed = 8f;
            float inertia = 20f;

            if (distanceToGoal > 1500f)
            {
                Projectile.position = goalPosition;
                Projectile.velocity /= 2;
                Projectile.netUpdate = true;
            }
            else if (distanceToGoal > 300f)
            {
                // Speed up the minion if it's away from the player
                speed = 12f;
                inertia = 60f;
            }
            else
            {
                // Slow down the minion if closer to the player
                speed = 6f;
                inertia = 40f;
            }

            if (distanceToGoal > 10f || Projectile.velocity.Length() > 10)
            {
                goalDirection *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + goalDirection) / inertia;
            }
            else
            {
                SetState(stateIdle);
            }
        }

        private void StateAttack()
        {
            if (foundTarget == null || foundTarget.life <= 0)
            {
                foundTarget = null;
                SetState(stateFollowPlayer);

                return;
            }

            var droneMP = GetOwnerModPlayer<PearlDronePlayer>();

            // Movement
            Vector2 goalPosition;
            if (Collision.CanHitLine(foundTarget.Center + new Vector2(0, -100), Projectile.width, Projectile.height, foundTarget.Center, 1, 1))
            {
                goalPosition = foundTarget.Center + new Vector2(0, -100);
            }
            else
            {
                goalPosition = foundTarget.Center;
            }

            var distanceToTarget = Vector2.Distance(Projectile.Center, foundTarget.Center);
            var goalDirection = Projectile.Center.DirectionTo(goalPosition);

            float speed;
            float inertia;

            switch (droneMP.PowerLevel)
            {
                case 2:
                    speed = 16f;
                    inertia = 30f;
                    break;
                case 3:
                    speed = 20f;
                    inertia = 25f;
                    break;
                case 4:
                    speed = 24f;
                    inertia = 20f;
                    break;
                default:
                    speed = 12f;
                    inertia = 40f;
                    break;
            }

            Projectile.velocity = (Projectile.velocity * (inertia - 1) + goalDirection * speed) / inertia;

            // Attacks
            if (sprinklerCooldown <= 0)
            {
                sprinklerCooldown = GetCooldownValue(sprinklerCooldownMax);
                SprinklerProjectile p = CreateChildProjectile<PearlDroneSprinklerProjectile>(
                    Projectile.Center,
                    Projectile.Center.DirectionTo(foundTarget.Center) * 20 + foundTarget.velocity,
                    droneMP.GetSprinklerDamage());
                WoomyMathHelper.AddRotationToVector2(p.Projectile.velocity, Main.rand.NextFloat(-15, 15));
            }

            if (droneMP.IsBurstBombEnabled && burstBombCooldown <= 0 && distanceToTarget < 200)
            {
                burstBombCooldown = GetCooldownValue(burstBombCooldownMax);
                CreateChildProjectile<PearlDroneBurstBomb>(
                    Projectile.Center,
                    Projectile.Center.DirectionTo(foundTarget.Center) * 20 + foundTarget.velocity,
                    droneMP.GetBurstBombDamage());
            }
        }

        private void StateDropItem()
        {
            var goalPosition = GetOwner().Center + new Vector2(GetOwner().direction, -60);
            var distanceToGoal = Vector2.Distance(Projectile.Center, goalPosition);
            var goalDirection = Projectile.Center.DirectionTo(goalPosition);

            if (distanceToGoal > 40)
            {
                var inertia = 40f;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + goalDirection * distanceToGoal / 10) / inertia;
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }

            if (!hasDroppedItem && distanceToGoal < 80 && IsThisClientTheProjectileOwner())
            {
                timeSpentInState = 0;
                hasDroppedItem = true;

                SpawnHearts();
            }

            if (hasDroppedItem && timeSpentInState >= 60)
            {
                SetState(stateFollowPlayer);
            }
        }

        private int GetCooldownValue(int baseCooldown)
        {
            var droneMP = GetOwnerModPlayer<PearlDronePlayer>();
            return (int)(baseCooldown * droneMP.GetAttackCooldownModifier());
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);

            if (speechDisplayTime > 0)
            {
                if (speechScale < 1)
                {
                    speechScale = MathHelper.Lerp(speechScale, 1, 0.2f);
                }
            }

            if (speechDisplayTime <= 0)
            {
                if (speechScale > 0)
                {
                    speechScale = MathHelper.Lerp(speechScale, 0, 0.2f);
                }
            }

            //var level = GetOwnerModPlayer<PearlDronePlayer>().PowerLevel;
            //Utils.DrawBorderString(Main.spriteBatch, $"Lv. {level}", Projectile.Center - Main.screenPosition + new Vector2(0, 32), Color.White, scale: 1f, anchorx: 0.5f, anchory: 0.5f);

            if (speechText.Length == 0) return;
            Utils.DrawBorderString(Main.spriteBatch, speechText, Projectile.Center - Main.screenPosition + new Vector2(0, -50), Color.Pink, scale: speechScale, anchorx: 0.5f, anchory: 0.5f);
        }

        private bool CheckPlayerActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<PearlDroneBuff>());
                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<PearlDroneBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void FindTarget(float maxTargetDistance)
        {
            bool shotsCanPassThroughLiquid = GetOwnerModPlayer<InkAccessoryPlayer>().hasThermalInkTank;
            var success = false;

            float closestDistance = maxTargetDistance;
            foreach (var npc in Main.ActiveNPCs)
            {
                float distance = GetOwner().Center.Distance(npc.Center);
                if (distance < closestDistance && IsTargetEnemy(npc) && (!npc.wet || shotsCanPassThroughLiquid))
                {
                    if (Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.Center, 1, 1)
                    || Collision.CanHitLine(GetOwner().Center, Projectile.width, Projectile.height, npc.Center, 1, 1))
                    {
                        closestDistance = distance;
                        foundTarget = npc;
                        success = true;
                    }
                }
            }

            if (!success) foundTarget = null;
        }

        private void RenderVisuals()
        {
            Projectile.direction = Math.Sign(Projectile.velocity.X);
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            int frameSpeed = 6;

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Lighting.AddLight(Projectile.Center, Color.HotPink.ToVector3() * 0.5f);

            if (timeSpentAlive % 4 == 0 && Main.rand.NextBool(10))
            {
                var dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(35, 35), DustID.PlatinumCoin,
                    Vector2.Zero,
                    255, Main.DiscoColor, Main.rand.NextFloat(0.8f, 1.6f));
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.noLight = true;
                dust.noLightEmittence = true;
            }
        }

        private void SpawnHearts()
        {
            int heartCount = 0;
            var currentLife = GetOwner().statLife;
            var maxLife = GetOwner().statLifeMax2;

            if (currentLife < maxLife * 0.25f)
            {
                heartCount = (int)Math.Max(3f, Math.Floor((float)maxLife / 20f * 0.25f));
                healCooldown = healCooldownMax;
            }
            else if (currentLife < maxLife * 0.5f)
            {
                heartCount = (int)Math.Max(2f, Math.Floor((float)maxLife / 20f * 0.15f));
                healCooldown = (int)(healCooldownMax * 0.75f);
            }
            else
            {
                heartCount = 1;
                healCooldown = (int)(healCooldownMax * 0.5f);
            }

            for (int i = 0; i < heartCount; i++)
            {
                Item.NewItem(Player.GetSource_None(), Projectile.Center, ItemID.Heart);
            }

            // Audio/visual
            for (int i = 0; i < 15; i++)
            {
                var d = Dust.NewDustPerfect(
                    Position: Projectile.Center,
                    Type: DustID.FireworksRGB,
                    Velocity: Main.rand.NextVector2CircularEdge(8, 8),
                    newColor: Color.HotPink,
                    Scale: Main.rand.NextFloat(1f, 1.5f));
                d.noGravity = true;
                d.velocity *= Main.rand.NextFloat(0.8f, 1.2f);
            }

            SoundHelper.PlayAudio(SoundID.Item4, volume: 0.5f, maxInstances: 10, position: Projectile.Center);
        }

        #region Speech methods

        private void PlaySpeechSample(string sample, int amountOfVariations)
        {
            SoundHelper.PlayAudio(sample + Main.rand.Next(1, amountOfVariations + 1).ToString(), volume: 0.5f, maxInstances: 1, position: Projectile.Center);
        }

        public void TriggerDialoguePearlAppears()
        {
            if (speechCooldownCurrent > 0) return;

            if (IsOwnerAnIrlFriend())
            {
                Speak(MemeSpawnQuotes());
            }
            else
            {
                Speak(NormalSpawnQuotes());
            }

            PlaySpeechSample(shoutSample, shoutSampleCount);
        }

        public void TriggerDialoguePearlIdle()
        {
            if (speechCooldownCurrent > 0) return;

            var list = IdleQuotes();
            if (list.Count == 0) return;

            Speak(list);
            PlaySpeechSample(talkSample, talkSampleCount);
        }

        public void TriggerDialoguePearlHealing()
        {
            if (speechCooldownCurrent > 0) return;

            if (Main.rand.NextBool(3))
            {
                var list = PearlGivesHealingQuotes();
                if (list.Count == 0) return;

                Speak(list);
                PlaySpeechSample(talkSample, talkSampleCount);
            }
        }

        public void TriggerDialoguePlayerKillsNpc(NPC npc)
        {
            if (speechCooldownCurrent > 0) return;

            if (Main.rand.NextBool(10))
            {
                var list = PlayerGetsKillQuotes(npc);
                if (list.Count == 0) return;

                Speak(list);
                PlaySpeechSample(talkSample, talkSampleCount);
            }
        }

        public void TriggerDialoguePearlKillsNpc(NPC npc)
        {
            if (speechCooldownCurrent > 0) return;

            if (Main.rand.NextBool(10))
            {
                var list = PearlGetsKillQuotes(npc);
                if (list.Count == 0) return;

                Speak(list);
                PlaySpeechSample(talkSample, talkSampleCount);
            }
        }

        public void TriggerDialoguePlayerActivatesSpecial(int specialItemId)
        {
            if (speechCooldownCurrent > 0) return;

            if (Main.rand.NextBool(2))
            {
                var list = PlayerActivatesSpecial(specialItemId);
                if (list.Count == 0) return;

                Speak(list);
                PlaySpeechSample(talkSample, talkSampleCount);
            }
        }

        private void Speak(string message)
        {
            speechCooldownCurrent = speechCooldownMax;
            speechText = message;
            speechDisplayTime = 90 + message.Length * 5;

            ChatHelper.SendChatToThisClient("<Pearl> " + message, Color.HotPink);
        }

        private void Speak(List<string> messages)
        {
            string message = messages[Main.rand.Next(messages.Count)];
            Speak(message);
        }

        #endregion

        #region Regular quotes

        private List<string> NormalSpawnQuotes()
        {
            return new List<string>
            {
                "Don't get cooked, stay off the hook!",
                $"Wassup {ownerName}!",
                $"What's good {ownerName}?",
                $"Yo {ownerName}!",
                "Booyah!",
                "'Sup!",
                "Yo yo! MC Princess on the mic!",
                "MC Princess in the house!",
                "Let's make some noise!",
                "Mic check, one-two!",
                "Let's get KRAKEN!",
                "NASTY! P#$&%!",
                "We're so back!",
            };
        }

        private List<string> IdleQuotes()
        {
            // General dialogue
            var owner = GetOwner();
            var strings = new List<string>
            {
                "Uuuuugh, I'm booored!",
                $"I miss my wife, {ownerName}.",
                "Ay, wake yo sleepy head up!",
                "Man, is there nuthin' to do?",
                "Dude, what's taking so long?",
                "Y'know, being a drone ain't all that bad.",
                "I wonder how Eight is doin'.",
                $"Yo! You good, {ownerName}?",
                "Dang, is the gameplay always this slow?",
                "...I'm gonna go play Squid Beatz. Shout if ya need me.",
                "Sure is boring around here!",
                "...",
                "I'm gonna grab some water-- wait...",

                "Dude, you should play Lethal Company with SimonTendo's mods sometime. It's good stuff.",
            };

            // Weapon based dialogue
            var heldItem = owner.HeldItem.ModItem;
            switch (heldItem)
            {
                case BaseDualie:
                    if (heldItem is VortexDualie)
                    {
                        strings.Add("Ay yo... You sure Sheldon made those dualies?");
                        strings.Add("Dang, those are some fresh lookin' dualies!");
                        strings.Add("Yo, can I borrow those dualies sometime?");
                    }

                    strings.Add("You like dualies too? Ohhh yeah!");
                    strings.Add("Not to brag, but I'm kiiinda goated with the dualies. You can be the second best.");
                    break;

                case BaseBrella:
                    strings.Add("Talk with 'Rina sometime, she loooves brellas.");

                    if (owner.ZoneRain)
                    {
                        strings.Add("Good thing you have a brella, this weather suuucks.");
                    }
                    break;

                case EelSplatana:
                    strings.Add("You think Frye names each of her eels?");
                    break;
            }

            // Area based dialogue
            if (owner.ZoneNormalUnderground || owner.ZoneNormalCaverns)
            {
                strings.Add("Mining away...");
            }
            else if (owner.ZoneCorrupt || owner.ZoneCrimson)
            {
                strings.Add($"Not gonna lie, {ownerName}, I'm not digging the vibes here.");
                strings.Add($"Why's everything so creepy and gross here?");
            }
            else if (owner.ZoneDungeon)
            {
                strings.Add($"What's it like to have bones, {ownerName}?");
            }
            else if (owner.ZoneGraveyard)
            {
                strings.Add($"Good thing you can respawn, {ownerName}. You need it.");
                strings.Add("Cod damn, I'm feeling like a dead fish around these parts.");
            }

            return strings;
        }

        private List<string> PlayerGetsKillQuotes(NPC npc)
        {
            var strings = new List<string>();

            switch (npc.type)
            {
                case NPCID.Squid:
                    strings.Add("Nooo... Not John!!");
                    strings.Add("How could you do this to Tim?!");
                    break;

                case NPCID.Shark:
                    strings.Add("...Don't tell Shiver what you just did.");
                    break;
            }

            return strings;
        }

        private List<string> PearlGetsKillQuotes(NPC npc)
        {
            var npcName = npc.FullName.ToLower();
            var strings = new List<string>
            {
                "POW!",
                "BAM!",
                "SPLAT!",
                "Got 'em!",
                "How's that!",
                "K.O!",
                "Oh yeah!",
                "I'm straight up fishious!",
                $"See my moves, {ownerName}?",
                $"Check me out, {ownerName}!",
            };

            return strings;
        }

        private List<string> PearlGivesHealingQuotes()
        {
            return new List<string>
            {
                $"Have this, {ownerName}!",
                "I gotchu!",
                "Careful!",
                "Need a heal?",
                "This way! Heal!",
                "Get a life!",
                "Heal up!",
            };
        }

        private List<string> PlayerActivatesSpecial(int specialItemId)
        {
            var strings = new List<string>();

            if (specialItemId == ModContent.ItemType<TrizookaSpecial>())
            {
                strings.Add("Fire away!");
                strings.Add("BOOM! BOOM! BOOM!");
                strings.Add("React to that, chumps!");
            }

            if (specialItemId == ModContent.ItemType<KillerWail>())
            {
                strings.Add("Get your vocal chords ready!");
                strings.Add("Hey, that's my move!");
                strings.Add("Final smash!");
            }

            if (specialItemId == ModContent.ItemType<UltraStamp>())
            {
                strings.Add($"Squash them, {ownerName}!");
                strings.Add($"Hammerhead time!");
                strings.Add($"Sic 'em, {ownerName}!");
            }

            return strings;
        }

        #endregion

        #region Meme quotes

        private bool IsOwnerAnIrlFriend()
        {
            string name = ownerName.ToLower();
            return name.Equals("isa")
                || name.Equals("isey")
                || name.Equals("joel")
                || name.Equals("fieryice")
                || name.Equals("daan")
                || name.Equals("daanbanaan")
                || name.Equals("mrbraadworst")
                || name.Equals("hanna")
                || name.Equals("fenneathalia");
        }

        private List<string> MemeSpawnQuotes()
        {
            var name = ownerName.ToLower();
            if (name.Equals("isa") || name.Equals("isey"))
            {
                return new List<string>
                {
                    "I'm so glad 'Rina doesn't let me sleep on the couch.",
                    "Hey, what's this rabbies thing about?",
                };
            }

            if (name.Equals("joel") || name.Equals("fieryice"))
            {
                return new List<string>
                {
                    "Let's Spheal the deal!",
                    "Yo! I'm goated with the sauce!",
                    "When you're out of squid, submerge in it!",
                };
            }

            if (name.Equals("daan") || name.Equals("daanbanaan") || name.Equals("mrbraadworst"))
            {
                return new List<string>
                {
                    "Rauru! Rauru!",
                    "IT'S MEEE!! THE DEVIL!!",
                };
            }

            if (name.Equals("hanna") || name.Equals("fenneathalia"))
            {
                return new List<string>
                {
                    "YES! Burn all the schoolwork!",
                    "Guys, where is Hanna?!",
                };
            }

            return NormalSpawnQuotes();
        }

        #endregion
    }
}
