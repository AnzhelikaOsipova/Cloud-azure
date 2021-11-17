using Models.Domain;

namespace BusinessLayer.MessageSenders
{
    public interface ISMSSender
    {
        public PhoneNumber PhoneNumberOfSender { get; }
        public void Send(PhoneNumber phoneNumber, string message);
    }
}
