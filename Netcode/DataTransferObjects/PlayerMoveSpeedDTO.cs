namespace AchiSplatoon2.Netcode.DataTransferObjects
{
    internal class PlayerMoveSpeedDTO(float _moveSpeedMod, float _moveAccelMod, float _moveFrictionMod)
    {
        public float moveSpeedMod = _moveSpeedMod;
        public float moveAccelMod = _moveAccelMod;
        public float moveFrictionMod = _moveFrictionMod;
    }
}
