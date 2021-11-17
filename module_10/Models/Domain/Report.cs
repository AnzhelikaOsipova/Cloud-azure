using System;

namespace Models.Domain
{
    [Serializable]
    public class Report
    {
        public string StudentFio { get; set; }
        public string LectionTopic { get; set; }
        public string Date { get; set; }
        public string Attendance { get; set; }
    }
}
