#nullable enable
namespace Game
{
    public readonly struct Block : System.IEquatable<Block>
    {
        public static readonly Block Null = default;
        public static readonly Block Wall = new Block('w', true);
        public static readonly Block Plus = new Block('+', false);
        public static readonly Block Minus = new Block('-', false);
        public static readonly Block Mul = new Block('*', false);
        public static readonly Block Div = new Block('/', false);
        public static readonly Block Num0 = new Block('0', false);
        public static readonly Block Num1 = new Block('1', false);
        public static readonly Block Num2 = new Block('2', false);
        public static readonly Block Num3 = new Block('3', false);
        public static readonly Block Num4 = new Block('4', false);
        public static readonly Block Num5 = new Block('5', false);
        public static readonly Block Num6 = new Block('6', false);
        public static readonly Block Num7 = new Block('7', false);
        public static readonly Block Num8 = new Block('8', false);
        public static readonly Block Num9 = new Block('9', false);
        public static readonly Block Fixed0 = new Block('0', true);
        public static readonly Block Fixed1 = new Block('1', true);
        public static readonly Block Fixed2 = new Block('2', true);
        public static readonly Block Fixed3 = new Block('3', true);
        public static readonly Block Fixed4 = new Block('4', true);
        public static readonly Block Fixed5 = new Block('5', true);
        public static readonly Block Fixed6 = new Block('6', true);
        public static readonly Block Fixed7 = new Block('7', true);
        public static readonly Block Fixed8 = new Block('8', true);
        public static readonly Block Fixed9 = new Block('9', true);
        public static readonly Block FixedPlus = new Block('+', true);
        public static readonly Block FixedMinus = new Block('-', true);
        public static readonly Block FixedMul = new Block('*', true);
        public static readonly Block FixedDiv = new Block('/', true);
        public static readonly Block FixedEqual = new Block('=', true);

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

        public readonly bool Equals(Block other)
                            => Value == other.Value
            && IsFixed == other.IsFixed
            && IsFreeze == other.IsFreeze;

        public readonly override bool Equals(object obj)
            => (obj is Block other) && Equals(other);

        public readonly override int GetHashCode()
        {
            int hashCode = -1590736083;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFixed.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFreeze.GetHashCode();
            return hashCode;
        }

        public readonly bool IsExp
            => !Equals(Null) && !Equals(Wall);
    }
}
