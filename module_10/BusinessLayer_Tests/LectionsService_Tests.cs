using NUnit.Framework;
using Moq;
using DataLayer;
using BusinessLayer.ModelServices.Implementations;
using Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace BusinessLayer_Tests
{
    public class LectionsService_Tests
    {
        private Models.Database.Lection[] _mockLection;
        private LectionsService _lectionsService;

        [OneTimeSetUp]
        public void Setup()
        {
            Mock<IDataAccess> mockDataAccess = new Mock<IDataAccess>(MockBehavior.Strict);
            mockDataAccess.Setup(acs => acs.TryAdd<IHasIdProperty<int>, int>(It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryDelete<IHasIdProperty<int>, int>(It.IsAny<int>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryUpdate<IHasIdProperty<int>, int>(It.IsAny<int>(), It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryGetLections(out _mockLection, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            Mock<IMapper> mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            mockMapper.Setup(map => map.Map<Models.Domain.Lection[]>(It.IsAny<Models.Database.Lection[]>())).Returns(new Models.Domain.Lection[1]);
            mockMapper.Setup(map => map.Map<Models.Database.Lection>(It.IsAny<Models.Domain.Lection>())).Returns(new Models.Database.Lection());

            Mock<ILogger> mockLogger = new Mock<ILogger>(MockBehavior.Loose);

            _lectionsService = new LectionsService(mockMapper.Object, mockDataAccess.Object, mockLogger.Object);
        }

        [TestCase("Introduction", "21.10.2021", 1)]
        public void TryAddValid_Test(string topic, string date, int lectorId)
        {
            bool res = _lectionsService.TryAdd(new Models.Domain.Lection()
            {
                Topic = topic,
                Date = Models.Domain.Date.TryCreate(date),
                LectorId = lectorId
            });
            Assert.IsTrue(res);
        }

        [TestCase("", "21.10.2021", 5)]
        [TestCase("Introduction", "21102021", 5)]
        [TestCase("Introduction", "21.10.2021", 0)]
        public void TryAddInvalid_Test(string topic, string date, int lectorId)
        {
            bool res = _lectionsService.TryAdd(new Models.Domain.Lection()
            {
                Topic = topic,
                Date = Models.Domain.Date.TryCreate(date),
                LectorId = lectorId
            });
            Assert.IsFalse(res);
        }

        [TestCase(1)]
        public void TryDeleteValid_Test(int id)
        {
            bool res = _lectionsService.TryDelete(id);
            Assert.IsTrue(res);
        }

        [TestCase(0)]
        public void TryDeleteInvalid_Test(int id)
        {
            bool res = _lectionsService.TryDelete(id);
            Assert.IsFalse(res);
        }

        [TestCase(1, "Introduction", "21.10.2021", 1)]
        public void TryUpdateValid_Test(int id, string topic, string date, int lectorId)
        {
            bool res = _lectionsService.TryUpdate(id, new Models.Domain.Lection()
            {
                Topic = topic,
                Date = Models.Domain.Date.TryCreate(date),
                LectorId = lectorId
            });
            Assert.IsTrue(res);
        }

        [TestCase(0, "Introduction", "21.10.2021", 1)]
        [TestCase(1, "", "21.10.2021", 1)]
        [TestCase(1, "Introduction", "21102021", 1)]
        [TestCase(1, "Introduction", "21.10.2021", 0)]
        public void TryUpdateInvalid_Test(int id, string topic, string date, int lectorId)
        {
            bool res = _lectionsService.TryUpdate(id, new Models.Domain.Lection()
            {
                Topic = topic,
                Date = Models.Domain.Date.TryCreate(date),
                LectorId = lectorId
            });
            Assert.IsFalse(res);
        }

        [TestCase(1, "Introduction", "21.10.2021", 1)]
        public void TryGet_Test(int id, string topic, string date, int lectorId)
        {
            bool res = _lectionsService.TryGet(out Models.Domain.Lection[] att, id, topic, date, lectorId);
            Assert.IsTrue(res);
        }
    }
}
