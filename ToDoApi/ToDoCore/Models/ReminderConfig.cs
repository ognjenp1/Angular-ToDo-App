using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCore.Models
{
    public class ReminderConfig
    {
        public string SendGridKey { get; set; }
        public int Interval { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string TestUser { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTextContent { get; set; }
        public string HtmlOpenTag { get; set; }
        public string HtmlCloseTag { get; set; }
    }
}
