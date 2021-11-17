using NUnit.Framework;
using Moq;
using DataLayer;
using BusinessLayer.ModelServices.Implementations;
using Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace BusinessLayer_Tests
{
    public class StudentsService_Tests
    {
        private Models.Database.Student[] _mockStudent;
        private StudentsService _studentsService;

        [OneTimeSetUp]
        public void Setup()
        {
            Mock<IDataAccess> mockDataAccess = new Mock<IDataAccess>(MockBehavior.Strict);
            mockDataAccess.Setup(acs => acs.TryAdd<IHasIdProperty<int>, int>(It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryDelete<IHasIdProperty<int>, int>(It.IsAny<int>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryUpdate<IHasIdProperty<int>, int>(It.IsAny<int>(), It.IsAny<IHasIdProperty<int>>())).Returns(true);
            mockDataAccess.Setup(acs => acs.TryGetStudents(out _mockStudent, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            Mock<IMapper> mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            mockMapper.Setup(map => map.Map<Models.Domain.Student[]>(It.IsAny<Models.Database.Student[]>())).Returns(new Models.Domain.Student[1]);
            mockMapper.Setup(map => map.Map<Models.Database.Student>(It.IsAny<Models.Domain.Student>())).Returns(new Models.Database.Student());

            Mock<ILogger> mockLogger = new Mock<ILogger>(MockBehavior.Loose);

            _studentsService = new StudentsService(mockMapper.Object, mockDataAccess.Object, mockLogger.Object);
        }

        [TestCase("Asher Ashur", "ash@mail.ru", "+79458965569")]
        public void TryAddValid_Test(string fio, string email, string phoneNumber)
        {
            bool res = _studentsService.TryAdd(new Models.Domain.Student()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email),
                PhoneNumber = Models.Domain.PhoneNumber.TryCreate(phoneNumber)
            });
            Assert.IsTrue(res);
        }

        [TestCase("", "ash@mail.ru", "+79458965569")]
        [TestCase("Asher Ashur", "ashasha", "+79458965569")]
        [TestCase("Asher Ashur", "ash@mail.ru", "+lalaa")]
        public void TryAddInvalid_Test(string fio, string email, string phoneNumber)
        {
            bool res = _studentsService.TryAdd(new Models.Domain.Student()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email),
                PhoneNumber = Models.Domain.PhoneNumber.TryCreate(phoneNumber)
            });
            Assert.IsFalse(res);
        }

        [TestCase(1)]
        public void TryDeleteValid_Test(int id)
        {
            bool res = _studentsService.TryDelete(id);
            Assert.IsTrue(res);
        }

        [TestCase(0)]
        public void TryDeleteInvalid_Test(int id)
        {
            bool res = _studentsService.TryDelete(id);
            Assert.IsFalse(res);
        }

        [TestCase(1, "Asher Ashur", "ash@mail.ru", "+79458965569")]
        public void TryUpdateValid_Test(int id, string fio, string email, string phoneNumber)
        {
            bool res = _studentsService.TryUpdate(id, new Models.Domain.Student()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email),
                PhoneNumber = Models.Domain.PhoneNumber.TryCreate(phoneNumber)
            });
            Assert.IsTrue(res);
        }

        [TestCase(0, "Asher Ashur", "ash@mail.ru", "+79458965569")]
        [TestCase(1, "", "ash@mail.ru", "+79458965569")]
        [TestCase(1, "Asher Ashur", "ashasha", "+79458965569")]
        [TestCase(1, "Asher Ashur", "ash@mail.ru", "+alala")]
        public void TryUpdateInvalid_Test(int id, string fio, string email, string phoneNumber)
        {
            bool res = _studentsService.TryUpdate(id, new Models.Domain.Student()
            {
                Fio = fio,
                Email = Models.Domain.Email.TryCreate(email),
                PhoneNumber = Models.Domain.PhoneNumber.TryCreate(phoneNumber)
            });
            Assert.IsFalse(res);
        }

        [TestCase(1, "Asher Ashur", "ash@mail.ru", "+79458965569")]
        public void TryGet_Test(int id, string fio, string email, string phoneNumber)
        {
            bool res = _studentsService.TryGet(out Models.Domain.Student[] att, id, fio, email, phoneNumber);
            Assert.IsTrue(res);
        }
    }
}
