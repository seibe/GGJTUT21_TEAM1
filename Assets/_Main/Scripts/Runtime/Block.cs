#nullable enable
namespace Game
{
    public readonly struct Block : System.IEquatable<Block>
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

        public static bool operator !=(Block left, Block right)
            => !left.Equals(right);

        public static bool operator ==(Block left, Block right)
            => left.Equals(right);

        public bool Equals(Block other)
                            => Value == other.Value
            && IsFixed == other.IsFixed
            && IsFreeze == other.IsFreeze;

        public override bool Equals(object obj)
            => (obj is Block other) && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = -1590736083;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFixed.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFreeze.GetHashCode();
            return hashCode;
        }
    }
}
