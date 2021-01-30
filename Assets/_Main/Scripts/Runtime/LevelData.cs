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
        }

        public LevelData(in int width, in int height)
        {
            Assert.IsTrue(width > 0);
            Assert.IsTrue(height > 0);

            m_RawData = new Block[width * height];
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
    }
}
