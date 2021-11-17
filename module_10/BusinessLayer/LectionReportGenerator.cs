using System.Collections.Generic;
using Models.Domain;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.ReportFormats;

namespace BusinessLayer
{
    public class LectionReportGenerator
    {
        private IStudentsService _studentsService;
        private ILectionsService _lectionsService;
        private IAttendanceService _attendanceService;
        public LectionReportGenerator(IStudentsService studentsService, ILectionsService lectionsService,
            IAttendanceService attendanceService)
        {
            _studentsService = studentsService;
            _lectionsService = lectionsService;
            _attendanceService = attendanceService;
        }

        public bool TryMakeReportAboutLection(string lectionTopic, IReportFormatConverter reportFormatConverter, out string convertedReport)
        {
            convertedReport = null;
            if (reportFormatConverter is null || lectionTopic is null)
            {
                return false;
            }
            if (!_lectionsService.TryGet(out Lection[] lections, topic: lectionTopic))
            {
                return false;
            }
            if (!_studentsService.TryGet(out Student[] students))
            {
                return false;
            }
            List<Report> reports = new List<Report>();
            foreach (Lection lection in lections)
            {
                foreach (Student student in students)
                {
                    if (!_attendanceService.TryGet(out Attendance[] attendance, lectionId: lection.Id, studentId: student.Id))
                    {
                        return false;
                    }
                    string isAttended;
                    if (attendance.Length != 0)
                    {
                        isAttended = "attended";
                    }
                    else
                    {
                        isAttended = "missed";
                    }
                    reports.Add(new Report()
                    {
                        StudentFio = student.Fio,
                        LectionTopic = lection.Topic,
                        Date = lection.Date.CorrectDate.ToString("dd.MM.yyyy"),
                        Attendance = isAttended
                    });
                }
            }
            convertedReport = reportFormatConverter.Convert(reports);
            return true;
        }
    }
}
