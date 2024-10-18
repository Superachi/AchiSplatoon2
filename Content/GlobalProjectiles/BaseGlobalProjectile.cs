using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalProjectiles
{
    internal class BaseGlobalProjectile : GlobalProjectile
    {
        public bool deflected = false;
        public override bool InstancePerEntity => true;
    }
}
