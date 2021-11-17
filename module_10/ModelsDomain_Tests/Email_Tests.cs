using Models.Domain;
using NUnit.Framework;

namespace ModelsDomain_Tests
{
    public class Email_Tests
    {
        [TestCase("VasyaPupkin@mail.ru", true)]
        [TestCase("olyasem@yandex.ru", true)]
        [TestCase("krasov.sergey@gmail.com", true)]
        [TestCase("lisaëèñà@mail.ru", true)]
        [TestCase("Vasya555Pupkin@epam.com", true)]
        [TestCase("VasyaPupkin@mailru", true)]
        [TestCase("Vasya/Pupkin@mail.ru", true)]

        [TestCase("Vasya Pupkin@mail.ru", false)]
        [TestCase("VasyaPupkinmail.ru", false)]
        [TestCase("VasyaPupkin@mail ru", false)]
        [TestCase("@.", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void TryCreate_Test(string emailStr, bool expectedMethResult)
        {
            Email email = Email.TryCreate(emailStr);
            Assert.AreEqual(expectedMethResult, email is not null);
            if (expectedMethResult)
            {
                Assert.AreEqual(emailStr, email.CorrectEmail);
            }
        }
    }
}