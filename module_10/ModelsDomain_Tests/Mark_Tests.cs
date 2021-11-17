using Models.Domain;
using NUnit.Framework;

namespace BusinessLogic_Tests
{
    public class Mark_Tests
    {
        [TestCase(1, true)]
        [TestCase(3, true)]
        [TestCase(5, true)]
        [TestCase(0, true)]

        [TestCase(6, false)]
        [TestCase(-1, false)]
        public void TryCreate_Test(int mark, bool expectedMethResult)
        {
            Mark outMark = Mark.TryCreate(mark);
            Assert.AreEqual(expectedMethResult, outMark is not null);
            if (expectedMethResult)
            {
                Assert.AreEqual(mark, outMark.CorrectMark);
            }
        }
    }
}
