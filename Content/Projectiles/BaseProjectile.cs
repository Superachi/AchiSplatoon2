using AchiSplatoon2.Content.Buffs.Debuffs;
using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.GlobalProjectiles;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.Items.Consumables;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.AccessoryProjectiles;
using AchiSplatoon2.Content.Projectiles.LuckyBomb;
using AchiSplatoon2.Content.Projectiles.ProjectileVisuals;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.Projectiles;

enum ProjNetUpdateType : byte
{
    None,
    Initialize,
    EveryFrame,
    DustExplosion,
    UpdateCharge,
    ReleaseCharge,
    ShootAnimation,
    SyncMovement,
}

internal class BaseProjectile : ModProjectile
{
    public Player Owner => GetOwner();

    public int itemIdentifier = -1;
    private BaseWeapon weaponSource;
    public BaseWeapon WeaponInstance
    {
        get
        {
            return weaponSource;
        }
        set
        {
            weaponSource = value;
        }
    }

    public int weaponSourcePrefix = -1;

    public bool dataReady = false;
    protected virtual bool FallThroughPlatforms => true;

    public Projectile? parentProjectile = null;
    public int parentIdentity = -1;

    // If dissolvable = true, it means this projectile gets destroyed by liquids by default
    // An exception is made for shimmer
    protected bool dissolvable = true;

    // Audio
    private static SoundStyle _defaultSoundStyle = new SoundStyle(Path.Combine($"AchiSplatoon2/Content/Assets/Sounds/", "SplattershotShoot"));
    protected SoundStyle shootSample = _defaultSoundStyle;
    protected SoundStyle shootWeakSample = _defaultSoundStyle;
    protected SoundStyle shootAltSample = _defaultSoundStyle;

    // Colors
    public Color? colorOverride = null;
    public Color CurrentColor { get; private set; }
    public Color InitialColor { get; private set; }

    // Modifiers
    // See <InkWeaponPlayer.cs>
    protected float chargeSpeedModifier = 1f;
    protected float explosionRadiusModifier = 1f;
    protected int armorPierceModifier = 0;
    protected int piercingModifier = 0;
    protected float velocityModifier = 1f;
    protected float knockbackModifier = 1f;
    protected int originalDamage = 0;

    protected virtual float DamageModifierAfterPierce => 0.6f;
    protected bool enablePierceDamagefalloff;
    protected bool wormDamageReduction = false;

    protected int StandardNPCHitCooldown => 20 * FrameSpeed();
    protected bool ResetNPCHitCooldownAfterSpawnMethods = true;

    // Ink consumption
    protected virtual bool ConsumeInkAfterSpawn => true;
    public float currentInkCost = 0f;
    public float OriginalInkCost { get; private set; }

    // State machine
    protected int state = 0;
    protected int timeSpentInState = 0;
    public int timeSpentAlive = 0;

    // Netcode
    protected byte netUpdateType = (byte)ProjNetUpdateType.None;
    protected int afterInitializeDelay = 1;

    /// <summary>
    /// Used to declare a projectile destroyed locally, without invoking the Projectile.Kill() method
    /// Example use-case: makes slosher projectiles go 'inactive' on the owner client, so that other clients have time to show the projectile hitting the target/ground
    /// </summary>
    protected bool isFakeDestroyed = false;

    /// <summary>
    /// Declare a projectile destroyed locally. This sets its damage to 0, and sets Projectile.friendly to false
    /// </summary>
    protected virtual void FakeDestroy()
    {
        isFakeDestroyed = true;
        Projectile.damage = 0;
        Projectile.friendly = false;
    }

    public override bool PreAI()
    {
        timeSpentAlive++;
        timeSpentInState++;
        afterInitializeDelay--;
        if (afterInitializeDelay == 0)
        {
            AfterInitialize();
        }

        Dissolve();

        return true;
    }

    protected virtual void SetState(int targetState)
    {
        // In this method, you can do something different per changed state
        state = targetState;
        Projectile.netUpdate = true;

        timeSpentInState = 0;
    }

    protected bool IsStartOfState()
    {
        return timeSpentInState == 0;
    }

    protected virtual void AdvanceState()
    {
        state++;
        SetState(state);
    }

    #region Spawn/Despawn methods

