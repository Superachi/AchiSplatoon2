using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseProjectile : ModProjectile
    {
        protected InkColor inkColor = InkColor.Blue;
        protected bool isVisible = false;
        protected float delayUntilVisible = 0f;

        public void Initialize(InkColor color, bool visible, float visibleDelay)
        {
            inkColor = color;
            isVisible = visible;
            delayUntilVisible = visibleDelay;
        }
    }
}
