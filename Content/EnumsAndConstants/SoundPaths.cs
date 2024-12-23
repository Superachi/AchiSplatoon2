using System.IO;
using Terraria.Audio;

namespace AchiSplatoon2.Content.EnumsAndConstants
{
    internal static class SoundPaths
    {
        private static readonly string _audioRootPath = $"AchiSplatoon2/Content/Assets/Sounds/";

        public static SoundStyle ToSoundStyle(this string soundPath, bool appendAudioRootPath = false)
        {
            if (appendAudioRootPath)
            {
                soundPath = Path.Combine(_audioRootPath, soundPath);
            }

            return new SoundStyle(soundPath);
        }

        private static string FormatPath(string filename, string directory = "")
        {
            var name = filename.ToLower();

            if (name.StartsWith("brella"))
            {
                directory = "Brellas";
            }

            if (name.StartsWith("dualie"))
            {
                directory = "Dualies";
            }

            if (name.StartsWith("roller"))
            {
                directory = "Rollers";
            }

            if (name.StartsWith("slosher"))
            {
                directory = "Sloshers";
            }

            if (name.StartsWith("splatana"))
            {
                directory = "Splatanas";
            }

            var path = Path.Combine(_audioRootPath, directory, filename);
            //DebugHelper.PrintInfo(path);
            return path;
        }

        // Root directory
        public static string AchiGunBlast => FormatPath(nameof(AchiGunBlast));
        public static string AchiGunBoost => FormatPath(nameof(AchiGunBoost));
        public static string BambooChargerShoot => FormatPath(nameof(BambooChargerShoot));
        public static string BambooChargerShootWeak => FormatPath(nameof(BambooChargerShootWeak));
        public static string BlasterExplosion => FormatPath(nameof(BlasterExplosion));
        public static string BlasterExplosionLight => FormatPath(nameof(BlasterExplosionLight));
        public static string BlasterFire => FormatPath(nameof(BlasterFire));
        public static string BrushShoot => FormatPath(nameof(BrushShoot));
        public static string BrushShootAlt => FormatPath(nameof(BrushShootAlt));
        public static string ChargeReady => FormatPath(nameof(ChargeReady));
        public static string ChargeStart => FormatPath(nameof(ChargeStart));
        public static string DirectHit => FormatPath(nameof(DirectHit));
        public static string Dot52GalShoot => FormatPath(nameof(Dot52GalShoot));
        public static string Dot96GalShoot => FormatPath(nameof(Dot96GalShoot));
        public static string EliterChargerShoot => FormatPath(nameof(EliterChargerShoot));
        public static string EliterChargerShootWeak => FormatPath(nameof(EliterChargerShootWeak));
        public static string EmptyInkTank => FormatPath(nameof(EmptyInkTank));
        public static string HeroShotShoot => FormatPath(nameof(HeroShotShoot));
        public static string Hit => FormatPath(nameof(Hit));
        public static string HitNoDamage => FormatPath(nameof(HitNoDamage));
        public static string InkHitSplash00 => FormatPath(nameof(InkHitSplash00));
        public static string ItemGet => FormatPath(nameof(ItemGet));
        public static string ItemGet2 => FormatPath(nameof(ItemGet2));
        public static string JetSquelcherShoot => FormatPath(nameof(JetSquelcherShoot));
        public static string SBlastShoot => FormatPath(nameof(SBlastShoot));
        public static string Silence => FormatPath(nameof(Silence));
        public static string SlosherShoot => FormatPath(nameof(SlosherShoot));
        public static string SlosherShootAlt => FormatPath(nameof(SlosherShootAlt));
        public static string SloshingMachineShoot => FormatPath(nameof(SloshingMachineShoot));
        public static string SnipewriterShoot => FormatPath(nameof(SnipewriterShoot));
        public static string SplatChargerShoot => FormatPath(nameof(SplatChargerShoot));
        public static string SplatChargerShootWeak => FormatPath(nameof(SplatChargerShootWeak));
        public static string SplatlingChargeLoop => FormatPath(nameof(SplatlingChargeLoop));
        public static string SplatlingChargeReady => FormatPath(nameof(SplatlingChargeReady));
        public static string SplatlingChargeStart => FormatPath(nameof(SplatlingChargeStart));
        public static string SplatlingShoot => FormatPath(nameof(SplatlingShoot));
        public static string SplattershotShoot => FormatPath(nameof(SplattershotShoot));
        public static string SquifferChargerShoot => FormatPath(nameof(SquifferChargerShoot));
        public static string SquifferChargerShootWeak => FormatPath(nameof(SquifferChargerShootWeak));
        public static string TripleHit => FormatPath(nameof(TripleHit));
        public static string TriStringerShoot => FormatPath(nameof(TriStringerShoot));