    public void RunSpawnMethods()
    {
        if (float.IsNaN(Projectile.velocity.X) || float.IsNaN(Projectile.velocity.Y))
        {
            Projectile.velocity = Vector2.Zero;
        }

        if (float.IsNaN(Projectile.position.X) || float.IsNaN(Projectile.position.Y))
        {
            Projectile.position = GetOwner().Center;
        }

        if (float.IsNaN(Projectile.velocity.X) || float.IsNaN(Projectile.velocity.Y))
        {
            DebugHelper.PrintWarning($"Projectile '{Name}' has NaN value in its velocity");
        }

        if (float.IsNaN(Projectile.position.X) || float.IsNaN(Projectile.position.Y))
        {
            DebugHelper.PrintWarning($"Projectile '{Name}' has NaN value in its position");
        }

        AfterSpawn();
        ApplyWeaponPrefixData();
        AdjustVariablesOnShoot();
        CreateDustOnSpawn();

        if (ConsumeInkAfterSpawn)
        {
            ConsumeInk();
        }

        // If this isn't done, and extraUpdates are added, a projectile may hit the same target twice
        if (ResetNPCHitCooldownAfterSpawnMethods)
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.localNPCHitCooldown = StandardNPCHitCooldown;
            }
        }
    }

    /// <summary>
    /// This method is used to get data from the associated <seealso cref="BaseWeapon"/>.
    /// </summary>
    public virtual void ApplyWeaponInstanceData()
    {
        // Cast the provided WeaponInstance to the correct child of BaseWeapon
        // Then use it to set the projectile's properties
    }

    /// <summary>
    /// Using this, rather than <seealso cref="ModProjectile.OnSpawn"/>, provides time to set properties of the projectile after it's spawned into the world.
    /// </summary>
    protected virtual void AfterSpawn()
    {
        // Add your code here
    }

    protected virtual void ApplyWeaponPrefixData()
    {
        var prefix = PrefixHelper.GetWeaponPrefixById(weaponSourcePrefix);
        prefix?.ApplyProjectileStats(this);
    }

    /// <summary>
    /// This method is used to adjust parameters after the projectile has spawned in.
    /// </summary>
    protected virtual void AdjustVariablesOnShoot()
    {
        // "by default projectile.netupdate syncs: .identity, .position, .velocity, .knockback, .damage, .owner, .type, .ai[0],.ai[1]"
        // "https://discord.com/channels/103110554649894912/103115427491610624/385793560001249290"
    }

    protected virtual void CreateDustOnSpawn()
    {
    }

    public void SetInkCost(float inkCost)
    {
        OriginalInkCost = inkCost;
        currentInkCost = OriginalInkCost;
    }

    protected virtual void ConsumeInk(float? inkCostOverride = null, float? inkDelayOverride = null, bool consumeInkAsChildProj = false)
    {
        bool isChildProj = parentProjectile != null;
        if (isChildProj && !consumeInkAsChildProj) return;

        var inkTankPlayer = GetOwner().GetModPlayer<InkTankPlayer>();
        if (weaponSource == null)
        {
            inkTankPlayer.ConsumeInk(inkCostOverride ?? 0);
            inkTankPlayer.InkRecoveryDelay = inkDelayOverride ?? 0;
            return;
        }

        inkTankPlayer.ConsumeInk(inkCostOverride ?? currentInkCost);
        inkTankPlayer.InkRecoveryDelay = inkCostOverride ?? Math.Max(weaponSource.InkRecoveryDelay, inkTankPlayer.InkRecoveryDelay);

        if (inkTankPlayer.InkAmount < 0)
        {
            inkTankPlayer.InkAmount = 0;
        }
    }

    protected bool PlayerHasEnoughInk(float? inkCostOverride = null)
    {
        var inkTankPlayer = GetOwner().GetModPlayer<InkTankPlayer>();
        bool hasEnough = inkTankPlayer.HasEnoughInk(inkCostOverride ?? weaponSource?.InkCost ?? 0);

        return hasEnough;
    }

    public override void OnKill(int timeLeft)
    {
        RunDespawnMethods(timeLeft);
    }

    public void RunDespawnMethods(int timeLeft)
    {
        AfterKill(timeLeft);
        CreateDustOnDespawn();
    }

    protected virtual void AfterKill(int timeLeft)
    {
    }

    protected virtual void CreateDustOnDespawn()
    {
    }

    #endregion

    public virtual void AfterInitialize()
    {
        NetUpdate(ProjNetUpdateType.Initialize, true);
    }

    public bool Initialize(bool ignoreAimDeviation = false, bool isDissolvable = true, float aimDeviationOverride = -1f)
    {
        if (WeaponInstance == null)
        {
            // Attempt to get the source via the itemIdentifier
            if (itemIdentifier != -1)
            {
                // DebugHelper.PrintWarning($"itemId => {itemIdentifier}");
                ModItem modItem = ModContent.GetModItem(itemIdentifier);
                WeaponInstance = (BaseWeapon)modItem;
            }

            if (WeaponInstance == null)
            {
                DebugHelper.PrintStackTrace(this, 3);
                DebugHelper.PrintWarning($"Data for this projectile is not ready yet! (weaponSource: {WeaponInstance}, itemIdentifier: {itemIdentifier})");
                Projectile.Kill();
                return false;
            }
        }

        var owner = Main.player[Projectile.owner];
        var colorChipPlayer = owner.GetModPlayer<ColorChipPlayer>();
        SetInitialInkColor();

        if (IsThisClientTheProjectileOwner())
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = StandardNPCHitCooldown;

            if (colorChipPlayer.IsPaletteValid())
            {
                for (int i = 0; i < colorChipPlayer.ColorChipAmounts.Length; i++)
                {
                    // Red chips > more armor piercing
                    if (i == (int)ChipColor.Red)
                    {
                        armorPierceModifier += colorChipPlayer.CalculateArmorPierceBonus();
                        Projectile.ArmorPenetration += armorPierceModifier;
                    }

                    // Purple chips > faster charge speed
                    if (i == (int)ChipColor.Purple)
                    {
                        chargeSpeedModifier += colorChipPlayer.CalculateChargeSpeedBonus();
                    }

                    // Yellow chips > bigger explosions + projectile piercing
                    if (i == (int)ChipColor.Yellow)
                    {
                        explosionRadiusModifier += colorChipPlayer.CalculateExplosionRadiusBonus();
                        velocityModifier += colorChipPlayer.CalculateProjectileVelocityBonus();
                    }
                }
            }

            dissolvable = isDissolvable;

            if (!ignoreAimDeviation && WeaponInstance.AimDeviation != 0)
            {
                var vel = Projectile.velocity;
                var projSpeed = Vector2.Distance(Main.LocalPlayer.Center, Main.LocalPlayer.Center + vel);

                float dev = WeaponInstance.AimDeviation;
                if (aimDeviationOverride != -1f) dev = aimDeviationOverride;
                dev /= velocityModifier;

                Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, -dev, dev);
            }

            if (weaponSource != null && weaponSource.WeaponStyle is not MainWeaponStyle.Other)
            {
                Projectile.velocity *= velocityModifier;
            }

            UpdatePierceDamageModifiers();

            // Prevent double dipping modifiers
            if (parentIdentity == -1)
            {
                // Apply damage modifiers
                float damageMod = StatCalculationHelper.CalculateDamageModifiers(owner, WeaponInstance, this, false);
                Projectile.damage = MultiplyProjectileDamage(damageMod);
            }
            originalDamage = Projectile.damage;
        }

        return true;
    }

    protected virtual BaseProjectile CreateChildProjectile(Vector2 position, Vector2 velocity, int type, int damage, bool triggerSpawnMethods = true)
    {
        var p = Projectile.NewProjectileDirect(
            spawnSource: Projectile.GetSource_FromThis(),
            position: position,
            velocity: velocity,
            type: type,
            damage: damage,
            knockback: Projectile.knockBack,
            owner: Projectile.owner);

        var proj = p.ModProjectile as BaseProjectile;
        proj.WeaponInstance = WeaponInstance;
        proj.weaponSourcePrefix = weaponSourcePrefix;

        proj.itemIdentifier = itemIdentifier;
        proj.parentIdentity = Projectile.identity;
        proj.parentProjectile = Projectile;
        proj.colorOverride = CurrentColor;

        if (triggerSpawnMethods) proj.RunSpawnMethods();
        return proj;
    }

    protected virtual T CreateChildProjectile<T>(Vector2 position, Vector2 velocity, int damage, bool triggerSpawnMethods = true)
        where T : BaseProjectile
    {
        return (T)CreateChildProjectile(position, velocity, ModContent.ProjectileType<T>(), damage, triggerSpawnMethods);
    }

    protected Projectile? GetParentProjectile(int projectileId)
    {
        if (parentIdentity < Main.projectile.Length)
        {
            return Main.projectile[parentIdentity];
        }
        return null;
    }

    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        fallThrough = FallThroughPlatforms;
        return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
    }

    public bool IsProjectileOfType<T>(BaseProjectile projectile)
    {
        if (projectile is not T)
        {
            DebugHelper.PrintError($"{this.GetType().Name} tried to fire a projectile, but it was not of type {typeof(T)}");
            return false;
        }
        return true;
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        var accMP = GetOwner().GetModPlayer<AccessoryPlayer>();
        if (accMP.hasDamageStabilizer)
        {
            modifiers.DamageVariationScale *= 0;
            modifiers.DisableCrit();
        }

        if (wormDamageReduction && Main.expertMode && NpcHelper.IsTargetAWormSegment(target))
        {
            modifiers.FinalDamage *= 0.6f;
        }

        if (target.HasBuff<MarkedBuff>())
        {
            if (Main.rand.NextBool(MarkedBuff.CritChanceDenominator) && this is not EmpressInkTankProjectile)
            {
                modifiers.SetCrit();
            }
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        var owner = GetOwner();

        DamageToSpecialCharge(damageDone, target);

        if (enablePierceDamagefalloff && !NpcHelper.IsTargetAWormSegment(target))
        {
            Projectile.damage = MultiplyProjectileDamage(DamageModifierAfterPierce);
        }

        if (target.life <= 0)
        {
            var colorChipPlayer = owner.GetModPlayer<ColorChipPlayer>();

            if (!IsTargetEnemy(target)) return;
            if (!colorChipPlayer.IsPaletteValid()) return;
            if (NpcHelper.IsTargetAProjectile(target)) return;

            var splatInkRecoveryBonus = colorChipPlayer.CalculateSplatInkRecoveryBonus();
            if (splatInkRecoveryBonus > 0)
            {
                owner.GetModPlayer<InkTankPlayer>().HealInk(splatInkRecoveryBonus);
            }

            var luckyBombStartDamage = Math.Max(target.lifeMax / 10, Projectile.damage / 5);
            var luckyBombMinDamage = Main.expertMode ? 20 : 50;
            var luckyBombDamage = Math.Max(luckyBombMinDamage, luckyBombStartDamage);
            var luckyBombChance = colorChipPlayer.CalculateLuckyBombChance();
            var createdBombs = 0;

            void CreateLuckyBomb(int spawnOrder)
            {
                var p = CreateChildProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<LuckyBombProjectile>(), luckyBombDamage) as LuckyBombProjectile;
                p.spawnOrder = spawnOrder;
                createdBombs++;
            }

            void CreateLuckyBombCluster()
            {
                if (this is LuckyBombProjectile)
                {
                    for (int i = 0; i < Main.rand.Next(1, 3); i++)
                    {
                        CreateLuckyBomb(createdBombs);
                    }
                }
                else
                {
                    for (int i = 0; i < Main.rand.Next(2, 5); i++)
                    {
                        CreateLuckyBomb(createdBombs);
                    }
                }
            }

            float loopCount = 1f + luckyBombChance;
            for (float i = 0; i < loopCount; i++)
            {
                if (luckyBombChance > 0 && Main.rand.NextFloat() <= luckyBombChance)
                {
                    CreateLuckyBombCluster();
                }
                luckyBombChance--;
            }
        }

        var inkTankPlayer = GetOwnerModPlayer<InkTankPlayer>();
        bool isCooldownDepleted = inkTankPlayer.DropletCooldown == 0;
        bool isBoss = target.boss;
        bool otherChecks = isBoss || (target.life <= 0 && Main.rand.NextBool(20)) || Main.rand.NextBool(100);

        if (!inkTankPlayer.HasMaxInk() && IsTargetEnemy(target, true) && isCooldownDepleted && otherChecks)
        {
            if (IsThisClientTheProjectileOwner())
            {
                inkTankPlayer.ResetDropletCooldown();
                Item.NewItem(owner.GetSource_DropAsItem(), position: target.Center, Type: ModContent.ItemType<InkTankDroplet>(), Stack: 1, noGrabDelay: true);
            }
        }

        var empressInkTankPlayer = owner.GetModPlayer<EmpressInkTankPlayer>();
        if (hit.Crit
            && target.type != NPCID.TargetDummy
            && Owner.HasAccessory<EmpressInkTank>()
            && empressInkTankPlayer.CanSpawnProjectile())
        {
            empressInkTankPlayer.ActivateCooldown();
            var p = CreateChildProjectile<EmpressInkTankProjectile>(owner.Center, Vector2.Zero, 0, true);
        }
    }

    protected bool IsTargetEnemy(NPC target, bool countDummyAsEnemy = false)
    {
        if (target.dontTakeDamage) return false;
        if (target.type == NPCID.TargetDummy)
        {
            if (countDummyAsEnemy) return true;
            return false;
        }

        if (!target.friendly
                && !Main.npcCatchable[target.type]
                && target.damage > 0
                && target.lifeMax > 5) return true;

        if (target.type == NPCID.MoonLordCore
            || target.type == NPCID.MoonLordHand
            || target.type == NPCID.MoonLordHead
            || target.type == NPCID.MoonLordLeechBlob) return true;

        return false;
    }

    protected NPC? FindClosestEnemy(float maxTargetDistance, bool checkLineOfSight = false, Vector2? pointToCheck = null)
    {
        NPC npcTarget = null;
        pointToCheck = pointToCheck ?? Projectile.Center;

        float closestDistance = maxTargetDistance;
        foreach (var npc in Main.ActiveNPCs)
        {
            float distance = Projectile.Center.Distance(npc.Center);
            if (distance < closestDistance && IsTargetEnemy(npc))
            {
                if (checkLineOfSight)
                {
                    if (Collision.CanHitLine((Vector2)pointToCheck, Projectile.width, Projectile.height, npc.Center, 1, 1))
                    {
                        closestDistance = distance;
                        npcTarget = npc;
                    }
                }
                else
                {
                    closestDistance = distance;
                    npcTarget = npc;
                }
            }
        }

        return npcTarget;
    }

    protected void SetHitboxSize(int size, out Rectangle hitbox)
    {
        hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
    }

    protected bool CanHitNPCWithLineOfSight(NPC target)
    {
        if (Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1))
        {
            if (!target.friendly && Projectile.friendly) return true;
        }
        return false;
    }

    protected Player GetOwner()
    {
        return Main.player[Projectile.owner];
    }

    protected T GetOwnerModPlayer<T>()
        where T : ModPlayer
    {
        return GetOwner().GetModPlayer<T>();
    }

    protected int MultiplyProjectileDamage(float multiplier)
    {
        return (int)(Projectile.damage * multiplier);
    }

    public virtual bool CountDamageTowardsSpecialCharge()
    {
        return true;
    }

    public void DamageToSpecialCharge(float damage, NPC target)
    {
        if (target.friendly
            || target.type == NPCID.TargetDummy
            || target.SpawnedFromStatue
            || Main.npcCatchable[target.type]
            || !CountDamageTowardsSpecialCharge()
            || NpcHelper.IsTargetAProjectile(target))
        {
            return;
        }

        var specialPlayer = Owner.GetModPlayer<SpecialPlayer>();
        if (specialPlayer.SpecialReady) return;
        if (specialPlayer.SpecialActivated) return;
        if (!specialPlayer.PlayerCarriesSpecialWeapon) return;

        if (target.life <= 0 && !target.boss)
        {
            var p = CreateChildProjectile<SpecialChargeProjectile>(target.Center, Vector2.Zero, 0, true);

            p.chargeValue = 5;
            if (NpcHelper.IsTargetABossMinion(target))
            {
                p.chargeValue = 2;
            }

            return;
        }

        if (target.boss && specialPlayer.bossSpecialDropCooldown == 0)
        {
            var p = CreateChildProjectile<SpecialChargeProjectile>(target.Center, Vector2.Zero, 0, true);
            p.chargeValue = 2;

            specialPlayer.ApplyBossSpecialDropCooldown();
        }
    }

    protected int FrameSpeed(int frames = 1)
    {
        return frames + Projectile.extraUpdates;
    }

    protected float FrameSpeedDivide(float input)
    {
        return input / (1 + Projectile.extraUpdates);
    }

    protected int FrameSpeedMultiply(int input = 1)
    {
        return input * (1 + Projectile.extraUpdates);
    }

    protected float FrameSpeedMultiply(float input = 1)
    {
        return input * (1 + Projectile.extraUpdates);
    }

    public Color GenerateInkColor()
    {
        if (colorOverride != null)
        {
            return (Color)colorOverride;
        }

        return GetOwnerModPlayer<ColorChipPlayer>().GetColorFromChips();
    }

    private void SetInitialInkColor()
    {
        var color = GenerateInkColor();
        CurrentColor = color;
        InitialColor = CurrentColor;
    }

    public void UpdateCurrentColor(Color color)
    {
        CurrentColor = color;
    }

    public void UpdatePierceDamageModifiers()
    {
        if (Projectile.penetrate != 1)
        {
            enablePierceDamagefalloff = true;
            wormDamageReduction = true;
        }
    }

    /// <summary>
    /// This check makes sure that the current player (aka client) is the owner of the projectile
    /// This is used for example to make sure not all clients/server spawns ammo when the projectile is destroyed
    /// See OnKill method here: http://docs.tmodloader.net/docs/stable/class_mod_projectile.html
    /// </summary>
    /// <returns></returns>
    public bool IsThisClientTheProjectileOwner()
    {
        return Main.myPlayer == Projectile.owner;
    }

    #region Item animation
    protected void SyncProjectilePosWithPlayer(Player owner, float offsetX = 0, float offsetY = 0)
    {
        Projectile.position = owner.Center + new Vector2(offsetX, offsetY);
    }

    protected void SyncProjectilePosWithWeaponBarrel(Vector2 position, Vector2 velocity, BaseWeapon weaponData)
    {
        Vector2 weaponOffset = weaponData.HoldoutOffset() ?? new Vector2(0, 0);
        Vector2 muzzleOffset = Vector2.Normalize(velocity) * weaponData.MuzzleOffsetPx;

        if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
        {
            Projectile.position += muzzleOffset;
        }
    }

    protected void PlayerItemAnimationFaceCursor(Player owner, Vector2? offset = null, float? radiansOverride = null)
    {
        // Change player direction depending on what direction the charger is held when charging
        float mouseDirRadians;
        if (radiansOverride == null)
        {
            mouseDirRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
        }
        else
        {
            mouseDirRadians = (float)radiansOverride;
        }

        var mouseDirDegrees = MathHelper.ToDegrees(mouseDirRadians);

        if (mouseDirDegrees >= -90 && mouseDirDegrees <= 90)
        {
            owner.direction = 1;
            owner.itemRotation = mouseDirRadians;
        }
        else
        {
            owner.direction = -1;
            var sign = Math.Sign(mouseDirDegrees);
            if (sign > 0)
            {
                owner.itemRotation = MathHelper.ToRadians((mouseDirDegrees - 180));
            }
            else
            {
                owner.itemRotation = MathHelper.ToRadians(mouseDirDegrees + 180);
            }
        }

        if (offset != null)
        {
            owner.itemLocation += (Vector2)offset;
        }
        owner.itemAnimation = owner.itemAnimationMax;
        owner.itemTime = owner.itemTimeMax;
    }
    #endregion

    #region Audio

    protected virtual void PlayShootSound()
    {

    }

    protected SlotId PlayAudio(SoundStyle soundStyle, float volume = 0.3f, float pitchVariance = 0f, int maxInstances = 1, float pitch = 0f, Vector2? position = null)
    {
        position = position ?? Projectile.Center;
        return SoundHelper.PlayAudio(soundStyle, volume, pitchVariance, maxInstances, pitch, position);
    }
    #endregion

    protected void debugMessage(bool isDebug, string message)
    {
        if (isDebug) Main.NewText(message);
    }

    #region Visuals

    protected void DrawTrail(float scale = 1f, float alpha = 0.5f, int modulo = 1, Color? colorOverride = null, bool considerWorldLight = true, Vector2? positionOffset = null)
    {
        var len = Projectile.oldPos.Length;
        for (int i = len - 1; i > 0; i--)
        {
            if (i % modulo != 0 && modulo > 1) continue;

            var iMult = (float)(len - i) / len;
            var posDiff = Projectile.oldPos[i] - Projectile.position;
            var alphaMod = iMult * alpha;
            var color = colorOverride ?? ColorHelper.ColorWithAlpha255(CurrentColor);
            var offset = positionOffset ?? Vector2.Zero;

            DrawProjectile(color, Projectile.rotation, scale, alphaMod: alphaMod, considerWorldLight: considerWorldLight, positionOffset: posDiff + offset);
        }
    }

    protected void DrawTrailShrinking(float scale = 1f, float alpha = 0.5f, int modulo = 1, bool considerWorldLight = true)
    {
        var len = Projectile.oldPos.Length;
        for (int i = len - 1; i > 0; i--)
        {
            if (modulo > 1 && i % modulo == 0) continue;

            var iMult = (float)(len - i) / len;
            var posDiff = Projectile.oldPos[i] - Projectile.position;
            var alphaMod = iMult * alpha;

            DrawProjectile(ColorHelper.ColorWithAlpha255(CurrentColor), Projectile.rotation, scale * iMult, alphaMod: alphaMod, considerWorldLight: considerWorldLight, positionOffset: posDiff);
        }
    }

    protected void EmitBurstDust(float dustMaxVelocity = 1, int amount = 1, float minScale = 0.5f, float maxScale = 1f, float radiusModifier = 100f)
    {
        float radiusMult = radiusModifier / 140;
        amount = Convert.ToInt32(amount * radiusMult);
        Color dustColor = CurrentColor;

        // Ink
        for (int i = 0; i < amount / 2; i++)
        {
            DustHelper.NewDust(
                position: Projectile.Center,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: Main.rand.NextVector2Circular(dustMaxVelocity, dustMaxVelocity) * radiusMult / 2,
                color: dustColor,
                scale: Main.rand.NextFloat(minScale * 0.8f, maxScale * 0.8f),
                data: new(scaleIncrement: -0.1f, frictionMult: 0.9f, gravity: 0.2f));
        }

        for (int i = 0; i < amount * 2; i++)
        {
            DustHelper.NewDust(
                position: Projectile.Center,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: Main.rand.NextVector2CircularEdge(dustMaxVelocity, dustMaxVelocity) * radiusMult,
                color: dustColor,
                scale: Main.rand.NextFloat(minScale * 0.5f, maxScale * 0.5f),
                data: new(scaleIncrement: -0.1f, frictionMult: 0.8f));
        }

        var loopCount = 8;
        for (int i = 0; i < loopCount; i++)
        {
            var velocity = new Vector2(Main.rand.Next(8, 12), 0);
            velocity = WoomyMathHelper.AddRotationToVector2(velocity, i * (360f / loopCount) + Main.rand.Next(-20, 20));

            DustHelper.NewDust(
                position: Projectile.Center,
                dustType: DustID.FireworksRGB,
                velocity: velocity,
                color: dustColor,
                scale: (minScale + maxScale) / 6,
                data: new(gravity: 1));
        }
    }

    protected void EmitBurstDust(ExplosionDustModel dustModel)
    {
        EmitBurstDust(dustModel.dustMaxVelocity, dustModel.dustAmount, dustModel.minScale, dustModel.maxScale, dustModel.radiusModifier);
    }

    protected ExplosionProjectileVisual? CreateExplosionVisual(ExplosionDustModel expModel, PlayAudioModel? audioModel = null)
    {
        if (IsThisClientTheProjectileOwner())
        {
            if (expModel == null)
            {
                DebugHelper.PrintError($"Tried to create {nameof(ExplosionProjectileVisual)}, but {nameof(ExplosionDustModel)} was null.");
                return null;
            }

            var p = CreateChildProjectile<ExplosionProjectileVisual>(
                position: Projectile.Center,
                velocity: Vector2.Zero,
                damage: 0,
                triggerSpawnMethods: false);

            p.colorOverride = colorOverride ?? CurrentColor;
            p.explosionDustModel = expModel;
            p.playAudioModel = audioModel;
            p.RunSpawnMethods();

            return p;
        }

        return null;
    }

    protected void VisualizeRadius()
    {
        if (!IsThisClientTheProjectileOwner()) return;
        for (int i = 0; i < 15; i++)
        {
            int id = Dust.NewDust(Projectile.Center - new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.width, Projectile.height, DustID.BlueFairy, 0, 0);
            Dust d = Main.dust[id];
            d.velocity = Vector2.Zero;
        }
    }

    protected void DrawProjectile(
        Color inkColor,
        float rotation,
        float scale = 1f,
        float alphaMod = 1,
        bool considerWorldLight = true,
        SpriteEffects flipSpriteSettings = SpriteEffects.None,
        Vector2? positionOffset = null,
        float additiveAmount = 0f,
        Texture2D? spriteOverride = null,
        int? frameOverride = null,
        Vector2? positionOverride = null)
    {
        Vector2 position = (positionOverride ?? Projectile.Center) - Main.screenPosition + (positionOffset ?? Vector2.Zero);
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        if (spriteOverride != null) texture = spriteOverride;

        Rectangle sourceRectangle = texture.Frame(Main.projFrames[Projectile.type], frameX: frameOverride ?? Projectile.frame); // The sourceRectangle says which frame to use.
        Vector2 origin = sourceRectangle.Size() / 2f;

        // The light value in the world
        var lightInWorld = Color.White;
        if (considerWorldLight)
        {
            lightInWorld = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
        }
        var finalColor = new Color(inkColor.R * lightInWorld.R / 255, inkColor.G * lightInWorld.G / 255, inkColor.B * lightInWorld.G / 255);

        SpriteBatch spriteBatch = Main.spriteBatch;

        Main.EntitySpriteDraw(texture, position, sourceRectangle, finalColor * alphaMod, rotation, origin, scale, flipSpriteSettings, 0f);

        // By drawing the same sprite over the original with an additive blendmode, it becomes more vibrant
        if (additiveAmount > 0)
        {
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, position, sourceRectangle, finalColor * alphaMod * additiveAmount, rotation, origin, scale, flipSpriteSettings, 0f);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }

    protected void DirectHitDustBurst(Vector2? position = null)
    {
        if (position == null)
        {
            position = Projectile.Center;
        }

        if (IsThisClientTheProjectileOwner())
        {
            void spawnDust(Vector2 velocity, float scale, Color? newColor = null)
            {
                Color color;
                if (newColor == null)
                {
                    color = new Color(255, 255, 255);
                }
                else
                {
                    color = ColorHelper.LerpBetweenColors((Color)newColor, new Color(255, 255, 255), 0.8f);
                }

                var dust = Dust.NewDustPerfect((Vector2)position, 306,
                    velocity,
                    0, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.noLight = true;
                dust.noLightEmittence = true;
                dust.rotation = Main.rand.NextFloatDirection();
            }

            PlayAudio(SoundPaths.DirectHit.ToSoundStyle(), volume: 0.3f, pitchVariance: 0.1f);
            PlayAudio(SoundID.Item14, volume: 0.5f, pitch: 0.4f, pitchVariance: 0.2f);
            PlayAudio(SoundID.Item62, volume: 0.3f, pitchVariance: 0.1f);
            GameFeelHelper.ShakeScreenNearPlayer(Owner, true, strength: 6, speed: 8, duration: 15);

            var modPlayer = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();
            Color inkColor = colorOverride != null ? (Color)colorOverride : modPlayer.GetColorFromChips();

            for (int i = 0; i < 10; i++)
            {
                float hspeed = i * 1.5f;
                float vspeed = i / 1.5f;
                float scale = 2 - (i / 10) * 2;
                spawnDust(new Vector2(hspeed, 0), scale, inkColor);
                spawnDust(new Vector2(-hspeed, 0), scale, inkColor);
                spawnDust(new Vector2(0, vspeed), scale, inkColor);
                spawnDust(new Vector2(0, -vspeed), scale, inkColor);
                spawnDust(Main.rand.NextVector2Circular(32, 32), Main.rand.NextFloat(1.5f, 3f), inkColor);
            }
        }
    }

    protected void WeakDirectDustBurst(Vector2? position = null)
    {
        if (position == null)
        {
            position = Projectile.Center;
        }

        PlayAudio(SoundPaths.Hit.ToSoundStyle(), volume: 0.3f, pitchVariance: 0.1f, pitch: 0f, position: position);
        PlayAudio(SoundID.Item14, volume: 0.5f, pitch: 0.4f, pitchVariance: 0.2f);
        PlayAudio(SoundID.Item99, volume: 1f, pitch: 0.4f, pitchVariance: 0.2f);

        TripleHitDustBurst(position, false);
    }

    // For use by nozzlenoses, stringers, etc. Anything that shoots a burst of shots.
    protected void TripleHitDustBurst(Vector2? position = null, bool playSample = true)
    {
        if (position == null)
        {
            position = Projectile.Center;
        }

        if (IsThisClientTheProjectileOwner())
        {
            void spawnDust(Vector2 velocity, float scale, Color? newColor = null)
            {
                Color color;
                if (newColor == null)
                {
                    color = new Color(255, 255, 255);
                }
                else
                {
                    color = ColorHelper.LerpBetweenColors((Color)newColor, new Color(255, 255, 255), 0.8f);
                }

                var dust = Dust.NewDustPerfect((Vector2)position, 306,
                    velocity,
                    0, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.noLight = true;
                dust.noLightEmittence = true;
                dust.rotation = Main.rand.NextFloatDirection();
            }

            if (playSample) PlayAudio(SoundPaths.TripleHit.ToSoundStyle(), pitchVariance: 0.1f);

            var modPlayer = Main.LocalPlayer.GetModPlayer<ColorChipPlayer>();
            Color inkColor = modPlayer.GetColorFromChips();

            for (int i = 0; i < 10; i++)
            {
                float hspeed = i * 1.5f;
                float vspeed = i / 1.5f;
                float scale = 2 - (i / 10) * 2;
                spawnDust(Main.rand.NextVector2Circular(32, 32), Main.rand.NextFloat(1.5f, 3f), inkColor);
            }
        }
    }
    #endregion

    private void Dissolve()
    {
        var accMP = GetOwner().GetModPlayer<AccessoryPlayer>();
        var hasThermalInkTank = accMP.hasThermalInkTank;
        if (dissolvable && !hasThermalInkTank)
        {
            Tile tile = Framing.GetTileSafely(Projectile.Center);
            if (tile.LiquidType >= LiquidID.Water && tile.LiquidType < LiquidID.Shimmer && tile.LiquidAmount > 100)
            {
                Projectile.Kill();
            }
        }
    }

    protected virtual void ProjectileBounce(Vector2 oldVelocity, Vector2? postBounceSpeedMod = null)
    {
        // If the projectile hits the left or right side of the tile, reverse the X velocity
        if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
        {
            Projectile.velocity.X = -oldVelocity.X;
        }

        // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
        if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
        {
            Projectile.velocity.Y = -oldVelocity.Y;
        }

        if (postBounceSpeedMod != null)
        {
            Projectile.velocity *= (Vector2)postBounceSpeedMod!;
        }
    }

    protected Projectile DeflectProjectileWithinRectangle(Rectangle rect)
    {
        if (!IsThisClientTheProjectileOwner()) return null;

        foreach (var p in Main.ActiveProjectiles)
        {
            bool skip = false;
            switch (p.type)
            {
                case ProjectileID.SandnadoHostile:
                    skip = true;
                    break;
            }
            if (skip) continue;

            if (rect.Contains((int)p.Center.X, (int)p.Center.Y) && p.hostile)
            {
                var globalProjectile = p.GetGlobalProjectile<BaseGlobalProjectile>();

                if (!globalProjectile.deflected)
                {
                    globalProjectile.deflected = true;
                    p.velocity = -p.velocity;

                    return p;
                }
            }
        }

        return null;
    }

    protected bool IsVelocityGreaterThan(float speed)
    {
        return (Math.Abs(Projectile.velocity.X) > speed || Math.Abs(Projectile.velocity.Y) > speed);
    }

    #region NetCode
    public virtual void NetUpdate(ProjNetUpdateType type, bool ownerOnly = false)
    {
        // By default, perform the netUpdate
        // If ownerOnly is true, then we check if this client owns the projectile first
        bool willUpdate = true;
        if (ownerOnly)
        {
            willUpdate = IsThisClientTheProjectileOwner();
        }
        if (!willUpdate) return;

        netUpdateType = (byte)type;
        Projectile.netUpdate = true;
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        if (NetHelper.IsSinglePlayer()) return;

        writer.Write(netUpdateType);

        // Fallbacks for getting the associated item data
        if (itemIdentifier == -1)
        {
            if (WeaponInstance != null)
            {
                itemIdentifier = WeaponInstance.ItemIdentifier;
            }
            else
            {
                WeaponInstance = (BaseWeapon)Main.LocalPlayer.HeldItem.ModItem;
                itemIdentifier = WeaponInstance.ItemIdentifier;
            }
        }

        // DebugHelper.PrintInfo($"Packet type: {(ProjNetUpdateType)netUpdateType} | Item ID: {(Int16)itemIdentifier}");
        writer.Write((Int16)itemIdentifier);

        switch (netUpdateType)
        {
            case (byte)ProjNetUpdateType.Initialize:
                NetSendInitialize(writer);
                break;
            case (byte)ProjNetUpdateType.EveryFrame:
                NetSendEveryFrame(writer);
                break;
            case (byte)ProjNetUpdateType.DustExplosion:
                NetSendDustExplosion(writer);
                break;
            case (byte)ProjNetUpdateType.ReleaseCharge:
                NetSendReleaseCharge(writer);
                break;
            case (byte)ProjNetUpdateType.UpdateCharge:
                NetSendUpdateCharge(writer);
                break;
            case (byte)ProjNetUpdateType.ShootAnimation:
                NetSendShootAnimation(writer);
                break;
            case (byte)ProjNetUpdateType.SyncMovement:
                NetSendSyncMovement(writer);
                break;
        }
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        if (NetHelper.IsSinglePlayer()) return;

        netUpdateType = reader.ReadByte();
        itemIdentifier = reader.ReadInt16();
        // DebugHelper.PrintInfo($"Receiving packet type: {(ProjNetUpdateType)netUpdateType} | Item ID: {(Int16)itemIdentifier}");

        switch (netUpdateType)
        {
            case (byte)ProjNetUpdateType.Initialize:
                NetReceiveInitialize(reader);
                break;
            case (byte)ProjNetUpdateType.EveryFrame:
                NetReceiveEveryFrame(reader);
                break;
            case (byte)ProjNetUpdateType.DustExplosion:
                NetReceiveDustExplosion(reader);
                break;
            case (byte)ProjNetUpdateType.ReleaseCharge:
                NetReceiveReleaseCharge(reader);
                break;
            case (byte)ProjNetUpdateType.UpdateCharge:
                NetReceiveUpdateCharge(reader);
                break;
            case (byte)ProjNetUpdateType.ShootAnimation:
                NetReceiveShootAnimation(reader);
                break;
            case (byte)ProjNetUpdateType.SyncMovement:
                NetReceiveSyncMovement(reader);
                break;
        }
    }

    protected virtual void NetSendInitialize(BinaryWriter writer)
    {
    }

    protected virtual void NetReceiveInitialize(BinaryReader reader)
    {
        RunSpawnMethods();
    }

    private string NotImplementedWarning()
    {
        DebugHelper.PrintStackTrace(this, 3);
        return $"No implementation for this projectile packet type yet.";
    }

    protected virtual void NetSendSyncMovement(BinaryWriter writer)
    {
    }

    protected virtual void NetReceiveSyncMovement(BinaryReader reader)
    {
        DebugHelper.PrintWarning(NotImplementedWarning());
    }

    protected virtual void NetSendEveryFrame(BinaryWriter writer)
    {
        // Given that this method will be called very often, try not to make the packet size too large
    }

    protected virtual void NetReceiveEveryFrame(BinaryReader reader)
    {
        DebugHelper.PrintWarning(NotImplementedWarning());
    }

    protected virtual void NetSendDustExplosion(BinaryWriter writer)
    {
    }

    protected virtual void NetReceiveDustExplosion(BinaryReader reader)
    {
        DebugHelper.PrintWarning(NotImplementedWarning());
    }

    // Shoot animation (for cases where it doesn't manually show)
    protected virtual void NetSendShootAnimation(BinaryWriter writer)
    {
    }

    protected virtual void NetReceiveShootAnimation(BinaryReader reader)
    {
        DebugHelper.PrintWarning(NotImplementedWarning());
    }

    // Charge mechanics
    protected virtual void NetSendUpdateCharge(BinaryWriter writer)
    {
    }
    protected virtual void NetReceiveUpdateCharge(BinaryReader reader)
    {
        DebugHelper.PrintWarning(NotImplementedWarning());
    }

    protected virtual void NetSendReleaseCharge(BinaryWriter writer)
    {
    }

    protected virtual void NetReceiveReleaseCharge(BinaryReader reader)
    {
        DebugHelper.PrintWarning(NotImplementedWarning());
    }
    #endregion
}
