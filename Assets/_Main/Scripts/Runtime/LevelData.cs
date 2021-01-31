#nullable enable
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Assertions;

namespace Game
{
    public class LevelData
    {
        private readonly Block[] m_RawData;

        public readonly int Height;
        public readonly int Width;

        public LevelData(Block[] raw, in int width, in int height)
        {
            Assert.IsTrue(width > 0);
            Assert.IsTrue(height > 0);
            Assert.AreEqual(raw.Length, width * height);

            m_RawData = raw;
            Width = width;
            Height = height;
        }

        public LevelData(in int width, in int height)
        {
            Assert.IsTrue(width > 0);
            Assert.IsTrue(height > 0);

            m_RawData = new Block[width * height];
            Width = width;
            Height = height;
        }

        public Block GetAt(in int x, in int y)
        {
            if (x < 0 || x >= Width)
            {
                throw new System.ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= Height)
            {
                throw new System.ArgumentOutOfRangeException(nameof(y));
            }

            return m_RawData[x + Width * y];
        }

        public bool IsSuccess()
        {
            if (!FindFixedEqaul(out var x, out var y)) return false;

            // 左辺を収集、計算
            int left;
            {
                var exp = new IntExp();
                for (var i = x; i-- > 0;)
                {
                    var block = GetAt(i, y);
                    if (!block.IsExp) break;
                    exp.Unshift(block.Value);
                }

                if (exp.Count == 0 || !exp.TryCalculate(out left)) return false;
            }

            // 右辺を収集、計算
            int right;
            {
                var exp = new IntExp();
                for (var i = x; ++i < Width;)
                {
                    var block = GetAt(i, y);
                    if (!block.IsExp) break;
                    exp.Push(block.Value);
                }

                if (exp.Count == 0 || !exp.TryCalculate(out right)) return false;
            }

            return left == right;

            bool FindFixedEqaul(out int x, out int y)
            {
                var index = System.Array.IndexOf(m_RawData, Block.FixedEqual);
                if (index != -1)
                {
                    x = index % Width;
                    y = index / Width;
                    return true;
                }

                x = y = default;
                return false;
            }
        }

        public bool IsValid()
        {
            var count = 0;
            foreach (var block in m_RawData)
            {
                if (block == Block.FixedEqual) count++;
            }

            return count == 1;
        }

        public ref Block RefAt(in int x, in int y)
        {
            if (x < 0 || x >= Width)
            {
                throw new System.ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= Height)
            {
                throw new System.ArgumentOutOfRangeException(nameof(y));
            }

            return ref m_RawData[x + Width * y];
        }

        public void SetAt(in int x, in int y, in Block value)
        {
            if (x < 0 || x >= Width)
            {
                throw new System.ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= Height)
            {
                throw new System.ArgumentOutOfRangeException(nameof(y));
            }

            m_RawData[x + Width * y] = value;
        }

        public void SetRow(in int y, params Block[] row)
        {
            Assert.AreEqual(Width, row.Length);

            for (var x = 0; x < row.Length; x++)
            {
                SetAt(x, y, row[x]);
            }
        }

        /// <summary>
        /// 下に移動する
        /// </summary>
        /// <param name="x">移動元ブロックのX座標</param>
        /// <param name="y">移動元ブロックのY座標</param>
        /// <returns>移動できたかどうか</returns>
        public bool TryMoveDown(in int x, in int y)
            => TryMove(x, y, x, y - 1);

        /// <summary>
        /// 左に移動する
        /// </summary>
        /// <param name="x">移動元ブロックのX座標</param>
        /// <param name="y">移動元ブロックのY座標</param>
        /// <returns>移動できたかどうか</returns>
        public bool TryMoveLeft(in int x, in int y)
            => TryMove(x, y, x - 1, y);

        /// <summary>
        /// 右に移動する
        /// </summary>
        /// <param name="x">移動元ブロックのX座標</param>
        /// <param name="y">移動元ブロックのY座標</param>
        /// <returns>移動できたかどうか</returns>
        public bool TryMoveRight(in int x, in int y)
            => TryMove(x, y, x + 1, y);

