using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Armors.AgentEight;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class ArmorPlayer : ModPlayer
    {
        public bool hasEightMask = false;

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (hasEightMask)
            {
                modifiers.DisableSound();
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (info.SoundDisabled)
            {
                int soundId = Main.rand.Next(8);
                var path = SoundPaths.InklingHurt00;
                path = path.Remove(path.Length - 1) + soundId;

                SoundHelper.PlayAudio(path.ToSoundStyle(), volume: 1f, pitchVariance: 0.2f);
            }
        }

        private bool IsEightMaskEquipped()
        {
            var maskId = ModContent.ItemType<EightMask>();
            return Player.armor[10].type == maskId || Player.armor[0].type == maskId;
        }

        public override void UpdateEquips()
        {
            hasEightMask = IsEightMaskEquipped();
        }
    }
}
