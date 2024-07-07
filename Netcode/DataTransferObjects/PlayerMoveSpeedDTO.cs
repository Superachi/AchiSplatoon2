namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class PlayerMoveSpeedDTO(float _moveSpeedMod, float _moveAccelMod) : IDataDTO
    {
        public float moveSpeedMod = _moveSpeedMod;
        public float moveAccelMod = _moveAccelMod;

        public string CreateDTOSummary()
        {
            string desc = "DTO Summary:";
            desc += $"\n moveSpeedMod={moveSpeedMod}";
            desc += $"\n moveAccelMod={moveAccelMod}";
            return desc;
        }
    }
}
