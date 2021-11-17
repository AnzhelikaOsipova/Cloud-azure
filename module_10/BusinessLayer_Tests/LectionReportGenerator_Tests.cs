using NUnit.Framework;
using Moq;
using BusinessLayer;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.ReportFormats;

namespace BusinessLayer_Tests
{
    public class LectionReportGenerator_Tests
    {
        private Models.Domain.Student[] _mockStudents;
        private Models.Domain.Lection[] _mockLections;
        private Models.Domain.Attendance[] _mockAttendance;
        private string _expectedJSONReport;
        private string _expectedXMLReport;

        private LectionReportGenerator _generator;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockStudents = new[]
            {
                new Models.Domain.Student()
                {
                    Fio = "stud1"
                },
                new Models.Domain.Student()
                {
                    Fio = "stud2"
                }
            };
            _mockLections = new[]
            {
                new Models.Domain.Lection()
                {
                    Topic = "lect1",
                    Date = Models.Domain.Date.TryCreate("20.10.2021")
                }
            };
            _mockAttendance = new[]
            {
                new Models.Domain.Attendance()
            };
            _expectedJSONReport = "[\r\n  {\r\n    \"StudentFio\": \"stud1\",\r\n    \"LectionTopic\": \"lect1\",\r\n    \"Date\": \"20.10.2021\",\r\n    \"Attendance\": \"attended\"\r\n  },\r\n  {\r\n    \"StudentFio\": \"stud2\",\r\n    \"LectionTopic\": \"lect1\",\r\n    \"Date\": \"20.10.2021\",\r\n    \"Attendance\": \"attended\"\r\n  }\r\n]";
            _expectedXMLReport = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfReport xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Report>\r\n    <StudentFio>stud1</StudentFio>\r\n    <LectionTopic>lect1</LectionTopic>\r\n    <Date>20.10.2021</Date>\r\n    <Attendance>attended</Attendance>\r\n  </Report>\r\n  <Report>\r\n    <StudentFio>stud2</StudentFio>\r\n    <LectionTopic>lect1</LectionTopic>\r\n    <Date>20.10.2021</Date>\r\n    <Attendance>attended</Attendance>\r\n  </Report>\r\n</ArrayOfReport>";
            Mock<IStudentsService> mockStudentsService = new Mock<IStudentsService>(MockBehavior.Strict);
            mockStudentsService.Setup(service => service.TryGet(out _mockStudents, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            Mock<ILectionsService> mockLectionsService = new Mock<ILectionsService>(MockBehavior.Strict);
            mockLectionsService.Setup(service => service.TryGet(out _mockLections, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            Mock<IAttendanceService> mockAttendanceService = new Mock<IAttendanceService>(MockBehavior.Strict);
            mockAttendanceService.Setup(service => service.TryGet(out _mockAttendance, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            _generator = new LectionReportGenerator(mockStudentsService.Object, mockLectionsService.Object, mockAttendanceService.Object);
        }

        [Test]
        public void TryMakeJSONReportAboutLection_Test()
        {
            bool res = _generator.TryMakeReportAboutLection("lect1", new ReportJSONConverter(), out string convertedReport);
            Assert.AreEqual(_expectedJSONReport, convertedReport);
        }

        [Test]
        public void TryMakeXMLReportAboutLection_Test()
        {
            bool res = _generator.TryMakeReportAboutLection("lect1", new ReportXMLConverter(), out string convertedReport);
            Assert.AreEqual(_expectedXMLReport, convertedReport);
        }
    }
}
