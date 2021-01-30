#nullable enable
namespace Game
{
    public readonly struct Block
    {
        public readonly char Value;
        public readonly bool IsFixed;
        public readonly bool IsFreeze;

        public Block(in char value, bool isFixed = false, bool isFreeze = false)
        {
            Value = value;
            IsFixed = isFixed;
            IsFreeze = isFreeze;
        }
    }
}
