
using System.Text.RegularExpressions;

namespace Models.Domain
{
    public class PhoneNumber
    {
        private string _phoneNumber;
        public string CorrectPhoneNumber { get { return _phoneNumber; } }

        private PhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
        }

        public static PhoneNumber TryCreate(string phoneNumber)
        {
            if (!IsValid(phoneNumber))
            {
                return null;
            }
            return new PhoneNumber(phoneNumber);
        }

        private static bool IsValid(string phoneNumber)
        {
            if (phoneNumber is null)
            {
                return false;
            }
            Regex[] regs = new[]
            {
                new Regex(@"^\+\d\d{3}\d{3}\d{2}\d{2}$"),
                new Regex(@"^\d\d{3}\d{3}\d{2}\d{2}$")
            };
            foreach (Regex regex in regs)
            {
                if(regex.IsMatch(phoneNumber))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
