using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AchiSplatoon2.Content.Buffs;
using System.Collections.Generic;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using Terraria.Map;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneMinion : BaseProjectile
    {
        private int idleTime = 0;
        private const int stateIdle = 0;
        private const int stateFollowPlayer = 1;
        private const int stateAttack = 2;

        private int speechCooldownCurrent = 0;
        private int speechCooldownMax = 600;
        private int speechDisplayTime = 0;
        private float speechScale = 0f;
        private string speechText = "";

        private int sprinklerCooldown = 0;
        private int sprinklerCooldownMax = 15;
        private int burstBombCooldown = 0;
        private int burstBombCooldownMax = 300;

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
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            switch (state)
            {
                case stateIdle:
                    idleTime = 0;
                    break;
                case stateFollowPlayer:
                    break;
                case stateAttack:
                    break;
            }
        }

        private void DeductCooldowns()
        {
            if (speechCooldownCurrent > 0) speechCooldownCurrent--;
            if (speechDisplayTime > 0) speechDisplayTime--;

            if (sprinklerCooldown > 0) sprinklerCooldown--;
            if (burstBombCooldown > 0) burstBombCooldown--;
        }

        public override void AI()
        {
            if (!CheckPlayerActive(GetOwner()))
            {
                return;
            }

            DeductCooldowns();

            // Greet the player
            if (timeSpentAlive == 60 && Main.rand.NextBool(2))
            {
                if (IsOwnerAnIrlFriend())
                {
                    Speak(MemeSpawnQuotes());
                }
                else
                {
                    Speak(NormalSpawnQuotes());
                }
            }

            if (timeSpentAlive % 20 == 0)
            {
                FindTarget(1000f);
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
            }

            RenderVisuals();
        }

        public void StateIdle()
        {
            if (foundTarget != null)
            {
                SetState(stateAttack);
                return;
            }

            if (Vector2.Distance(Projectile.Center, GetOwner().Center) > 150)
            {
                SetState(stateFollowPlayer);
                return;
            }

            var goalPosition = GetOwner().Center + new Vector2(GetOwner().direction * 60, -60);
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
                } else
                {
                    idleTime++;
                }
            }

            if (idleTime >= 60 * 20)
            {
                Speak(IdleQuotes());
                idleTime -= 60 * 60;
            }
        }

        public void StateFollowPlayer()
        {
            if (foundTarget != null)
            {
                SetState(stateAttack);
                return;
            }

            var goalPosition = GetOwner().Center + new Vector2(GetOwner().direction * 60, -60);
            var distanceToGoal = Vector2.Distance(Projectile.Center, goalPosition);
            var goalDirection = Projectile.Center.DirectionTo(goalPosition);

            // Default movement parameters (here for attacking)
            float speed = 8f;
            float inertia = 20f;

            if (distanceToGoal > 1000f)
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
                speed = 4f;
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

        public void StateAttack()
        {
            if (foundTarget == null || foundTarget.life <= 0)
            {
                foundTarget = null;
                SetState(stateFollowPlayer);

                return;
            }

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

            float speed = 12f;
            float inertia = 45f;

            Projectile.velocity = (Projectile.velocity * (inertia - 1) + goalDirection * speed) / inertia;

            var droneMP = GetOwnerModPlayer<PearlDronePlayer>();

            if (sprinklerCooldown <= 0)
            {
                sprinklerCooldown = GetCooldownValue(sprinklerCooldownMax);
                SprinklerProjectile p = CreateChildProjectile<SprinklerProjectile>(
                    Projectile.Center,
                    Projectile.Center.DirectionTo(foundTarget.Center) * 20 + foundTarget.velocity,
                    droneMP.GetSprinklerDamage());
                WoomyMathHelper.AddRotationToVector2(p.Projectile.velocity, Main.rand.NextFloat(-15, 15));
            }

            if (burstBombCooldown <= 0 && distanceToTarget < 200)
            {
                burstBombCooldown = GetCooldownValue(burstBombCooldownMax);
                CreateChildProjectile<PearlDroneBurstBomb>(
                    Projectile.Center,
                    Projectile.Center.DirectionTo(foundTarget.Center) * 20 + foundTarget.velocity,
                    droneMP.GetBurstBombDamage());
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
            var success = false;

            float closestDistance = maxTargetDistance;
            foreach (var npc in Main.ActiveNPCs)
            {
                float distance = GetOwner().Center.Distance(npc.Center);
                if (distance < closestDistance && IsTargetEnemy(npc) && !npc.wet)
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

            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.8f);

            if (timeSpentAlive % 4 == 0 && Main.rand.NextBool(10))
            {
                var dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(35, 35), DustID.PlatinumCoin,
                    Vector2.Zero,
                    255, Main.DiscoColor, Main.rand.NextFloat(0.8f, 1.6f));
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
            }
        }

        #region Speech methods

        public void TriggerDialoguePlayerKillsNpc(NPC npc)
        {
            if (Main.rand.NextBool(10))
            {
                var list = PlayerGetsKillQuotes(npc);
                if (list.Count == 0) return;

                Speak(list);
            }
        }

        public void TriggerDialoguePearlKillsNpc(NPC npc)
        {
            if (Main.rand.NextBool(10))
            {
                var list = PearlGetsKillQuotes(npc);
                if (list.Count == 0) return;

                Speak(list);
            }
        }

        private void Speak(string message)
        {
            if (speechCooldownCurrent > 0) return;
            speechCooldownCurrent = speechCooldownMax;
            speechText = message;
            speechDisplayTime = 60 + message.Length * 5;
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
                $"Wassup {GetOwner().name}!",
                $"What's good {GetOwner().name}?",
                $"Yo {GetOwner().name}!",
                "Booyah!",
                "'Sup!",
                "Yo yo! MC Princess on the mic!",
                "MC Princess in the house!",
                "Let's make some noise!",
                "Yo! I'm never shook, 'cause I'm OFF THE HOOK!",
                "Mic check, one-two!",
                "Let's get KRAKEN!",
                "NASTY! P#$&%!",
            };
        }

        private List<string> IdleQuotes()
        {
            var strings = new List<string>
            {
                "Uuuuugh, I'm booored!",
                $"I miss my wife, {GetOwner().name}.",
                "Ay, wake yo sleepy head up!",
                "Man, is there nuthin' to do?",
                "Dude, what's taking so long?",
                "Y'know, being a drone ain't all that bad.",
                "I wonder how Eight is doin'.",
                $"Yo! You good, {GetOwner().name}?",
                "Dang, is the gameplay always this slow?",
                "...I'm gonna go play Squid Beatz. Shout if ya need me.",
            };

            var heldItem = GetOwner().HeldItem.ModItem;
            switch (heldItem)
            {
                case BaseDualie:
                    if (heldItem is VortexDualie)
                    {
                        strings.Add("Ay yo... You sure Sheldon made those dualies?");
                        strings.Add("Dang, those are some fresh lookin' dualies!");
                        strings.Add("Yo, can I borrow those dualies sometime?");
                    }

                    strings.Add("You like dualies too? Shell yeah!");
                    break;

                case BaseBrella:
                    strings.Add("Talk with 'Rina sometime, she loooves brellas.");
                    break;

                case EelSplatana:
                    strings.Add("You think Frye names each of her eels?");
                    break;
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
                    strings.Add("Uhh... Don't let Shiver know what you did.");
                    strings.Add("Let's hope Shiver didn't see that.");
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
                $"See my moves, {GetOwner().name}?",
            };

            if (npcName.Contains("slime"))
            {
                strings.Add("Squish!");
                strings.Add("Don't be made of gel if you can't handle the heat!");
            }

            return strings;
        }

        private List<string> PearlGivesHealing()
        {
            return new List<string>
            {
                $"Have this, {GetOwner().name}!",
                $"Chin up, {GetOwner().name}!",
                "I gotchu!",
                "Get yo life up!",
                "Marina told me to give you this!",
                $"I need you alive and well, {GetOwner().name}!",
            };
        }

        #endregion

        #region Meme quotes

        private bool IsOwnerAnIrlFriend()
        {
            string ownerName = GetOwner().name.ToLower();
            return ownerName.Equals("isa")
                || ownerName.Equals("isey")
                || ownerName.Equals("joel")
                || ownerName.Equals("fieryice")
                || ownerName.Equals("daan")
                || ownerName.Equals("daanbanaan")
                || ownerName.Equals("mrbraadworst")
                || ownerName.Equals("hanna")
                || ownerName.Equals("fenneathalia");
        }

        private List<string> MemeSpawnQuotes()
        {
            var name = GetOwner().name.ToLower();
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
