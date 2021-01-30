#nullable enable
using UnityEngine.Assertions;

namespace Game
{
    public class LevelData
    {
        private readonly char[] m_RawData;

        public readonly int Height;
        public readonly int Width;

        public LevelData(char[] raw, in int width, in int height)
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

            m_RawData = new char[width * height];
        }

        public char GetAt(in int x, in int y)
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

        public void SetAt(in int x, in int y, in char value)
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
