using System.Collections.Generic;
using Models.Domain;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.ReportFormats;
using BusinessLayer.MessageSenders;

namespace BusinessLayer
{
    public class AttendanceAnalyzer
    {
        private IStudentsService _studentsService;
        private ILectionsService _lectionsService;
        private IAttendanceService _attendanceService;
        private IEmailSender _emailSender;

        public AttendanceAnalyzer(IStudentsService studentsService, ILectionsService lectionsService,
            IAttendanceService attendanceService, IEmailSender emailSender)
        {
            _studentsService = studentsService;
            _lectionsService = lectionsService;
            _attendanceService = attendanceService;
            _emailSender = emailSender;
        }

        public bool TryCheckAttendance()
        {
            if (!_studentsService.TryGet(out Student[] students))
            {
                return false;
            }
            if (!_lectionsService.TryGet(out Lection[] lections))
            {
                return false;
            }
            foreach (Student student in students)
            {
                if (!_attendanceService.TryGet(out Attendance[] attendance, studentId: student.Id))
                {
                    return false;
                }
                if (lections.Length - attendance.Length > 3)
                {
                    string message = "Dear " + student.Fio + "!" + System.Environment.NewLine +
                        "You have missed " + (lections.Length - attendance.Length) + " lections." +
                        " Please contact to our administrator to discuss this situation.";
                    _emailSender.Send(student.Email, message);
                    message = "Student " + student.Fio + " missed more than 3 lections.";
                    _emailSender.Send(_emailSender.LectorOfCourseEmail, message);
                }
            }
            return true;
        }
    }
}
