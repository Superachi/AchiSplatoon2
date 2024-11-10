using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BlasterProjectiles
{
    internal class BlasterExplosion : BaseProjectile
    {
        private int _blastRadius;
        private List<int> _targetsToIgnore = new List<int>();

        public void SetProperties(int radius, List<int> targetsToIgnore)
        {
            _blastRadius = radius;
            _targetsToIgnore = targetsToIgnore;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 6;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            enablePierceDamagefalloff = false;
            wormDamageReduction = true;

            if (IsThisClientTheProjectileOwner())
            {
                var e = new ExplosionDustModel(_dustMaxVelocity: 20, _dustAmount: 40, _minScale: 2, _maxScale: 4, _radiusModifier: _blastRadius);
                CreateExplosionVisual(e);

                Projectile.Resize(_blastRadius, _blastRadius);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            // Skip targets already hit by the blaster shot (pre-explosion)

            foreach (var id in _targetsToIgnore)
            {
                if (target.whoAmI == id)
                {
                    return false;
                }
            }

            if (!CanHitNPCWithLineOfSight(target) && Projectile.Center.Distance(target.Center) > _blastRadius / 3)
            {
                return false;
            }

            return base.CanHitNPC(target);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            _targetsToIgnore.Add(target.whoAmI);
            modifiers.HitDirectionOverride = Math.Sign(target.position.X - GetOwner().position.X);
            base.ModifyHitNPC(target, ref modifiers);
        }

        protected override void AfterKill(int timeLeft)
        {
            if (IsThisClientTheProjectileOwner())
            {
                var accMP = GetOwner().GetModPlayer<AccessoryPlayer>();
                if (accMP.hasFieryPaintCan) accMP.SetBlasterBuff(_targetsToIgnore.Count != 0);
            }
        }
    }
}
