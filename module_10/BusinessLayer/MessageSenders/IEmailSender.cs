using Models.Domain;

namespace BusinessLayer.MessageSenders
{
    public interface IEmailSender
    {
        public Email LectorOfCourseEmail { get; }
        public void Send(Email sendTo, string message);
    }
}