        /// <summary>
        /// 上に移動する
        /// </summary>
        /// <param name="x">移動元ブロックのX座標</param>
        /// <param name="y">移動元ブロックのY座標</param>
        /// <returns>移動できたかどうか</returns>
        public bool TryMoveUp(in int x, in int y)
            => TryMove(x, y, x, y + 1);

        private bool TryMove(in int x, in int y, in int x2, in int y2)
        {
            if (x2 < 0 || x2 >= Width) return false;
            if (y2 < 0 || y2 >= Height) return false;

            var to = GetAt(x2, y2);
            if (to != Block.Empty) return false;

            var from = GetAt(x, y);
            if (from == Block.Empty || from.IsFixed) return false;

            SetAt(x2, y2, from);
            SetAt(x, y, Block.Empty);
            return true;
        }

        public static bool TryGetLevel(in int index, [NotNullWhen(true)] out LevelData? data)
        {
            switch (index + 1)
            {
                case 1:
                    data = new LevelData(3, 3);
                    data.SetRow(2, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedEqual, Block.Fixed1);
                    data.SetRow(0, Block.Num1, Block.Wall, Block.Wall);
                    return true;

                case 2:
                    data = new LevelData(5, 3);
                    data.SetRow(2, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Fixed2, Block.Empty, Block.FixedEqual, Block.Fixed2, Block.Fixed3);
                    data.SetRow(0, Block.Wall, Block.Num3, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 3:
                    data = new LevelData(5, 4);
                    data.SetRow(3, Block.Num3, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.FixedEqual, Block.Fixed1, Block.Fixed3);
                    data.SetRow(0, Block.Wall, Block.Num1, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 4:
                    data = new LevelData(5, 3);
                    data.SetRow(2, Block.Num1, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Fixed3);
                    data.SetRow(0, Block.Wall, Block.Wall, Block.Num2, Block.Wall, Block.Wall);
                    return true;

                case 5:
                    data = new LevelData(3, 3);
                    data.SetRow(2, Block.Num3, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedEqual, Block.Num3);
                    data.SetRow(0, Block.Num1, Block.Wall, Block.Wall);
                    return true;

                case 6:
                    data = new LevelData(7, 3);
                    data.SetRow(2, Block.Num1, Block.Wall, Block.Num4, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Num2,
                        Block.FixedPlus, Block.Num3);
                    data.SetRow(0, Block.Num9, Block.Wall, Block.Num1, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 7:
                    data = new LevelData(6, 4);
                    data.SetRow(3, Block.Num9, Block.Wall, Block.Num8, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Num1, Block.Num8);
                    data.SetRow(0, Block.Num9, Block.Wall, Block.Num1, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 8:
                    data = new LevelData(7, 3);
                    data.SetRow(2, Block.Num6, Block.Wall, Block.Num5, Block.Wall, Block.Num6, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Empty, Block.FixedPlus, Block.Fixed3);
                    data.SetRow(0, Block.Num4, Block.Wall, Block.Num1, Block.Wall, Block.Num4, Block.Wall, Block.Wall);
                    return true;

                case 9:
                    data = new LevelData(7, 3);
                    data.SetRow(2, Block.Num1, Block.Num3, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.FixedPlus, Block.Fixed7, Block.FixedEqual, Block.Num2, Block.Num1);
                    data.SetRow(0, Block.Num4, Block.Num5, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 10:
                    data = new LevelData(7, 4);
                    data.SetRow(3, Block.Num4, Block.Num1, Block.Wall, Block.Num0, Block.Wall, Block.Num3, Block.Num5);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Empty, Block.Empty);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Empty, Block.Empty);
                    data.SetRow(0, Block.Wall, Block.Num3, Block.Wall, Block.Num9, Block.Wall, Block.Wall, Block.Num1);
                    return true;

                case 11:
                    data = new LevelData(5, 3);
                    data.SetRow(2, Block.Num0, Block.Wall, Block.Num3, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Fixed4);
                    data.SetRow(0, Block.Num2, Block.Wall, Block.Num4, Block.Wall, Block.Wall);
                    return true;

                case 12:
                    data = new LevelData(7, 4);
                    data.SetRow(3, Block.Num3, Block.Num0, Block.Wall, Block.Wall, Block.Wall, Block.Num0, Block.Num1);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Wall, Block.Wall, Block.Wall, Block.Empty, Block.Empty);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.FixedPlus, Block.Fixed9, Block.FixedEqual, Block.Empty, Block.Empty);
                    data.SetRow(0, Block.Wall, Block.Num1, Block.Wall, Block.Wall, Block.Wall, Block.Num2, Block.Num3);
                    return true;

                case 13:
                    data = new LevelData(9, 4);
                    data.SetRow(3, Block.Num0, Block.Num1, Block.Wall, Block.Num2, Block.Wall, Block.Num1, Block.Wall, Block.Num9, Block.Num8);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Empty, Block.Empty, Block.Empty, Block.Empty);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Empty, Block.FixedPlus, Block.Empty, Block.Empty);
                    data.SetRow(0, Block.Num1, Block.Num3, Block.Wall, Block.Num4, Block.Wall, Block.Num9, Block.Wall, Block.Wall, Block.Num0);
                    return true;

                case 14:
                    data = new LevelData(5, 3);
                    data.SetRow(2, Block.Wall, Block.Plus, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Fixed1, Block.Empty, Block.Fixed3, Block.FixedEqual, Block.Fixed4);
                    data.SetRow(0, Block.Wall, Block.Minus, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 15:
                    data = new LevelData(7, 3);
                    data.SetRow(2, Block.Num4, Block.Plus, Block.Num7, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.Empty, Block.Fixed8, Block.FixedEqual, Block.Num4, Block.Num5);
                    data.SetRow(0, Block.Num3, Block.Num5, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 16:
                    data = new LevelData(9, 4);
                    data.SetRow(3, Block.Num3, Block.Num1, Block.Num9, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Num4, Block.Wall, Block.Num1, Block.Num0, Block.Num3);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.Empty, Block.Fixed1, Block.Empty, Block.FixedEqual, Block.Empty, Block.Empty, Block.Empty);
                    data.SetRow(0, Block.Num2, Block.Empty, Block.Num2, Block.Wall, Block.Num2, Block.Wall, Block.Num4, Block.Num2, Block.Num6);
                    return true;

                case 17:
                    data = new LevelData(8, 4);
                    data.SetRow(3, Block.Num6, Block.Num9, Block.Wall, Block.Num7, Block.Num3, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.FixedMinus, Block.Empty, Block.Empty, Block.FixedEqual, Block.Fixed1, Block.Fixed9);
                    data.SetRow(0, Block.Wall, Block.Num0, Block.Wall, Block.Num2, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    return true;

                case 18:
                    data = new LevelData(7, 4);
                    data.SetRow(3, Block.Num1, Block.Wall, Block.Num3, Block.Wall, Block.Num1, Block.Wall, Block.Wall);
                    data.SetRow(2, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.FixedMinus, Block.Empty, Block.FixedPlus, Block.Empty, Block.FixedEqual, Block.Fixed0);
                    data.SetRow(0, Block.Num1, Block.Wall, Block.Num4, Block.Wall, Block.Num3, Block.Wall, Block.Wall);
                    return true;

                case 19:
                    data = new LevelData(9, 4);
                    data.SetRow(3, Block.Wall, Block.Empty, Block.Num9, Block.Minus, Block.Num4, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
                    data.SetRow(2, Block.Wall, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Wall, Block.Num9, Block.Plus, Block.Wall);
                    data.SetRow(1, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.Empty, Block.FixedEqual, Block.Empty, Block.Empty, Block.Fixed3);
                    data.SetRow(0, Block.Wall, Block.Wall, Block.Num1, Block.Plus, Block.Num8, Block.Wall, Block.Num1, Block.Minus, Block.Wall);
                    return true;

                default:
                    data = null;
                    return false;
            }
        }
    }
}
