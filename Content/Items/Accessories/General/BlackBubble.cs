using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Netcode.DataModels;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    internal class BlackBubble : BaseAccessory
    {
        public static float FallSpeedMultiplier = -0.02f;
        public static float VelocityMultiplier = 0.3f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 30;
            Item.height = 30;
            Item.SetValuePostEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<BlackBubble>();
        }

        public static void PlayBubbleSound(Projectile projectile)
        {
            var a = new PlayAudioModel(SoundID.Item87.SoundPath, _volume: 0.6f, _maxInstances: 1, _pitch: 0.2f, _pitchVariance: 0.2f, _position: projectile.Center);
            SoundHelper.PlayAudio(a);

            a = new PlayAudioModel(SoundID.Item95.SoundPath, _volume: 0.2f, _maxInstances: 1, _pitch: 0.4f, _pitchVariance: 0.2f, _position: projectile.Center);
            SoundHelper.PlayAudio(a);
        }

        public static bool ExcludeThisProjectile(ModProjectile projectile)
        {
            if (projectile is VortexDualieShotProjectile) return true;

            return false;
        }
    }
}
