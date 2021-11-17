using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging;
using DataLayer;
using Models.Database;
using System.Collections.Generic;

namespace DataLayer_Tests
{
    public class DataAccess_Tests
    {
        private DataAccess _dataAccess;
        private Lector[] _testLectors;
        private Student[] _testStudents;
        private Lection[] _testLections;
        private Homework[] _testHomeworks;
        private Attendance[] _testAttendance;

        [OneTimeSetUp]
        public void Init()
        {
            LoggerFactory log = new LoggerFactory();
            log.AddProvider(new DebugLoggerProvider());

            _dataAccess = new DataAccess(new DbContextOptionsBuilder().UseInMemoryDatabase("TestDb").Options,
                log.CreateLogger("Test"));
            _testLectors = new[]
            {
                new Lector()
                {
                    Fio = "Asalia Asaliva",
                    Email = "asalia@mail.ru"
                },
                new Lector()
                {
                    Fio = "Ashat Artur",
                    Email = "ashat.artur@gmail.com"
                },
                new Lector()
                {
                    Fio = "Gennadiy",
                    Email = "gena@epam.com"
                }
            };

            _testStudents = new[]
            {
                new Student()
                {
                    Fio = "Katya Malinova",
                    Email = "katya@mail.ru",
                    PhoneNumber = "88956548898"
                },
                new Student()
                {
                    Fio = "Alex Smirnov",
                    Email = "alik@mail.ru",
                    PhoneNumber = "+78945695412"
                },
                new Student()
                {
                    Fio = "Alegorov Vasya",
                    Email = "al.vas@yandex.ru",
                    PhoneNumber = "86595412214"
                }
            };

            _testLections = new[]
            {
                new Lection()
                {
                   Topic = "Introduction",
                   Date = "20.10.2021",
                   LectorId = 1
                },
                new Lection()
                {
                    Topic = "Definitions",
                   Date = "23.10.2021",
                   LectorId = 2
                },
                new Lection()
                {
                    Topic = "Services",
                   Date = "02.10.2021",
                   LectorId = 1
                }
            };

            _testHomeworks = new[]
            {
                new Homework()
                {
                   StudentId = 1,
                   LectionId = 1,
                   Mark = 5
                },
                new Homework()
                {
                   StudentId = 1,
                   LectionId = 2,
                   Mark = 3
                },
                new Homework()
                {
                   StudentId = 2,
                   LectionId = 2,
                   Mark = 4
                }
            };
            _testAttendance = new[]
            {
                new Attendance()
                {
                   StudentId = 1,
                   LectionId = 1
                },
                new Attendance()
                {
                   StudentId = 1,
                   LectionId = 2
                },
                new Attendance()
                {
                   StudentId = 2,
                   LectionId = 2
                }
            };
        }

