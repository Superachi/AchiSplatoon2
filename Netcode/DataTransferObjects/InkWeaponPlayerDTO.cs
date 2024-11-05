using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Newtonsoft.Json;

namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class InkWeaponPlayerDTO : BaseDTO
    {
        [JsonProperty]
        public bool? SpecialReady = null;

        [JsonProperty]
        public int[]? ColorChipAmounts = null;

        [JsonProperty]
        public float? MoveSpeedMod = null;

        [JsonProperty]
        public float? MoveAccelMod = null;

        [JsonProperty]
        public float? MoveFrictionMod = null;

        public InkWeaponPlayerDTO(
            bool? specialReady = null,
            int[]? colorChipAmounts = null,
            float? moveSpeedMod = null,
            float? moveAccelMod = null,
            float? moveFrictionMod = null)
        {
            SpecialReady = specialReady;
            ColorChipAmounts = colorChipAmounts;
            MoveSpeedMod = moveSpeedMod;
            MoveAccelMod = moveAccelMod;
            MoveFrictionMod = moveFrictionMod;
        }

        public void ApplyToModPlayer(InkWeaponPlayer modPlayer)
        {
            if (SpecialReady != null)
            {
                modPlayer.SpecialReady = (bool)SpecialReady;
            }

            if (ColorChipAmounts != null)
            {
                modPlayer.ColorChipAmounts = ColorChipAmounts;
            }

            if (MoveSpeedMod != null)
            {
                modPlayer.moveSpeedModifier = (float)MoveSpeedMod;
            }

            if (MoveAccelMod != null)
            {
                modPlayer.moveAccelModifier = (float)MoveAccelMod;
            }

            if (MoveFrictionMod != null)
            {
                modPlayer.moveFrictionModifier = (float)MoveFrictionMod;
            }

            modPlayer.UpdateInkColor();
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(
                this,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
        }

        public static InkWeaponPlayerDTO? Deserialize(string json)
        {
            var dto = JsonConvert.DeserializeObject<InkWeaponPlayerDTO>(json);
            if (dto == null)
            {
                DebugHelper.PrintWarning($"Tried to deserialize {nameof(InkWeaponPlayerDTO)}, but the result was {dto}");
            }
            else
            {
                DebugHelper.PrintInfo($"Received JSON:\n{json}");
            }

            return dto;
        }
    }
}
