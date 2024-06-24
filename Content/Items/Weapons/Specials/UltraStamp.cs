using AchiSplatoon2.Content.Projectiles.SpecialProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AchiSplatoon2.Content.Players;
using Terraria.DataStructures;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.Buffs;

namespace AchiSplatoon2.Content.Items.Weapons.Specials
{
    internal class UltraStamp : BaseSpecial
    {
        public override Vector2? HoldoutOffset() { return new Vector2(20, 40); }
        public override bool IsSpecialWeapon => true;
        public override float SpecialDrainPerTick => 0.12f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Melee;
            Item.damage = 80;
            Item.knockBack = 10;
            Item.scale = 2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = Item.useTime;
            Item.autoReuse = true;

            Item.width = 96;
            Item.height = 80;

            Item.value = Item.buyPrice(gold: 5);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= 2;
            base.ModifyHitNPC(player, target, ref modifiers);
        }

        public override void UseAnimation(Player player)
        {
            SoundHelper.PlayAudio("Specials/UltraStamp/UltraStampSwing", volume: 0.3f, pitchVariance: 0.1f, maxInstances: 5);

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
