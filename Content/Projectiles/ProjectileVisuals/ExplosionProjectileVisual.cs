using AchiSplatoon2.Netcode.DataModels;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Newtonsoft.Json;
using AchiSplatoon2.Helpers;

namespace AchiSplatoon2.Content.Projectiles.ProjectileVisuals
{
    internal class ExplosionProjectileVisual : BaseProjectile
    {
        private bool hasActivated = false;
        public ExplosionDustModel explosionDustModel;
        public PlayAudioModel? playAudioModel;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Timer = 3;
            // Note to self: OnSpawn runs DIRECTLY after calling Projectile.NewProjectile()
            // This means you cannot create a projectile, set a variable, and then expect those variables to be ready when OnSpawn is running
        }

        private void PlayEffect()
        {
            if (playAudioModel != null)
            {
                SoundHelper.PlayAudio(playAudioModel);
            }

            if (explosionDustModel == null)
            {
                Main.NewText("ExplosionDustModel is missing");
            }

            EmitBurstDust(explosionDustModel);
        }

        public override void AI()
        {
            Timer--;

            if (Timer <= 0 && !hasActivated)
            {
                hasActivated = true;
                PlayEffect();

                if (NetHelper.IsThisAClient())
                {
                    NetUpdate(ProjNetUpdateType.DustExplosion);
                }
            }
        }

        protected override void NetSendDustExplosion(BinaryWriter writer)
        {
            string expJson = JsonConvert.SerializeObject(explosionDustModel);
            writer.Write((string)expJson);

            bool containsAudio = playAudioModel != null;
            writer.Write((bool)containsAudio);

            if (containsAudio)
            {
                string audioJson = JsonConvert.SerializeObject(playAudioModel);
                writer.Write((string)audioJson);
            }
        }

        protected override void NetReceiveDustExplosion(BinaryReader reader)
        {
            string expJson = reader.ReadString();
            explosionDustModel = JsonConvert.DeserializeObject<ExplosionDustModel>(expJson);

            bool containsAudio = reader.ReadBoolean();
            if (containsAudio)
            {
                string audioJson = reader.ReadString();
                playAudioModel = JsonConvert.DeserializeObject<PlayAudioModel>(audioJson);
            }

            PlayEffect();
        }
    }
}
