using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class UltraStamp : BaseSpecial
    {
        public override Vector2? HoldoutOffset() { return new Vector2(20, 40); }
        public override bool IsSpecialWeapon => true;
        public override float SpecialDrainPerTick => 0.15f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.damage = 50;
            Item.knockBack = 8;
            Item.scale = 2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = Item.useTime;
            Item.autoReuse = true;

            Item.width = 96;
            Item.height = 80;

            Item.value = Item.buyPrice(gold: 5);
        }

        // public override void AddRecipes() => AddRecipePostSkeletron();

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 2;
            base.ModifyHitNPC(player, target, ref modifiers);
        }

        public override void UseAnimation(Player player)
        {
            SoundHelper.PlayAudio(SoundPaths.UltraStampSwing.ToSoundStyle(), volume: 0.3f, pitchVariance: 0.1f, maxInstances: 5);

            if (player.whoAmI == Main.myPlayer)
            {
                player.velocity.X += player.direction * 3.5f;
                player.velocity.X = Math.Clamp(player.velocity.X, -8, 8);

                // Apply damage reduction
                player.AddBuff(ModContent.BuffType<UltraStampBuff>(), 30);
            }
        }
    }
}