        // Brellas
        public static string BrellaBreak => FormatPath(nameof(BrellaBreak));
        public static string BrellaDeflect => FormatPath(nameof(BrellaDeflect));
        public static string BrellaRecover => FormatPath(nameof(BrellaRecover));
        public static string BrellaShot => FormatPath(nameof(BrellaShot));

        // Dualies
        public static string DualieGloogaRoll => FormatPath(nameof(DualieGloogaRoll));
        public static string DualieSplatRoll => FormatPath(nameof(DualieSplatRoll));
        public static string DualieTetraRoll => FormatPath(nameof(DualieTetraRoll));

        // Rollers
        public static string RollerFling1 => FormatPath(nameof(RollerFling1));
        public static string RollerFling2 => FormatPath(nameof(RollerFling2));
        public static string RollerSwingMedium => FormatPath(nameof(RollerSwingMedium));
        public static string RollerSwingSmall => FormatPath(nameof(RollerSwingSmall));

        // Sloshers
        public static string SlosherBloblobberShoot => FormatPath(nameof(SlosherBloblobberShoot));
        public static string SlosherBloblobberShootAlt => FormatPath(nameof(SlosherBloblobberShootAlt));

        // Specials
        private static readonly string _directorySpecial = "Specials";
        public static string SpecialReady => FormatPath(nameof(SpecialReady), _directorySpecial);

        public static string KillerWailCharge => FormatPath(nameof(KillerWailCharge), _directorySpecial);
        public static string KillerWailDespawn => FormatPath(nameof(KillerWailDespawn), _directorySpecial);
        public static string KillerWailFire => FormatPath(nameof(KillerWailFire), _directorySpecial);
        public static string KillerWailSpawn => FormatPath(nameof(KillerWailSpawn), _directorySpecial);

        public static string TrizookaLaunch => FormatPath(nameof(TrizookaLaunch), _directorySpecial);
        public static string TrizookaLaunchAlly => FormatPath(nameof(TrizookaLaunchAlly), _directorySpecial);
        public static string TrizookaLaunchWet => FormatPath(nameof(TrizookaLaunchWet), _directorySpecial);
        public static string TrizookaActivate => FormatPath(nameof(TrizookaActivate), _directorySpecial);
        public static string TrizookaDeactivate => FormatPath(nameof(TrizookaDeactivate), _directorySpecial);
        public static string TrizookaSplash => FormatPath(nameof(TrizookaSplash), _directorySpecial);

        public static string UltraStampActivate => FormatPath(nameof(UltraStampActivate), _directorySpecial);
        public static string UltraStampFly => FormatPath(nameof(UltraStampFly), _directorySpecial);
        public static string UltraStampHit => FormatPath(nameof(UltraStampHit), _directorySpecial);
        public static string UltraStampSwing => FormatPath(nameof(UltraStampSwing), _directorySpecial);
        public static string UltraStampThrow => FormatPath(nameof(UltraStampThrow), _directorySpecial);

        public static string BombRushActivate => FormatPath(nameof(BombRushActivate), _directorySpecial);
        public static string BombRushJingle => FormatPath(nameof(BombRushJingle), _directorySpecial);
        public static string BombRushJingleAlt => FormatPath(nameof(BombRushJingleAlt), _directorySpecial);


