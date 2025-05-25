namespace AchiSplatoon2.StaticData
{
    internal class ShellOutDropIndex
    {
        public int ItemType { get; set; }
        public int MinStack { get; set; }
        public int MaxStack { get; set; }
        public int Weight { get; set; }
        public int MinWeightBracket { get; set; }
        public int MaxWeightBracket { get; set; }
        public bool RareDrop { get; set; } = false;

        public ShellOutDropIndex(int itemType, int minStack = 1, int? maxStack = null, int weight = 100)
        {
            ItemType = itemType;
            MinStack = minStack;
            MaxStack = maxStack ?? minStack;
            Weight = weight;
        }
    }
}
