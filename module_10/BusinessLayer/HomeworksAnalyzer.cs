using Models.Domain;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.MessageSenders;


namespace BusinessLayer
{
    public class HomeworksAnalyzer
    {
        private IStudentsService _studentsService;
        private IHomeworksService _homeworksService;
        private ISMSSender _smsSender;

        public HomeworksAnalyzer(IStudentsService studentsService, IHomeworksService homeworksService,
            ISMSSender smsSender)
        {
            _studentsService = studentsService;
            _homeworksService = homeworksService;
            _smsSender = smsSender;
        }

        public bool TryCheckMeanMark()
        {
            if (!_studentsService.TryGet(out Student[] students))
            {
                return false;
            }
            foreach (Student student in students)
            {
                if (!_homeworksService.TryGet(out Homework[] homeworks, studentId: student.Id))
                {
                    return false;
                }
                int countMarks = 0;
                foreach (Homework homework in homeworks)
                {
                    countMarks += homework.Mark.CorrectMark;
                }
                if (countMarks * 1.0 / homeworks.Length < 4)
                {
                    string message = "Dear " + student.Fio + "!" + System.Environment.NewLine +
                        "Please, take into account, that your mean mark on course is " + (countMarks * 1.0 / homeworks.Length) +
                        ". If you have any problems, contact to our administrator.";
                    _smsSender.Send(student.PhoneNumber, message);
                }
            }
            return true;
        }
    }
}