        // Splatanas
        public static string SplatanaStamperCharge => FormatPath(nameof(SplatanaStamperCharge));
        public static string SplatanaStamperStrongSlash => FormatPath(nameof(SplatanaStamperStrongSlash));
        public static string SplatanaStamperWeakSlash => FormatPath(nameof(SplatanaStamperWeakSlash));
        public static string SplatanaWiperCharge => FormatPath(nameof(SplatanaWiperCharge));
        public static string SplatanaWiperStrongSlash => FormatPath(nameof(SplatanaWiperStrongSlash));
        public static string SplatanaWiperWeakSlash => FormatPath(nameof(SplatanaWiperWeakSlash));

        // Swim Form
        private static readonly string _directorySwimForm = "SwimForm";
        public static string SwimFormEnter => FormatPath("Enter", _directorySwimForm);
        public static string SwimFormExit => FormatPath("Exit", _directorySwimForm);
        public static string Slime00 => FormatPath(nameof(Slime00), _directorySwimForm);
        public static string Slime01 => FormatPath(nameof(Slime01), _directorySwimForm);
        public static string Slime02 => FormatPath(nameof(Slime02), _directorySwimForm);
        public static string Slime03 => FormatPath(nameof(Slime03), _directorySwimForm);
        public static string Slime04 => FormatPath(nameof(Slime04), _directorySwimForm);


        // Throwables
        private static readonly string _directoryThrowables = "Throwables";
        public static string AngleShooterThrow => FormatPath(nameof(AngleShooterThrow), _directoryThrowables);
        public static string InkMineActivate => FormatPath(nameof(InkMineActivate), _directoryThrowables);
        public static string InkMineDetonate => FormatPath(nameof(InkMineDetonate), _directoryThrowables);
        public static string SplatBombDetonate => FormatPath(nameof(SplatBombDetonate), _directoryThrowables);
        public static string SplatBombFuse => FormatPath(nameof(SplatBombFuse), _directoryThrowables);
        public static string SplatBombThrow => FormatPath(nameof(SplatBombThrow), _directoryThrowables);
        public static string SprinklerDeploy => FormatPath(nameof(SprinklerDeploy), _directoryThrowables);
        public static string SprinklerDeployNew => FormatPath(nameof(SprinklerDeployNew), _directoryThrowables);
        public static string TorpedoChase => FormatPath(nameof(TorpedoChase), _directoryThrowables);
        public static string TorpedoLockOn => FormatPath(nameof(TorpedoLockOn), _directoryThrowables);

        // Voice SFX
        private static readonly string _directoryVoiceInkling = "Voice\\InklingGirl";
        public static string InklingHurt00 => FormatPath("Voice_SquidGirl_Damage_00", _directoryVoiceInkling);
        public static string InklingHurt01 => FormatPath("Voice_SquidGirl_Damage_01", _directoryVoiceInkling);
        public static string InklingHurt02 => FormatPath("Voice_SquidGirl_Damage_02", _directoryVoiceInkling);
        public static string InklingHurt03 => FormatPath("Voice_SquidGirl_Damage_03", _directoryVoiceInkling);
        public static string InklingHurt04 => FormatPath("Voice_SquidGirl_Damage_04", _directoryVoiceInkling);
        public static string InklingHurt05 => FormatPath("Voice_SquidGirl_Damage_05", _directoryVoiceInkling);
        public static string InklingHurt06 => FormatPath("Voice_SquidGirl_Damage_06", _directoryVoiceInkling);
        public static string InklingHurt07 => FormatPath("Voice_SquidGirl_Damage_07", _directoryVoiceInkling);


        private static readonly string _directoryPearl = "Voice\\Pearl";
        public static string PearlShortTalk1 => FormatPath("ShortTalk1", _directoryPearl);
        public static string PearlShortTalk2 => FormatPath("ShortTalk2", _directoryPearl);
        public static string PearlShortTalk3 => FormatPath("ShortTalk3", _directoryPearl);
        public static string PearlShortTalk4 => FormatPath("ShortTalk4", _directoryPearl);
        public static string PearlShortTalk5 => FormatPath("ShortTalk5", _directoryPearl);
        public static string PearlShout1 => FormatPath("Shout1", _directoryPearl);
        public static string PearlShout2 => FormatPath("Shout2", _directoryPearl);
        public static string PearlShout3 => FormatPath("Shout3", _directoryPearl);
    }
}
