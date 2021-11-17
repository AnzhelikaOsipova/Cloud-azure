using NUnit.Framework;
using Moq;
using DataLayer;
using BusinessLayer.ModelServices.Implementations;
using Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace BusinessLayer_Tests
{
    public class HomeworksService_Tests
    {
        private Models.Database.Homework[] _mockHomework;
        private HomeworksService _homeworksService;

        [OneTimeSetUp]
        public void Setup()
        {
            Mock<IDataAccess> mockDataAccess = new Mock<IDataAccess>(MockBehavior.Strict);
            mockDataAccess.Setup(acs => acs.TryAdd<IHasIdProperty<int>, int>(It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryDelete<IHasIdProperty<int>, int>(It.IsAny<int>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryUpdate<IHasIdProperty<int>, int>(It.IsAny<int>(), It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryGetHomeworks(out _mockHomework, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            Mock<IMapper> mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            mockMapper.Setup(map => map.Map<Models.Domain.Homework[]>(It.IsAny<Models.Database.Homework[]>())).Returns(new Models.Domain.Homework[1]);
            mockMapper.Setup(map => map.Map<Models.Database.Homework>(It.IsAny<Models.Domain.Homework>())).Returns(new Models.Database.Homework());

            Mock<ILogger> mockLogger = new Mock<ILogger>(MockBehavior.Loose);

            _homeworksService = new HomeworksService(mockMapper.Object, mockDataAccess.Object, mockLogger.Object);
        }

        [TestCase(1, 1, 5)]
        public void TryAddValid_Test(int lectionId, int studentId, int mark)
        {
            bool res = _homeworksService.TryAdd(new Models.Domain.Homework()
            {
                LectionId = lectionId,
                StudentId = studentId,
                Mark = Models.Domain.Mark.TryCreate(mark)
            });
            Assert.IsTrue(res);
        }

        [TestCase(-1, 1, 5)]
        [TestCase(1, -1, 5)]
        [TestCase(1, 1, 6)]
        public void TryAddInvalid_Test(int lectionId, int studentId, int mark)
        {
            bool res = _homeworksService.TryAdd(new Models.Domain.Homework()
            {
                LectionId = lectionId,
                StudentId = studentId,
                Mark = Models.Domain.Mark.TryCreate(mark)
            });
            Assert.IsFalse(res);
        }

        [TestCase(1)]
        public void TryDeleteValid_Test(int id)
        {
            bool res = _homeworksService.TryDelete(id);
            Assert.IsTrue(res);
        }

        [TestCase(0)]
        public void TryDeleteInvalid_Test(int id)
        {
            bool res = _homeworksService.TryDelete(id);
            Assert.IsFalse(res);
        }

        [TestCase(1, 1, 1, 5)]
        public void TryUpdateValid_Test(int id, int lectionId, int studentId, int mark)
        {
            bool res = _homeworksService.TryUpdate(id, new Models.Domain.Homework()
            {
                LectionId = lectionId,
                StudentId = studentId,
                Mark = Models.Domain.Mark.TryCreate(mark)
            });
            Assert.IsTrue(res);
        }

        [TestCase(0, 1, 1, 5)]
        [TestCase(1, 0, 1, 5)]
        [TestCase(1, 1, 0, 5)]
        [TestCase(1, 1, 1, -1)]
        public void TryUpdateInvalid_Test(int id, int lectionId, int studentId, int mark)
        {
            bool res = _homeworksService.TryUpdate(id, new Models.Domain.Homework()
            {
                LectionId = lectionId,
                StudentId = studentId,
                Mark = Models.Domain.Mark.TryCreate(mark)
            });
            Assert.IsFalse(res);
        }

        [TestCase(1, 1, 1, 1)]
        [TestCase(0, 1, 1, 1)]
        public void TryGet_Test(int id, int lectionId, int studentId, int mark)
        {
            bool res = _homeworksService.TryGet(out Models.Domain.Homework[] att, id, lectionId, studentId, mark);
            Assert.IsTrue(res);
        }
    }
}
