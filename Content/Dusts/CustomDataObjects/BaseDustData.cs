namespace AchiSplatoon2.Content.Dusts.CustomDataObjects
{
    internal class BaseDustData
    {
        public bool emitLight;
        public float scaleIncrement;
        public float frictionMult;
        public float gravity;
        public float gravityDelay;

        public BaseDustData(
            bool emitLight = true,
            float scaleIncrement = -0.05f,
            float frictionMult = 1f,
            float gravity = 0,
            float gravityDelay = 0)
        {
            this.emitLight = emitLight;
            this.scaleIncrement = scaleIncrement;
            this.frictionMult = frictionMult;
            this.gravity = gravity;
            this.gravityDelay = gravityDelay;
        }
    }
}
