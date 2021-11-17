using Models.Domain;
using NUnit.Framework;

namespace BusinessLogic_Tests
{
    public class PhoneNumber_Tests
    {
        [TestCase("+79811542232", true)]
        [TestCase("89811542232", true)]

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("+7 9811542232", false)]
        [TestCase("898115422325", false)]
        [TestCase(" +79811542232", false)]
        [TestCase("+7a811542232", false)]
        public void TryCreate_Tests(string phoneNumber, bool expectedMethResult)
        {
            var outPhoneNumber = PhoneNumber.TryCreate(phoneNumber);
            Assert.AreEqual(expectedMethResult, outPhoneNumber is not null);
            if (expectedMethResult)
            {
                Assert.AreEqual(phoneNumber, outPhoneNumber.CorrectPhoneNumber);
            }
        }
    }
}
