
namespace Models.Domain
{
    public class Email
    {
        private string _email;
        public string CorrectEmail { get { return _email; } }

        private Email( string emailStr)
        {
            _email = emailStr;
        }

        public static Email TryCreate(string emailStr)
        {
            if(!IsValid(emailStr))
            {
                return null;
            }
            return new Email(emailStr);            
        }

        private static bool IsValid(string emailStr)
        {
            if(emailStr is null)
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailStr);
                return addr.Address == emailStr;
            }
            catch
            {
                return false;
            }
        }
    }
}
