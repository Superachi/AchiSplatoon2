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

namespace AchiSplatoon2.Content.Projectiles.Minions.PearlDrone
{
    internal class PearlDroneMinion : BaseProjectile
    {
        private int speechCooldownCurrent = 0;
        private int speechCooldownMax = 600;
        private int speechDisplayTime = 0;
        private float speechScale = 0f;
        private string speechText = "";

        private int idleTime = 0;
        private const int stateIdle = 0;
        private const int stateFollowPlayer = 1;
        private const int stateAttack = 2;

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

        private void Speak(string message)
        {
            if (speechCooldownCurrent > 0) return;
            speechCooldownCurrent = speechCooldownMax;
            speechText = message;
            speechDisplayTime = 30 + message.Length * 10;

            //AdvancedPopupRequest request = default;
            //    request.Text = message;
            //    request.DurationInFrames = 30 + message.Length * 10;
            //    request.Velocity = new Vector2(0, -10);
            //    request.Color = Color.Pink;
            //    PopupText.NewText(request, Projectile.Center);

            // Main.NewText($"<Pearl> {message}", Color.Pink);
        }

        private void Speak(List<string> messages)
        {
            string message = messages[Main.rand.Next(messages.Count)];
            Speak(message);
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

        public override void AI()
        {
            Player owner = GetOwner();

            if (!CheckActive(owner))
            {
                return;
            }

            if (speechCooldownCurrent > 0) speechCooldownCurrent--;
            if (speechDisplayTime > 0) speechDisplayTime--;

            // Greet the player
            if (timeSpentAlive == 60 && Main.rand.NextBool(3))
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
                // Projectile.velocity = goalDirection * distanceToGoal / 30;
            } else
            {
                Projectile.velocity *= 0.9f;
            }

            if (IsThisClientTheProjectileOwner())
            {
                if (Projectile.velocity.Length() <= 1 && InputHelper.IsAnyKeyPressed())
                {
                    idleTime++;
                }
            }

            if (idleTime >= 60 * 15)
            {
                Speak(IdleQuotes());
                idleTime -= 60 * 60;
            }
        }

        public void StateFollowPlayer()
        {
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

            if (speechText.Length == 0) return;
            Utils.DrawBorderString(Main.spriteBatch, speechText, Projectile.Center - Main.screenPosition + new Vector2(0, -50), Color.Pink, scale: speechScale, anchorx: 0.5f, anchory: 0.5f);
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player owner)
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
            DebugHelper.PrintInfo($"Pearl killed {npc.FullName}");
        }

        #region Regular quotes

        private List<string> NormalSpawnQuotes()
        {
            return new List<string>
            {
                "Don't get cooked... Stay off the hook!",
                $"Wassup {GetOwner().name}!",
                $"What's good {GetOwner().name}?",
                $"Yo {GetOwner().name}!",
                "Booyah!",
                "'Sup.",
                "MC Princess on the mic!",
                "Get hyped!",
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
            };

            switch (GetOwner().HeldItem.ModItem)
            {
                case VortexDualie:
                    strings.Add("Yooo... You gotta lend me those dualies one day!");
                    break;

                case BaseDualie:
                    strings.Add("I see ya like dualies too, niiice.");
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
            var npcName = npc.FullName.ToLower();
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
