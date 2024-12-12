using AchiSplatoon2.Helpers;
using MonoMod;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class DeathPlayer : ModPlayer
    {
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            playSound = true;
            genGore = true;

            List<string> deathVerbs = new List<string>
            {
                "splatted",
                "splattered",
                "squished",
                "squidbagged",
                "wiped out",
                "splashed upon",
            };

            bool useCustomTemplate = false;
            string template = $"{Player.name} got {Main.rand.NextFromCollection(deathVerbs)}";

            if (NetHelper.IsPlayerSameAsLocalPlayer(Player))
            {
                template += $" by ";

                damageSource.TryGetCausingEntity(out Entity entity);
                switch (entity)
                {
                    case NPC npc:
                        if (DoesWordStartWithVowel(npc.FullName) && !npc.boss)
                        {
                            template += "an ";
                        }
                        else
                        {
                            template += "a ";
                        }

                        template += npc.FullName;
                        useCustomTemplate = true;
                        break;

                    case Projectile projectile:
                        if (DoesWordStartWithVowel(projectile.Name))
                        {
                            template += "an ";
                        }
                        else
                        {
                            template += "a ";
                        }

                        template += projectile.Name;
                        useCustomTemplate = true;
                        break;
                }
            }

            template += "!";

            if (useCustomTemplate)
            {
                damageSource = PlayerDeathReason.ByCustomReason(template);
            }

            return true;
        }

        private bool DoesWordStartWithVowel(string word)
        {
            char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u' };

            foreach (char vowel in vowels)
            {
                if (word.ToLower().StartsWith(vowel))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
