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
    }
}
