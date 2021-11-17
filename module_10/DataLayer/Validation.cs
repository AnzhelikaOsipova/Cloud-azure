using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models;
using Models.Database;

namespace DataLayer
{
    public static class Validation
    {
        public static bool IsAllLectorsValid(IEnumerable<Lector> lectors, out IEnumerable<Lector> validLectors, 
            out string invalidLectorsId)
        {
            bool allValid = true;
            invalidLectorsId = "";
            var invalidLectors = lectors.
                Where(lect => lect.Fio == "" ||
                    !IsValidEmail(lect.Email));
            if (invalidLectors.Any())
            {
                allValid = false;
                foreach (Lector lector in invalidLectors)
                {
                    invalidLectorsId = String.Join(" ", invalidLectorsId, lector.Id); 
                }
            }
            validLectors = lectors.Except(invalidLectors, new DBComparer<Lector>());
            return allValid;
        }

        public static bool IsAllStudentsValid(IEnumerable<Student> students, out IEnumerable<Student> validStudents,
            out string invalidStudentsId)
        {
            bool allValid = true;
            invalidStudentsId = "";
            var invalidStudents = students.
                Where(stud => stud.Fio == "" ||
                !IsValidEmail(stud.Email) ||
                !IsValidPhoneNumber(stud.PhoneNumber));
            if (invalidStudents.Any())
            {
                allValid = false;
                foreach (Student student in invalidStudents)
                {
                    invalidStudentsId = String.Join(" ", invalidStudentsId, student.Id);
                }
            }
            validStudents = students.Except(invalidStudents, new DBComparer<Student>());
            return allValid;
        }

        public static bool IsAllLectionsValid(IEnumerable<Lection> lections, out IEnumerable<Lection> validLections,
            out string invalidLectionsId)
        {
            bool allValid = true;
            invalidLectionsId = "";
            var invalidLections = lections.
                Where(lect => lect.Topic == "" ||
                lect.LectorId < 1 ||
                !IsValidDate(lect.Date)).ToArray();
            if (invalidLections.Any())
            {
                allValid = false;
                foreach (Lection lection in invalidLections)
                {
                    invalidLectionsId = String.Join(" ", invalidLectionsId, lection.Id);
                }
            }
            validLections = lections.Except(invalidLections, new DBComparer<Lection>());
            return allValid;
        }

        public static bool IsAllAttendanceValid(IEnumerable<Attendance> attendance, out IEnumerable<Attendance> validAttendance,
            out string invalidAttendanceId)
        {
            bool allValid = true;
            invalidAttendanceId = "";
            var invalidAttendance = attendance.
                Where(att => att.LectionId < 1 ||
                att.StudentId < 1).ToArray();
            if (invalidAttendance.Any())
            {
                allValid = false;
                foreach (Attendance att in invalidAttendance)
                {
                    invalidAttendanceId = String.Join(" ", invalidAttendanceId, att.Id);
                }
            }
            validAttendance = attendance.Except(invalidAttendance, new DBComparer<Attendance>());
            return allValid;
        }

        public static bool IsAllHomeworksValid(IEnumerable<Homework> homeworks, out IEnumerable<Homework> validHomeworks,
            out string invalidHomeworksId)
        {
            bool allValid = true;
            invalidHomeworksId = "";
            var invalidHomeworks = homeworks.
                Where(work => work.LectionId < 1 ||
                work.StudentId < 1 ||
                !IsValidMark(work.Mark));
            if (invalidHomeworks.Any())
            {
                allValid = false;
                foreach (Homework work in invalidHomeworks)
                {
                    invalidHomeworksId = String.Join(" ", invalidHomeworksId, work.Id);
                }
            }
            validHomeworks = homeworks.Except(invalidHomeworks, new DBComparer<Homework>());
            return allValid;
        }

        private class DBComparer<T> : IEqualityComparer<T>
           where T : IHasIdProperty<int>
        {
            public bool Equals(T x, T y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(T obj)
            {
                return obj.Id;
            }
        }

        private static bool IsValidEmail(string emailStr)
        {
            if (emailStr is null)
            {
                return false;
            }
            try
            {
                emailStr = emailStr.TrimEnd();
                var addr = new System.Net.Mail.MailAddress(emailStr);
                return addr.Address == emailStr;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber is null)
            {
                return false;
            }
            phoneNumber = phoneNumber.TrimEnd();
            Regex[] regs = new[]
            {
                new Regex(@"^\+\d\d{3}\d{3}\d{2}\d{2}$"),
                new Regex(@"^\d\d{3}\d{3}\d{2}\d{2}$")
            };
            foreach (Regex regex in regs)
            {
                if (regex.IsMatch(phoneNumber))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidDate(string date)
        {
            date = date.TrimEnd();
            if (!DateTime.TryParseExact(date, "dd.MM.yyyy", new CultureInfo("en-US"), 
                DateTimeStyles.None, out DateTime validDate))
            {
                return false;
            }
            return true;
        }

        private static bool IsValidMark(int mark)
        {
            if (mark < 0 || mark > 5)
            {
                return false;
            }
            return true;
        }
    }
}
