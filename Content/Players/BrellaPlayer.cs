using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Prefixes.BrellaPrefixes;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class BrellaPlayer : ModPlayer
    {
        public float shieldLife;
        public float shieldLifeMax;

        public int shieldCooldown;
        public int shieldCooldownMax;
        public bool shieldAvailable = true;
        public int timeSinceLastDeflect = 0;

        public int damageCooldown = 0;
        public int damageCooldownMax = 6;

        private InventoryPlayer inventoryPlayer => Player.GetModPlayer<InventoryPlayer>();

        public void DamageShield(int damage)
        {
            if (!shieldAvailable || damageCooldown > 0 || damage == 0) return;
            timeSinceLastDeflect = shieldCooldownMax / 2;
            damageCooldown = damageCooldownMax;

            float damageMod = 1f;
            if (Main.masterMode)
            {
                damageMod = 3f;
            }
            else if (Main.expertMode)
            {
                damageMod = 2f;
            }

            var finalDamage = (int)(damage * damageMod);
            shieldLife -= finalDamage;

            if (shieldLife < 1)
            {
                SoundHelper.PlayAudio("Brellas/BrellaBreak", volume: 1f, maxInstances: 5);
                CombatTextHelper.DisplayText($"Brella broke!", Player.Center, Color.HotPink);

                if (Player.GetModPlayer<AccessoryPlayer>().hasMarinatedNecklace)
                {
                    shieldCooldown = (int)(shieldCooldownMax * MarinatedNecklace.RecoverTimeModifier);
                }
                else
                {
                    shieldCooldown = shieldCooldownMax;
                }

                shieldAvailable = false;

                Player.immuneTime = 60;
                Player.immune = true;
                Player.immuneNoBlink = false;
            }
            else
            {
                SoundHelper.PlayAudio("Brellas/BrellaDeflect", volume: 0.4f, pitchVariance: 0.4f, maxInstances: 5);
                DisplayBrellaLife();
            }
        }

        private void BrellaRecoveryDust()
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Player.Center, 0, 0, DustID.GoldCoin, 0, 0, 0, default, 2);
            }
        }

        private void DisplayBrellaLife()
        {
            var shieldPercentage = shieldLife / shieldLifeMax;
            var textColor = Color.LimeGreen;
            if (shieldPercentage < 1f)
            {
                textColor = ColorHelper.LerpBetweenColorsPerfect(Color.HotPink, Color.White, shieldPercentage);
            }

            var textPosition = Player.Center;
            if (!Player.ItemTimeIsZero && inventoryPlayer.HeldModItem() is BaseBrella)
            {
                textPosition = new Vector2(Player.Center.X + Math.Sign(Player.direction) * 80, Player.Center.Y);
            }

            CombatTextHelper.DisplayText($"{(int)(shieldPercentage * 100)}%", textPosition, textColor);
        }

        public override void ResetEffects()
        {
            if (inventoryPlayer.HeldModItem() is not BaseBrella && shieldAvailable)
            {
                shieldLife = 1;
                shieldLifeMax = 1;
                shieldCooldown = 0;
                shieldCooldownMax = 20;
            }
        }

        private void GetBrellaStats()
        {
            if (!inventoryPlayer.HasHeldItemChanged()) return;
            if (inventoryPlayer.HeldModItem() is not BaseBrella) return;

            var brellaData = (BaseBrella)inventoryPlayer.HeldModItem();

            shieldLife = brellaData.ShieldLife;
            shieldLifeMax = brellaData.ShieldLife;
            shieldCooldown = Math.Max(shieldCooldown, 0);
            shieldCooldownMax = brellaData.ShieldCooldown;

            var prefix = PrefixHelper.GetWeaponPrefixById(Player.HeldItem.prefix);
            if (prefix is BaseBrellaPrefix brellaPrefix)
            {
                shieldLife = (int)(shieldLife * brellaPrefix.ShieldLifeModifier.NormalizePrefixMod());
                shieldCooldownMax = (int)(shieldLife * brellaPrefix.ShieldCooldownModifier.NormalizePrefixMod());
            }
        }

        public override void PreUpdate()
        {
            GetBrellaStats();

            if (Player.dead) return;

            if (damageCooldown > 0) damageCooldown--;

            if (shieldCooldown > 0)
            {
                shieldCooldown--;
                if (shieldCooldown == 0)
                {
                    shieldLife = shieldLifeMax;
                    shieldAvailable = true;
                    timeSinceLastDeflect = 0;

                    DisplayBrellaLife();
                    BrellaRecoveryDust();
                    SoundHelper.PlayAudio("Brellas/BrellaRecover", volume: 0.8f, maxInstances: 5);
                }

                return;
            }

            if (timeSinceLastDeflect > 0 && shieldAvailable)
            {
                timeSinceLastDeflect--;
                if (timeSinceLastDeflect == 0)
                {
                    shieldLife = shieldLifeMax;

                    DisplayBrellaLife();
                    BrellaRecoveryDust();
                    SoundHelper.PlayAudio(SoundID.MaxMana, 0.3f);
                }
            }
        }
    }
}
