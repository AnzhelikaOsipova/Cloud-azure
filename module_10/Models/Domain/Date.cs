using System;
using System.Globalization;

namespace Models.Domain
{
    public class Date
    {
        private DateTime _correctDate;
        public DateTime CorrectDate { get { return _correctDate; } }

        private Date(DateTime date)
        {
            _correctDate = date;
        }

        public static Date TryCreate(string date)
        {
            if (DateTime.TryParseExact(date, "dd.MM.yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime correctDate))
            {
                return new Date(correctDate);
            }
            else
            {
                return null;
            }
        }
    }
}