        [Test]
        public void AddTest()
        {
            foreach (Lector lector in _testLectors)
            {
                bool added = _dataAccess.TryAdd<Lector, int>(lector);
                Assert.IsTrue(added);
            }
            bool got = _dataAccess.TryGetLectors(out Lector[] lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(_testLectors.Length, lectors.Length);
            for (int i = 0; i < lectors.Length; i++)
            {
                Assert.AreEqual(_testLectors[i].Fio, lectors[i].Fio);
                Assert.AreEqual(_testLectors[i].Email, lectors[i].Email);
            }
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void DeleteTest()
        {
            foreach (Lector lector in _testLectors)
            {
                _dataAccess.TryAdd<Lector, int>(lector);
            }
            bool got = _dataAccess.TryGetLectors(out Lector[] lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(3, lectors.Length);
            for (int i = 1; i <= 3; i++)
            {
                _dataAccess.TryDelete<Lector, int>(i);
            }
            got = _dataAccess.TryGetLectors(out lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(0, lectors.Length);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void DeleteOneTest()
        {
            foreach (Lector lector in _testLectors)
            {
                _dataAccess.TryAdd<Lector, int>(lector);
            }
            bool got = _dataAccess.TryGetLectors(out Lector[] lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(3, lectors.Length);

            int indexToDelete = 1;
            _dataAccess.TryDelete<Lector, int>(indexToDelete);
            got = _dataAccess.TryGetLectors(out lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(2, lectors.Length);
            Assert.IsTrue(lectors[0].Id != indexToDelete && lectors[1].Id != indexToDelete);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void UpdateTest()
        {
            foreach (Lector lector in _testLectors)
            {
                _dataAccess.TryAdd<Lector, int>(lector);
            }
            bool got = _dataAccess.TryGetLectors(out Lector[] lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(3, lectors.Length);

            int indexToUpdate = 3;
            Lector updatedLector = new Lector()
            {
                Fio = "Vasiliev Vasiliy",
                Email = "vasia@mail.ru"
            };
            _dataAccess.TryUpdate<Lector, int>(indexToUpdate, updatedLector);
            got = _dataAccess.TryGetLectors(out lectors);
            Assert.IsTrue(got);
            Assert.AreEqual(3, lectors.Length);
            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(_testLectors[i].Fio, lectors[i].Fio);
                Assert.AreEqual(_testLectors[i].Email, lectors[i].Email);
            }
            Assert.AreEqual(updatedLector.Fio, lectors[2].Fio);
            Assert.AreEqual(updatedLector.Email, lectors[2].Email);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void GetLectorWithFiltersTest()
        {
            foreach (Lector lector in _testLectors)
            {
                _dataAccess.TryAdd<Lector, int>(lector);
            }
            bool got = _dataAccess.TryGetLectors(out Lector[] lectors, id: 2, fio: "Ashat Artur", email: "ashat.artur@gmail.com");
            Assert.AreEqual(1, lectors.Length);
            Assert.AreEqual(_testLectors[1].Fio, lectors[0].Fio);
            Assert.AreEqual(_testLectors[1].Email, lectors[0].Email);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void GetStudentWithFiltersTest()
        {
            foreach (Student student in _testStudents)
            {
                _dataAccess.TryAdd<Student, int>(student);
            }
            bool got = _dataAccess.TryGetStudents(out Student[] students, id: 2, fio: "Alex Smirnov",  email: "alik@mail.ru", phoneNumber: "+78945695412");
            Assert.AreEqual(1, students.Length);
            Assert.AreEqual(_testStudents[1].Fio, students[0].Fio);
            Assert.AreEqual(_testStudents[1].Email, students[0].Email);
            Assert.AreEqual(_testStudents[1].PhoneNumber, students[0].PhoneNumber);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void GetLectionWithFiltersTest()
        {
            foreach (Lection lection in _testLections)
            {
                _dataAccess.TryAdd<Lection, int>(lection);
            }
            bool got = _dataAccess.TryGetLections(out Lection[] lections, topic: "Definitions", date: "23.10.2021", lectorId: 2);
            Assert.AreEqual(1, lections.Length);
            Assert.AreEqual(_testLections[1].Topic, lections[0].Topic);
            Assert.AreEqual(_testLections[1].Date, lections[0].Date);
            Assert.AreEqual(_testLections[1].LectorId, lections[0].LectorId);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void GetHomeworkWithFiltersTest()
        {
            foreach (Homework homework in _testHomeworks)
            {
                _dataAccess.TryAdd<Homework, int>(homework);
            }
            bool got = _dataAccess.TryGetHomeworks(out Homework[] lections, id: 3, studentId: 2, lectionId: 2);
            Assert.AreEqual(1, lections.Length);
            Assert.AreEqual(_testHomeworks[2].StudentId, lections[0].StudentId);
            Assert.AreEqual(_testHomeworks[2].LectionId, lections[0].LectionId);
            Assert.AreEqual(_testHomeworks[2].Mark, lections[0].Mark);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void GetAttendanceWithFiltersTest()
        {
            foreach (Attendance iattendance in _testAttendance)
            {
                _dataAccess.TryAdd<Attendance, int>(iattendance);
            }
            bool got = _dataAccess.TryGetAttendance(out Attendance[] attendance, id: 3, studentId: 2, lectionId: 2);
            Assert.AreEqual(1, attendance.Length);
            Assert.AreEqual(_testAttendance[2].StudentId, attendance[0].StudentId);
            Assert.AreEqual(_testAttendance[2].LectionId, attendance[0].LectionId);
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void TryGetInvalidLectorTest()
        {
            foreach (Lector lector in _testLectors)
            {
                _dataAccess.TryAdd<Lector, int>(lector);
            }
            _dataAccess.TryAdd<Lector, int>(new Lector()
            {
                Fio = "Lecto Lect",
                Email = "alalamail"
            });
            _dataAccess.TryAdd<Lector, int>(new Lector()
            {
                Fio = "",
                Email = "lectoSecond@mail.ru"
            });
            bool got = _dataAccess.TryGetLectors(out Lector[] lectors);
            Assert.AreEqual(_testLectors.Length, lectors.Length);
            foreach (Lector lector1 in lectors)
            {
                Assert.IsTrue(lector1.Id != 4 &&
                    lector1.Id != 5);
            }
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void TryGetInvalidStudentTest()
        {
            foreach (Student student in _testStudents)
            {
                _dataAccess.TryAdd<Student, int>(student);
            }
            _dataAccess.TryAdd<Student, int>(new Student()
            {
                Fio = "Stude Stud",
                Email = "alalamail",
                PhoneNumber = "+79856423359"
            });
            _dataAccess.TryAdd<Student, int>(new Student()
            {
                Fio = "",
                Email = "StudeSecond@mail.ru",
                PhoneNumber = "89654265547"
            });
            _dataAccess.TryAdd<Student, int>(new Student()
            {
                Fio = "Stude Third",
                Email = "StudeThird@mail.ru",
                PhoneNumber = "SomePhoneNumber"
            });
            bool got = _dataAccess.TryGetStudents(out Student[] students);
            Assert.AreEqual(_testStudents.Length, students.Length);
            foreach (Student student1 in students)
            {
                Assert.IsTrue(student1.Id != 4 &&
                    student1.Id != 5 &&
                    student1.Id != 6);
            }
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void TryGetInvalidLectionTest()
        {
            foreach (Lection lection in _testLections)
            {
                _dataAccess.TryAdd<Lection, int>(lection);
            }
            _dataAccess.TryAdd<Lection, int>(new Lection()
            {
                Topic = "",
                Date = "25.10.2021",
                LectorId = 2
            });
            _dataAccess.TryAdd<Lection, int>(new Lection()
            {
                Topic = "Access",
                Date = "25102021",
                LectorId = 1
            });
            _dataAccess.TryAdd<Lection, int>(new Lection()
            {
                Topic = "Final",
                Date = "26.10.2021",
                LectorId = -1
            });
            bool got = _dataAccess.TryGetLections(out Lection[] lections);
            Assert.AreEqual(_testLections.Length, lections.Length);
            foreach (Lection lection1 in lections)
            {
                Assert.IsTrue(lection1.Id != 4 &&
                    lection1.Id != 5 &&
                    lection1.Id != 6);
            }
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void TryGetInvalidHomeworkTest()
        {
            foreach (Homework homework in _testHomeworks)
            {
                _dataAccess.TryAdd<Homework, int>(homework);
            }
            _dataAccess.TryAdd<Homework, int>(new Homework()
            {
                StudentId = -1,
                LectionId = 1,
                Mark = 5
            });
            _dataAccess.TryAdd<Homework, int>(new Homework()
            {
                StudentId = 1,
                LectionId = -1,
                Mark = 5
            });
            _dataAccess.TryAdd<Homework, int>(new Homework()
            {
                StudentId = 2,
                LectionId = 2,
                Mark = 6
            });
            bool got = _dataAccess.TryGetHomeworks(out Homework[] homeworks);
            Assert.AreEqual(_testHomeworks.Length, homeworks.Length);
            foreach (Homework homework1 in homeworks)
            {
                Assert.IsTrue(homework1.Id != 4 &&
                    homework1.Id != 5 &&
                    homework1.Id != 6);
            }
            _dataAccess.ReCreateDatabase();
        }

        [Test]
        public void TryGetInvalidAttendanceTest()
        {
            foreach (Attendance attendance1 in _testAttendance)
            {
                _dataAccess.TryAdd<Attendance, int>(attendance1);
            }
            _dataAccess.TryAdd<Attendance, int>(new Attendance()
            {
                StudentId = -1,
                LectionId = 1,
            });
            _dataAccess.TryAdd<Attendance, int>(new Attendance()
            {
                StudentId = 1,
                LectionId = -1,
            });
            bool got = _dataAccess.TryGetAttendance(out Attendance[] attendance);
            Assert.AreEqual(_testAttendance.Length, attendance.Length);
            foreach (Attendance attendance1 in attendance)
            {
                Assert.IsTrue(attendance1.Id != 4 &&
                    attendance1.Id != 5);
            }
            _dataAccess.ReCreateDatabase();
        }
    }
}