using NUnit.Framework;

namespace Game.Editor.Tests
{
    sealed class EditorTest
    {
        [Test, Category("Expression")]
        public void Expression()
        {
            Assert.AreEqual(1, new IntExp("1").Calculate());
            Assert.AreEqual(1, new IntExp("1 + 0").Calculate());
            Assert.AreEqual(1, new IntExp("1 - 0").Calculate());
            Assert.AreEqual(0, new IntExp("1 * 0").Calculate());
            Assert.AreEqual(2, new IntExp("1 + 1").Calculate());
            Assert.AreEqual(3, new IntExp("1 + 1 + 1").Calculate());
            Assert.AreEqual(1, new IntExp("1 + 1 - 1").Calculate());
            Assert.AreEqual(2, new IntExp("1 + 1 * 1").Calculate());
            Assert.AreEqual(2, new IntExp("1 + 1 / 1").Calculate());
            Assert.AreEqual(3, new IntExp("1 + 1 * 2").Calculate());
            Assert.AreEqual(7, new IntExp("1 + 2 * 3").Calculate());
        }

        [Test, Category("Level")]
        public void Level001()
        {
            var data = new LevelData(3, 3);
            data.SetRow(2, Block.Wall, Block.Wall, Block.Wall);
            data.SetRow(1, Block.Empty, Block.FixedEqual, Block.Fixed1);
            data.SetRow(0, Block.Num1, Block.Wall, Block.Wall);

            Assert.IsTrue(data.IsValid());
            Assert.IsFalse(data.IsSuccess());
            Assert.IsFalse(data.TryMoveDown(0, 2));
            Assert.IsTrue(data.TryMoveUp(0, 0));
            Assert.IsTrue(data.IsSuccess());
        }

        [Test, Category("Level")]
        public void Level002()
        {
            var data = new LevelData(5, 3);
            data.SetRow(2, Block.Wall, Block.Wall, Block.Wall, Block.Wall, Block.Wall);
            data.SetRow(1, Block.Fixed2, Block.Empty, Block.FixedEqual, Block.Fixed2, Block.Fixed3);
            data.SetRow(0, Block.Wall, Block.Num3, Block.Wall, Block.Wall, Block.Wall);

            Assert.IsTrue(data.IsValid());
            Assert.IsFalse(data.IsSuccess());
            Assert.IsFalse(data.TryMoveRight(0, 1));
            Assert.IsTrue(data.TryMoveUp(1, 0));
            Assert.IsTrue(data.IsSuccess());
        }

        [Test, Category("Level")]
        public void Level015()
        {
            var data = new LevelData(7, 3);
            data.SetAt(0, 0, Block.Num3);
            data.SetAt(1, 0, Block.Empty);
            data.SetAt(2, 0, Block.Num5);
            data.SetAt(3, 0, Block.Wall);
            data.SetAt(4, 0, Block.Wall);
            data.SetAt(5, 0, Block.Wall);
            data.SetAt(6, 0, Block.Wall);
            data.SetAt(0, 1, Block.Empty);
            data.SetAt(1, 1, Block.Empty);
            data.SetAt(2, 1, Block.Empty);
            data.SetAt(3, 1, Block.Fixed8);
            data.SetAt(4, 1, Block.FixedEqual);
            data.SetAt(5, 1, Block.Fixed4);
            data.SetAt(6, 1, Block.Fixed5);
            data.SetAt(0, 2, Block.Num4);
            data.SetAt(1, 2, Block.Plus);
            data.SetAt(2, 2, Block.Num7);
            data.SetAt(3, 2, Block.Wall);
            data.SetAt(4, 2, Block.Wall);
            data.SetAt(5, 2, Block.Wall);
            data.SetAt(6, 2, Block.Wall);

            Assert.IsTrue(data.IsValid());
            Assert.IsFalse(data.IsSuccess());
            Assert.IsTrue(data.TryMoveDown(1, 2));
            Assert.IsFalse(data.IsSuccess());
            Assert.IsTrue(data.TryMoveRight(1, 1));
            Assert.IsFalse(data.IsSuccess());
            Assert.IsTrue(data.TryMoveLeft(2, 2));
            Assert.IsFalse(data.IsSuccess());
            Assert.IsTrue(data.TryMoveDown(1, 2));
            Assert.IsFalse(data.IsSuccess());
            Assert.IsTrue(data.TryMoveUp(0, 0));
            Assert.IsTrue(data.IsSuccess());
        }
    }
}
