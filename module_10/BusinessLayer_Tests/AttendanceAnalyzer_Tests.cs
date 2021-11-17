using NUnit.Framework;
using Moq;
using BusinessLayer;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.MessageSenders;

namespace BusinessLayer_Tests
{
    public class AttendanceAnalyzer_Tests
    {
        private Models.Domain.Student[] _mockStudents;
        private Models.Domain.Lection[] _mockLections;
        private Models.Domain.Attendance[] _mockAttendance;
        private AttendanceAnalyzer _attendanceAnalyzer;

        private int cntEmail = 0;
        [OneTimeSetUp]
        public void Setup()
        {
            _mockStudents = new[]
            {
                new Models.Domain.Student()
            };
            _mockLections = new[]
            {
                new Models.Domain.Lection(),
                new Models.Domain.Lection(),
                new Models.Domain.Lection(),
                new Models.Domain.Lection(),
                new Models.Domain.Lection()
            };
            _mockAttendance = new[]
            {
                new Models.Domain.Attendance()
            };
            Mock<IStudentsService> mockStudentsService = new Mock<IStudentsService>(MockBehavior.Strict);
            mockStudentsService.Setup(service => service.TryGet(out _mockStudents, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            Mock<ILectionsService> mockLectionsService = new Mock<ILectionsService>(MockBehavior.Strict);
            mockLectionsService.Setup(service => service.TryGet(out _mockLections, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            Mock<IAttendanceService> mockAttendanceService = new Mock<IAttendanceService>(MockBehavior.Strict);
            mockAttendanceService.Setup(service => service.TryGet(out _mockAttendance, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            Mock<IEmailSender> mockEmailSender = new Mock<IEmailSender>(MockBehavior.Strict);
            mockEmailSender.Setup(sender => sender.Send(It.IsAny<Models.Domain.Email>(), It.IsAny<string>())).Callback(() => cntEmail++);
            mockEmailSender.SetupGet<Models.Domain.Email>(sender => sender.LectorOfCourseEmail).Returns(Models.Domain.Email.TryCreate("lect@mail.ru"));

            _attendanceAnalyzer = new AttendanceAnalyzer(mockStudentsService.Object, mockLectionsService.Object, mockAttendanceService.Object, mockEmailSender.Object);
        }

        [Test]
        public void TryCheckAttendance_Test()
        {
            bool res = _attendanceAnalyzer.TryCheckAttendance();
            Assert.IsTrue(res);
            Assert.AreEqual(2, cntEmail);
        }
    }
}
