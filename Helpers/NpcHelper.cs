using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Helpers;

internal class NpcHelper
{
    public static bool IsTargetABossMinion(NPC target)
    {
        switch (target.type)
        {
            case NPCID.BlueSlime:
            case NPCID.SlimeSpiked:
            case NPCID.ServantofCthulhu:
            case NPCID.Creeper:
            case NPCID.Bee:
            case NPCID.BeeSmall:
            case NPCID.SkeletronHand:

            case NPCID.TheHungry:
            case NPCID.TheHungryII:
            case NPCID.LeechBody:
            case NPCID.LeechHead:
            case NPCID.LeechTail:

            case NPCID.Probe:
            case NPCID.PrimeCannon:
            case NPCID.PrimeSaw:
            case NPCID.PrimeLaser:
            case NPCID.PrimeVice:

            case 658: // Crystal Slime - Summoned by Queen Slime
            case 659: // Bouncy Slime
            case 660: // Heavenly Slime

            case NPCID.Sharkron:
            case NPCID.Sharkron2:
                return true;
        }

        return false;
    }

    public static bool IsTargetAWormSegment(NPC target)
    {
        int n = target.type;
        if ((n >= NPCID.EaterofWorldsHead && n <= NPCID.EaterofWorldsTail)
        || (n >= NPCID.TheDestroyer && n <= NPCID.TheDestroyerTail)
        || (n >= NPCID.GiantWormHead && n <= NPCID.GiantWormTail)
        || (n >= NPCID.DiggerHead && n <= NPCID.DiggerTail)
        || (n >= NPCID.DevourerHead && n <= NPCID.DevourerTail)
        || (n >= NPCID.SeekerHead && n <= NPCID.SeekerTail)
        || (n >= NPCID.TombCrawlerHead && n <= NPCID.TombCrawlerTail)
        || (n >= NPCID.DuneSplicerHead && n <= NPCID.DuneSplicerTail)
        || (n >= NPCID.WyvernHead && n <= NPCID.WyvernTail)
        || (n >= NPCID.BoneSerpentHead && n <= NPCID.BoneSerpentTail))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// See also <seealso href="https://terraria.wiki.gg/wiki/Category:Projectile_NPCs"/>
    /// </summary>
    public static bool IsTargetAProjectile(NPC target)
    {
        switch (target.type)
        {
            case NPCID.AncientLight:
            case NPCID.BurningSphere:
            case NPCID.ChaosBall:
            case NPCID.DetonatingBubble:
            case NPCID.GiantFungiBulb:
            case NPCID.MoonLordLeechBlob:
            case NPCID.VileSpit:
            case NPCID.WaterSphere:
                return true;
        }

        return false;
    }
}
