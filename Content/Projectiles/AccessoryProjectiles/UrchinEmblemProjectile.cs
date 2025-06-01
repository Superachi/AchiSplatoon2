using Terraria;
using AchiSplatoon2.Content.Items.Accessories.RushAttacks;

namespace AchiSplatoon2.Content.Projectiles.AccessoryProjectiles
{
    internal class UrchinEmblemProjectile : SquidAuraProjectile
    {
        protected override void AfterSpawn()
        {
            base.AfterSpawn();

            Projectile.damage = UrchinEmblem.GetAttackDamage();
            Projectile.ArmorPenetration = 100;
            Projectile.knockBack = 8;

            if (Owner.immuneTime == 0)
            {
                Owner.immune = true;
                Owner.immuneTime = 18;
                Owner.immuneNoBlink = true;
            }
        }

        public override void AI()
        {
            base.AI();

            switch (state)
            {
                case stateSpawn:

                    if (drawAlpha < 1f)
                    {
                        drawAlpha += 0.1f;
                    }

                    if (timeSpentInState > 6)
                    {
                        hitboxSize += 10;
                    }
                    else
                    {
                        hitboxSize -= 4;
                    }

                    if (timeSpentInState > 12)
                    {
                        AdvanceState();
                    }

                    break;

                case stateFollowPlayer:
                    hitboxSize += 8;

                    if (timeSpentInState > 4)
                    {
                        AdvanceState();
                    }
                    break;

                case stateDisappear:
                    hitboxSize += 3;

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
