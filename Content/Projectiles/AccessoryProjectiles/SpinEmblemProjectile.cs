using AchiSplatoon2.Content.Items.Accessories.RushAttacks;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class SpinEmblemProjectile : SquidAuraProjectile
    {
        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            drawAlpha = 1;
            hitboxSize = 120;

            var heldItem = Owner.HeldItem;
            if (heldItem.ModItem is BaseDualie dualie)
            {
                Projectile.damage = heldItem.damage * 3;
            }
            else
            {
                Projectile.damage = SpinEmblem.DefaultDamage;
            }

            Projectile.ArmorPenetration = 100;
            Projectile.knockBack = 8;
        }

        public override void AI()
        {
            base.AI();

            switch (state)
            {
                case stateSpawn:
                    AdvanceState();
                    break;

                case stateFollowPlayer:
                    if (timeSpentInState > 16)
                    {
                        AdvanceState();
                    }
                    break;

                case stateDisappear:
                    hitboxSize += 10;

                    if (drawAlpha > 0f)
                    {
                        drawAlpha -= 0.1f;
                    }

                    if (drawAlpha <= 0f)
                    {
                        Projectile.Kill();
                        return;
                    }

                    break;
            }
        }
    }
}
