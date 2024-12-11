using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class TentacularOcular : BaseWeaponBoosterAccessory
    {
        public static float TerrainPierceDamageMod = 0.8f;
        public static float TerrainPierceVelocityMod = 0.2f;
        public static float BaseCritChance = 10f;
        public static int TerrainMaxPierceCount = 5;

        protected override string UsageHintParamA => "";
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"{BaseCritChance}");

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.hasTentacleScope = true;
            }
        }
    }
}
