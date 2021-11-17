using NUnit.Framework;
using Moq;
using DataLayer;
using BusinessLayer.ModelServices.Implementations;
using Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace BusinessLayer_Tests
{
    public class LectorsService_Tests
    {
        private Models.Database.Lector[] _mockLector;
        private LectorsService _lectorsService;

        [OneTimeSetUp]
        public void Setup()
        {
            Mock<IDataAccess> mockDataAccess = new Mock<IDataAccess>(MockBehavior.Strict);
            mockDataAccess.Setup(acs => acs.TryAdd<IHasIdProperty<int>, int>(It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryDelete<IHasIdProperty<int>, int>(It.IsAny<int>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryUpdate<IHasIdProperty<int>, int>(It.IsAny<int>(), It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryGetLectors(out _mockLector, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            Mock<IMapper> mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            mockMapper.Setup(map => map.Map<Models.Domain.Lector[]>(It.IsAny<Models.Database.Lector[]>())).Returns(new Models.Domain.Lector[1]);
            mockMapper.Setup(map => map.Map<Models.Database.Lector>(It.IsAny<Models.Domain.Lector>())).Returns(new Models.Database.Lector());

            Mock<ILogger> mockLogger = new Mock<ILogger>(MockBehavior.Loose);

            _lectorsService = new LectorsService(mockMapper.Object, mockDataAccess.Object, mockLogger.Object);
        }

        [TestCase("Asher Ashur", "ash@mail.ru")]
        public void TryAddValid_Test(string fio, string email)
        {
            bool res = _lectorsService.TryAdd(new Models.Domain.Lector()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email)
            });
            Assert.IsTrue(res);
        }

        [TestCase("", "ash@mail.ru")]
        [TestCase("Asher Ashur", "ashasha")]
        public void TryAddInvalid_Test(string fio, string email)
        {
            bool res = _lectorsService.TryAdd(new Models.Domain.Lector()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email)
            });
            Assert.IsFalse(res);
        }

        [TestCase(1)]
        public void TryDeleteValid_Test(int id)
        {
            bool res = _lectorsService.TryDelete(id);
            Assert.IsTrue(res);
        }

        [TestCase(0)]
        public void TryDeleteInvalid_Test(int id)
        {
            bool res = _lectorsService.TryDelete(id);
            Assert.IsFalse(res);
        }

        [TestCase(1, "Asher Ashur", "ash@mail.ru")]
        public void TryUpdateValid_Test(int id, string fio, string email)
        {
            bool res = _lectorsService.TryUpdate(id, new Models.Domain.Lector()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email)
            });
            Assert.IsTrue(res);
        }

        [TestCase(0, "Asher Ashur", "ash@mail.ru")]
        [TestCase(1, "", "ash@mail.ru")]
        [TestCase(1, "Asher Ashur", "ashasha")]
        public void TryUpdateInvalid_Test(int id, string fio, string email)
        {
            bool res = _lectorsService.TryUpdate(id, new Models.Domain.Lector()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email)
            });
            Assert.IsFalse(res);
        }

        [TestCase(1, "Asher Ashur", "ash@mail.ru")]
        public void TryGet_Test(int id, string fio, string email)
        {
            bool res = _lectorsService.TryGet(out Models.Domain.Lector[] att, id, fio, email);
            Assert.IsTrue(res);
        }
    }
}
