using AchiSplatoon2.Content.CustomConditions;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.Content.Items.Accessories.RushAttacks;
using AchiSplatoon2.Content.Items.Armors.Vanity;
using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Content.Items.Consumables.ColorVials.Gradients;
using AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs;
using AchiSplatoon2.Content.Items.Consumables.Potions;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Unclassed;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.StaticData;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles
{
    internal class ShellOutCapsuleProjectile : BaseProjectile
    {
        private const int _stateFly = 0;
        private const int _stateRoll = 1;
        private const int _stateUpgrade = 2;
        private const int _statePop = 3;

        private const int _blueTier = 0;
        private const int _purpleTier = 1;
        private const int _redTier = 2;
        private const int _goldTier = 3;

        private Color _blueColor;
        private Color _purpleColor;
        private Color _redColor;
        private Color _goldColor;

        private int _currentTier;
        private bool _doneUpgrading;
        private bool _maxUpgrade;
        private int _delayUntilNextUpgrade;

        // Physics
        private float _gravity;
        private float _groundFriction;
        private bool _hasBounced;
        private int _lastBounceTimestamp;

        // Audio/Visual
        private SlotId throwAudio;
        private float _drawScale;
        private float _drawScaleGoal;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 1;

            Projectile.tileCollide = true;
            Projectile.friendly = false;
        }

        protected override void AfterSpawn()
        {
            _gravity = 0.15f;
            _groundFriction = 0.965f;

            _blueColor = new Color(0, 170, 255, 255);
            _purpleColor = new Color(85, 0, 255, 255);
            _redColor = new Color(255, 0, 45, 255);
            _goldColor = new Color(255, 170, 0, 255);

            _drawScale = 1f;
            _drawScaleGoal = _drawScale;
            _lastBounceTimestamp = timeSpentAlive - 10;

            if (IsThisClientTheProjectileOwner())
            {
                var angle = Owner.Center.DirectionTo(Main.MouseWorld) * 10;
                float distance = Vector2.Distance(Owner.Center, Main.MouseWorld);
                float velocityMod = MathHelper.Clamp(distance / 250f, 0.3f, 1f) + 0.2f;

                Projectile.velocity = angle * velocityMod;
                NetUpdate(ProjNetUpdateType.SyncMovement);
            }

            throwAudio = PlayAudio(SoundPaths.SplatBombThrow.ToSoundStyle(), pitch: 0.2f);
        }

        protected override void SetState(int targetState)
        {
            base.SetState(targetState);

            switch (state)
            {
                case _stateRoll:
                    if (SoundEngine.TryGetActiveSound(throwAudio, out var sound))
                    {
                        sound.Stop();
                    }
                    break;

                case _stateUpgrade:
                    break;

                case _statePop:
                    var rolledLoot = RollLoot();
                    SpawnLoot(rolledLoot.ItemType, Main.rand.Next(rolledLoot.MinStack, rolledLoot.MaxStack));

                    SoundHelper.PlayAudio(SoundPaths.ShellOutCapsuleOpen.ToSoundStyle(), volume: 0.6f, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.NPCDeath63, volume: 0.2f, pitchVariance: 0.3f, pitch: 0.5f, position: Projectile.Center);
                    SoundHelper.PlayAudio(SoundID.DD2_GoblinBomb, volume: 0.1f, pitchVariance: 0f, pitch: 0.3f, maxInstances: 10, position: Projectile.Center);

                    for (int i = 0; i < 10; i++)
                    {
                        var d = DustHelper.NewDust(
                            Projectile.Center,
                            DustID.PortalBolt,
                            color: GetTierColor(),
                            velocity: Main.rand.NextVector2CircularEdge(4f, 4f) + Main.rand.NextVector2Circular(1f, 1f),
                            scale: 2f);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        var d = DustHelper.NewDust(
                            Projectile.Center,
                            DustID.PortalBolt,
                            color: GetTierColor(),
                            velocity: Main.rand.NextVector2CircularEdge(7f, 7f) + Main.rand.NextVector2Circular(3f, 3f),
                            scale: 1f);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        var g = Gore.NewGoreDirect(
                            Terraria.Entity.GetSource_None(),
                            Projectile.Center - new Vector2(Projectile.width/2, Projectile.height/2),
                            Vector2.Zero,
                            GoreID.Smoke1,
                            Main.rand.NextFloat(1.4f, 1.8f));

                        g.velocity = Main.rand.NextVector2CircularEdge(-1f, 1f);
                        g.alpha = 196;
                    }

                    if (_maxUpgrade)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            var d = DustHelper.NewDust(
                                Projectile.Center,
                                DustID.FireworksRGB,
                                color: GetTierColor().IncreaseHueBy(Main.rand.Next(-9, 9)),
                                velocity: Main.rand.NextVector2CircularEdge(10f, 10f) + Main.rand.NextVector2Circular(5f, 5f),
                                scale: 1.2f);
                            d.noGravity = false;
                        }
                    }

                    Projectile.Kill();
                    break;
            }
        }

        private bool TryUpgrade()
        {
            var tempTier = _currentTier;

            switch (_currentTier)
            {
                case _blueTier:
                    // 25%
                    if (Main.rand.NextBool(4))
                    {
                        _currentTier++;
                    }
                    break;
                case _purpleTier:
                    // 5%
                    if (Main.rand.NextBool(5))
                    {
                        _currentTier++;
                    }
                    break;
                case _redTier:
                    // 0.5%
                    if (Main.rand.NextBool(10))
                    {
                        _currentTier++;

                        _doneUpgrading = true;
                        _maxUpgrade = true;

                        Owner.GetModPlayer<PearlDronePlayer>().TriggerDialogueShellOutLuck();
                    }
                    break;
            }

            if (tempTier != _currentTier)
            {
                // Grow in size and 'shake' the capsule
                Projectile.velocity += new Vector2(Main.rand.NextFloat(-5, 5), -3);
                _drawScaleGoal += _currentTier * 0.1f;

                // Increase delay per upgrade to increase suspense
                if (!_maxUpgrade)
                {
                    _delayUntilNextUpgrade = (2 + _currentTier) * FrameSpeedMultiply(30);
                }
                else
                {
                    _delayUntilNextUpgrade = FrameSpeedMultiply(90);
                }

                // Audio/Visual
                for (int i = 0; i < 10; i++)
                {
                    var d = DustHelper.NewDust(
                        Projectile.Center,
                        DustID.PortalBolt,
                        color: GetTierColor(),
                        velocity: Main.rand.NextVector2Circular(5f, 5f),
                        scale: 2f,
                        data: new(scaleIncrement: -0.1f, gravity: 0));

                    d = DustHelper.NewDust(
                        Projectile.Center + Main.rand.NextVector2CircularEdge(16, 16) * (1 + _currentTier * 0.2f),
                        DustID.ShimmerSpark,
                        color: GetTierColor(),
                        velocity: new Vector2(0, -Main.rand.NextFloat(2, 4)),
                        scale: 2f,
                        data: new(scaleIncrement: -0.1f, gravity: 1));
                }

                var sparkle = CreateChildProjectile<StillSparkleVisual>(Projectile.Center, Vector2.Zero, 0, true);
                sparkle.AdjustRotation(0);
                sparkle.AdjustColor(ColorHelper.LerpBetweenColorsPerfect(CurrentColor, GetTierColor(), 0.3f));
                sparkle.AdjustScale(1 + _currentTier * 0.3f);

                var pitch = 1 / 12f * _currentTier;

                SoundHelper.PlayAudio(SoundID.Item4, pitch: pitch, maxInstances: 10, position: Projectile.Center);
                SoundHelper.PlayAudio(SoundID.Item60, volume: 0.3f, pitchVariance: 0.3f, maxInstances: 10, position: Projectile.Center);
                SoundHelper.PlayAudio(SoundID.Item101, pitch: pitch, volume: 0.1f, pitchVariance: 0.3f, maxInstances: 10, position: Projectile.Center);
            }
            else
            {
                _doneUpgrading = true;
            }

            return tempTier != _currentTier;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X / 30f;
            Projectile.velocity.Y += _gravity;
            _drawScale = MathHelper.Lerp(_drawScale, _drawScaleGoal, 0.1f);

            if (_currentTier > 0 && timeSpentAlive % 4 == 0 && Main.rand.NextBool(10))
            {
                var d = DustHelper.NewDust(
                    Projectile.Center + Main.rand.NextVector2Circular(24, 24) * (1 + _currentTier * 0.2f),
                    DustID.PortalBolt,
                    color: ColorHelper.LerpBetweenColorsPerfect(GetTierColor(), Color.White, 0.5f),
                    scale: Main.rand.NextFloat(1f, 1.6f));

                d.noGravity = true;
            }

            if (state > _stateFly)
            {
                Projectile.velocity.X *= _groundFriction;
            }

            switch (state)
            {
                case _stateFly:
                    break;

                case _stateRoll:
                    if (timeSpentInState > FrameSpeedMultiply(30))
                    {
                        AdvanceState();
                    }
                    break;

                case _stateUpgrade:
                    if (_delayUntilNextUpgrade > 0) _delayUntilNextUpgrade--;

                    if (_doneUpgrading)
                    {
                        if (_delayUntilNextUpgrade < FrameSpeedMultiply(10))
                        {
                            _drawScaleGoal += 0.2f;
                        }
                    }

                    if (_delayUntilNextUpgrade == 0)
                    {
                        if (!_doneUpgrading)
                        {
                            TryUpgrade();
                        }
                        else
                        {
                            AdvanceState();
                        }
                    }

                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (timeSpentAlive - _lastBounceTimestamp > 10)
            {
                _lastBounceTimestamp = timeSpentAlive;

                if (Projectile.velocity.Length() > 1)
                {
                    PlayAudio(SoundPaths.ShellOutCapsuleBounce.ToSoundStyle(), volume: 0.02f + 0.05f * oldVelocity.Length(), pitchVariance: 0.2f, maxInstances: 10, position: Projectile.Center);
                }
            }

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                if (state == _stateFly)
                {
                    SetState(_stateRoll);
                }
            }
            ProjectileBounce(oldVelocity, new Vector2(0.7f, 0.6f));

            if (!_hasBounced)
            {
                _hasBounced = true;
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(
                inkColor: Color.White,
                rotation: Projectile.rotation,
                scale: _drawScale,
                alphaMod: 1,
                considerWorldLight: true,
                additiveAmount: 0.2f,
                frameOverride: _currentTier);

            return false;
        }

        private Color GetTierColor()
        {
            switch (_currentTier)
            {
                case _blueTier:
                    return _blueColor;
                case _purpleTier:
                    return _purpleColor;
                case _redTier:
                    return _redColor;
                case _goldTier:
                    return _goldColor;
                default:
                    return Color.White;
            }
        }

        private void SpawnLoot(int type, int stackSize)
        {
            int mainItemId = Owner.QuickSpawnItem(Owner.GetSource_DropAsItem(), type, stackSize);
            var item = Main.item[mainItemId];
            item.Center = Projectile.Center;
        }

        #region Drop table

        private ShellOutDropIndex RollLoot()
        {
            List<ShellOutDropIndex> potentialDrops = new();

            int randomWeapon;

            if (!Condition.Hardmode.IsMet())
            {
                if (BossConditions.NotDownedEvilBoss.IsMet())
                {
                    randomWeapon = RandomHelper.GetRandomWeaponWithIngredient(new() { ModContent.ItemType<SheldonLicense>(), ItemID.DemoniteBar });
                }
                else
                {
                    randomWeapon = RandomHelper.GetRandomWeaponWithIngredient(new() { ModContent.ItemType<SheldonLicense>() });
                }
            }
            else
            {
                if (!Condition.DownedMechBossAll.IsMet())
                {
                    randomWeapon = RandomHelper.GetRandomHardmodeOreWeapon();
                }
                else if (!Condition.DownedPlantera.IsMet())
                {
                    randomWeapon = RandomHelper.GetRandomHardmodeOreWeapon();

                    if (Main.rand.NextBool(2))
                    {
                        randomWeapon = RandomHelper.GetRandomWeaponWithIngredient(new() { ModContent.ItemType<SheldonLicenseSilver>(), ItemID.HallowedBar });
                    }
                }
                else
                {
                    randomWeapon = RandomHelper.GetRandomWeaponWithIngredient(new() { ModContent.ItemType<SheldonLicenseSilver>() });
                }
            }

            int randomChip = RandomHelper.GetRandomColorChip();

            switch (_currentTier)
            {
                case _blueTier:
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.CopperCoin, minStack: 50, maxStack: 75, weight: 20));
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.SilverCoin, minStack: 5, maxStack: 25, weight: 60));
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.GoldCoin, minStack: 1, maxStack: 1, weight: 5));

                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<InkDroplet>(), minStack: 3, maxStack: 8, weight: 5));
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SheldonLicense>(), weight: 1));

                    if (Condition.Hardmode.IsMet())
                    {
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SheldonLicenseSilver>(), weight: 1));
                    }

                    // Ink vials
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<WaterGradientVial>(), weight: 1));
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<FireGradientVial>(), weight: 1));

                    break;

                case _purpleTier:
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.SilverCoin, minStack: 10, maxStack: 50, weight: 45));
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.GoldCoin, minStack: 1, maxStack: 3, weight: 5));
                    
                    potentialDrops.Add(new ShellOutDropIndex(randomChip, weight: 10));

                    // Potions
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<InkRegenerationPotion>(), weight: 15));
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<InkCapacityPotion>(), weight: 15));

                    // Vanity
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<BambooHat>(), weight: 5));
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<EnchantedHat>(), weight: 5));
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<PearlescentCrown>(), weight: 5));
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<MarinatedHeadphones>(), weight: 5));

                    // Woomerang
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SquidBoomerang>(), weight: 1));

                    // Vial
                    potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<RainbowGradientVial>(), weight: 1));

                    break;

                case _redTier:
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.GoldCoin, minStack: 3, maxStack: 10, weight: 5));
                    potentialDrops.Add(new ShellOutDropIndex(randomWeapon, weight: 45));

                    if (!Condition.Hardmode.IsMet())
                    {
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<DropletLocket>(), weight: 5));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<GoldHiHorses>(), weight: 5));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<UrchinEmblem>(), weight: 10));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SpinEmblem>(), weight: 5));

                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<DroneDiscA>(), weight: 2));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<Splashdown>(), weight: 5));
                    }
                    else
                    {
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SpecialChargeEmblem>(), weight: 10));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SpecialPowerEmblem>(), weight: 10));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SubPowerEmblem>(), weight: 10));

                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<MainSaverEmblem>(), weight: 5));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SubSaverEmblem>(), weight: 5));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<LastDitchEffortEmblem>(), weight: 5));

                        potentialDrops.Add(new ShellOutDropIndex(RandomHelper.GetRandomMainWeaponBoosterAccessory(), weight: 5));

                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<DroneDiscB>(), weight: 2));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<TripleSplashdown>(), weight: 5));
                    }

                    break;

                case _goldTier:
                    potentialDrops.Add(new ShellOutDropIndex(ItemID.PlatinumCoin, 1, 5, weight: 50));

                    var randomPurifiedWeapon = RandomHelper.GetRandomPurifiedWeapon();
                    potentialDrops.Add(new ShellOutDropIndex(randomPurifiedWeapon, weight: 100));

                    if (Condition.Hardmode.IsMet())
                    {
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<DroneDiscC>(), weight: 10));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SuperPaletteLeftPart>(), weight: 5));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SuperPaletteMiddlePart>(), weight: 5));
                        potentialDrops.Add(new ShellOutDropIndex(ModContent.ItemType<SuperPaletteRightPart>(), weight: 5));
                    }

                    break;
            }

            if (potentialDrops.Count == 0)
            {
                potentialDrops.Add(new ShellOutDropIndex(ItemID.GoldCoin, 1));
            }

            // Calculate the total weight/weight brackets
            int maxWeight = 0;
            List<ShellOutDropIndex> finalDrops = new();

            foreach (var drop in potentialDrops)
            {
                drop.MinWeightBracket = maxWeight;
                maxWeight += drop.Weight;
                drop.MaxWeightBracket = maxWeight;

                finalDrops.Add(drop);
            }

            // Get a random number and see between which brackets it falls
            var rng = Main.rand.Next(maxWeight);

            int currentTry = 0;
            int maxTries = 10;
            while (currentTry < maxTries)
            {
                currentTry++;

                foreach (var finalDrop in finalDrops)
                {
                    if (rng > finalDrop.MinWeightBracket && rng <= finalDrop.MaxWeightBracket)
                    {
                        return finalDrop;
                    }
                }
            }

            return new ShellOutDropIndex(ItemID.GoldCoin, 1);
        }

        #endregion
    }
}
