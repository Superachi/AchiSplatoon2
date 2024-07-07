using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.SplatlingProjectiles.Charges;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AchiSplatoon2.Content.Projectiles.SplatlingProjectiles
{
    internal class HeavySplatlingProjectile : BaseProjectile
    {
        private float delayUntilFall = 20f;
        private float fallSpeed = 0.1f;

        private bool firedWithCrayonBox = false;
        private bool countedForBurst = false;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void AfterSpawn()
        {
            Initialize();
            PlayShootSound();
            firedWithCrayonBox = GetOwner().GetModPlayer<InkAccessoryPlayer>().hasCrayonBox;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.98f;
                }
            }

            Color dustColor = GenerateInkColor();
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            for (int i = 0; i < 3; i++)
            {
                // Vector2 spawnPosition = Projectile.oldPosition != Vector2.Zero ? Vector2.Lerp(Projectile.position, Projectile.oldPosition, Main.rand.NextFloat()) : Projectile.position;
                var dust = Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
                dust.alpha = 64;
            }
        }

        private HeavySplatlingCharge? GetParentModProjectile()
        {
            var p = GetParentProjectile(parentIdentity);
            if (p.ModProjectile is HeavySplatlingCharge)
            {
                return (HeavySplatlingCharge)p.ModProjectile;
            }
            return null;
        }

        private void ResetCrayonBoxCombo(string message)
        {
            HeavySplatlingCharge parent = GetParentModProjectile();

            if (parent != null)
            {
                if (parent.barrageTarget != -1)
                {
                    if (parent.barrageCombo > 5)
                    {
                        CombatTextHelper.DisplayText($"{message}Combo: {parent.barrageCombo}x", GetOwner().Center);
                    }

                    parent.barrageTarget = -1;
                    parent.barrageCombo = 0;
                }
            }
        }

        private void PlayShootSound()
        {
            PlayAudio("SplatlingShoot", volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Reset CrayonBox barrage combo when missing target
            if (!countedForBurst)
            {
                ResetCrayonBoxCombo("Miss! ");
            }

            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = (Projectile.velocity.X + random) * -0.5f;
                float velY = (Projectile.velocity.Y + random) * -0.5f;
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (firedWithCrayonBox && target.life <= damageDone)
            {
                ResetCrayonBoxCombo("");
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (firedWithCrayonBox)
            {
                if (countedForBurst) return;
                modifiers.DamageVariationScale *= 0f;

                var proj = GetParentModProjectile();
                if (proj != null)
                {
                    if (proj.barrageTarget != target.whoAmI)
                    {
                        ResetCrayonBoxCombo("");
                        proj.barrageTarget = target.whoAmI;
                    } else
                    {
                        proj.barrageCombo++;
                    }

                    modifiers.FlatBonusDamage += proj.barrageCombo * CrayonBox.DamageIncrement;
                    if (proj.ChargedAmmo == 0) { ResetCrayonBoxCombo(""); }
                }

                countedForBurst = true;
            }

            base.ModifyHitNPC(target, ref modifiers);
        }

        // Netcode
        protected override void NetSendInitialize(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
        }
        protected override void NetReceiveInitialize(BinaryReader reader)
        {
            base.NetReceiveInitialize(reader);
            Projectile.velocity = reader.ReadVector2();

            PlayShootSound();
        }
    }
}