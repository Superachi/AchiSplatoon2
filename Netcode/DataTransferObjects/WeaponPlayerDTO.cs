using AchiSplatoon2.Content.Players;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class WeaponPlayerDTO : BaseDTO
    {
        [JsonProperty]
        public float? MoveSpeedMod = null;

        [JsonProperty]
        public float? MoveAccelMod = null;

        [JsonProperty]
        public float? MoveFrictionMod = null;

        public WeaponPlayerDTO(
            float? moveSpeedMod = null,
            float? moveAccelMod = null,
            float? moveFrictionMod = null)
        {
            MoveSpeedMod = moveSpeedMod;
            MoveAccelMod = moveAccelMod;
            MoveFrictionMod = moveFrictionMod;
        }

        public override void ApplyToModPlayer(ModPlayer modPlayer)
        {
            var wepMP = (WeaponPlayer)modPlayer;

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
