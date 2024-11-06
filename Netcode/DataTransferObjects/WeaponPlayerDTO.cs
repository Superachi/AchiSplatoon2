using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class WeaponPlayerDTO : BaseDTO
    {
        [JsonProperty]
        public bool? SpecialReady = null;

        [JsonProperty]
        public float? MoveSpeedMod = null;

        [JsonProperty]
        public float? MoveAccelMod = null;

        [JsonProperty]
        public float? MoveFrictionMod = null;

        public WeaponPlayerDTO(
            bool? specialReady = null,
            float? moveSpeedMod = null,
            float? moveAccelMod = null,
            float? moveFrictionMod = null)
        {
            SpecialReady = specialReady;
            MoveSpeedMod = moveSpeedMod;
            MoveAccelMod = moveAccelMod;
            MoveFrictionMod = moveFrictionMod;
        }

        public override void ApplyToModPlayer(ModPlayer modPlayer)
        {
            var wepMP = (WeaponPlayer)modPlayer;
            if (SpecialReady != null)
            {
                wepMP.SpecialReady = (bool)SpecialReady;
            }

            if (MoveSpeedMod != null)
            {
                wepMP.moveSpeedModifier = (float)MoveSpeedMod;
            }

            if (MoveAccelMod != null)
            {
                wepMP.moveAccelModifier = (float)MoveAccelMod;
            }

            if (MoveFrictionMod != null)
            {
                wepMP.moveFrictionModifier = (float)MoveFrictionMod;
            }
        }
    }
}
