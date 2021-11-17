using NUnit.Framework;
using Moq;
using BusinessLayer;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.MessageSenders;

namespace BusinessLayer_Tests
{
    public class HomeworksAnalyzer_Tests
    {
        private Models.Domain.Student[] _mockStudents;
        private Models.Domain.Homework[] _mockHomeworks;
        private HomeworksAnalyzer _homeworksAnalyzer;

        private int cntSMS = 0;
        [OneTimeSetUp]
        public void Setup()
        {
            _mockStudents = new[]
            {
                new Models.Domain.Student()
            };
            _mockHomeworks = new[]
            {
                new Models.Domain.Homework()
                {
                    LectionId = 1,
                    StudentId = 1,
                    Mark = Models.Domain.Mark.TryCreate(2)
                }
            };
            Mock<IStudentsService> mockStudentsService = new Mock<IStudentsService>(MockBehavior.Strict);
            mockStudentsService.Setup(service => service.TryGet(out _mockStudents, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            Mock<IHomeworksService> mockHomeworksService = new Mock<IHomeworksService>(MockBehavior.Strict);
            mockHomeworksService.Setup(service => service.TryGet(out _mockHomeworks, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            Mock<ISMSSender> mockSMSSender = new Mock<ISMSSender>(MockBehavior.Strict);
            mockSMSSender.Setup(sender => sender.Send(It.IsAny<Models.Domain.PhoneNumber>(), It.IsAny<string>())).Callback(() => cntSMS++);
            mockSMSSender.SetupGet<Models.Domain.PhoneNumber>(sender => sender.PhoneNumberOfSender).Returns(Models.Domain.PhoneNumber.TryCreate("+79825695545"));

            _homeworksAnalyzer = new HomeworksAnalyzer(mockStudentsService.Object, mockHomeworksService.Object, mockSMSSender.Object);
        }

        [Test]
        public void TryCheckMeanMark_Test()
        {
            bool res = _homeworksAnalyzer.TryCheckMeanMark();
            Assert.IsTrue(res);
            Assert.AreEqual(1, cntSMS);
        }
    }
}
