using System;

namespace SMS_Report.Models.Domains
{
    public class Error
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSent { get; set; }
    }
}
