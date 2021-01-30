#nullable enable
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
                if (exp.Count == 0) return false;
                left = exp.Calculate();
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
                if (exp.Count == 0) return false;
                right = exp.Calculate();
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
    }
}
