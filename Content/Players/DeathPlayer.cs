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
                "squished",
                "squidbagged",
                "wiped out",
                "splashed upon",
            };

            string template = $"{Player.name} got {Main.rand.NextFromCollection(deathVerbs)}";

            if (NetHelper.IsPlayerSameAsLocalPlayer(Player))
            {
                damageSource.TryGetCausingEntity(out Entity entity);
                switch (entity)
                {
                    case NPC npc:
                        template += $" by {npc.FullName}";
                        break;
                    case Projectile projectile:
                        template += $" by {projectile.Name}";
                        break;
                }
            }

            template += "!";
            damageSource = PlayerDeathReason.ByCustomReason(template);

            return true;
        }
    }
}
