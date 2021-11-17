using NUnit.Framework;
using Moq;
using DataLayer;
using BusinessLayer.ModelServices.Implementations;
using Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace BusinessLayer_Tests
{
    public class AttendanceService_Tests
    {
        private Models.Database.Attendance[] _mockAttendance;
        private AttendanceService _attendanceService;

        [OneTimeSetUp]
        public void Setup()
        {
            Mock<IDataAccess> mockDataAccess = new Mock<IDataAccess>(MockBehavior.Strict);
            mockDataAccess.Setup(acs => acs.TryAdd<IHasIdProperty<int>, int>(It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryDelete<IHasIdProperty<int>, int>(It.IsAny<int>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryUpdate<IHasIdProperty<int>, int>(It.IsAny<int>(), It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryGetAttendance(out _mockAttendance, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            Mock<IMapper> mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            mockMapper.Setup(map => map.Map<Models.Domain.Attendance[]>(It.IsAny<Models.Database.Attendance[]>())).Returns(new Models.Domain.Attendance[1]);
            mockMapper.Setup(map => map.Map<Models.Database.Attendance>(It.IsAny<Models.Domain.Attendance>())).Returns(new Models.Database.Attendance());

            Mock<ILogger> mockLogger = new Mock<ILogger>(MockBehavior.Loose);
            
            _attendanceService = new AttendanceService(mockMapper.Object, mockDataAccess.Object, mockLogger.Object);
        }

        [TestCase(1, 1)]
        public void TryAddValid_Test(int lectionId, int studentId)
        {
            bool res = _attendanceService.TryAdd(new Models.Domain.Attendance()
            {
                LectionId = lectionId,
                StudentId = studentId
            });
            Assert.IsTrue(res);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void TryAddInvalid_Test(int lectionId, int studentId)
        {
            bool res = _attendanceService.TryAdd(new Models.Domain.Attendance()
            {
                LectionId = lectionId,
                StudentId = studentId
            });
            Assert.IsFalse(res);
        }

        [TestCase(1)]
        public void TryDeleteValid_Test(int id)
        {
            bool res = _attendanceService.TryDelete(id);
            Assert.IsTrue(res);
        }

        [TestCase(0)]
        public void TryDeleteInvalid_Test(int id)
        {
            bool res = _attendanceService.TryDelete(id);
            Assert.IsFalse(res);
        }

        [TestCase(1, 1, 1)]
        public void TryUpdateValid_Test(int id, int lectionId, int studentId)
        {
            bool res = _attendanceService.TryUpdate(id, new Models.Domain.Attendance()
            {
                LectionId = lectionId,
                StudentId = studentId
            });
            Assert.IsTrue(res);
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        public void TryUpdateInvalid_Test(int id, int lectionId, int studentId)
        {
            bool res = _attendanceService.TryUpdate(id, new Models.Domain.Attendance()
            {
                LectionId = lectionId,
                StudentId = studentId
            });
            Assert.IsFalse(res);
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 1, 1)]
        public void TryGet_Test(int id, int lectionId, int studentId)
        {
            bool res = _attendanceService.TryGet(out Models.Domain.Attendance[] att, id, lectionId, studentId);
            Assert.IsTrue(res);
        }
    }
}