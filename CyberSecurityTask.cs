using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberBotPart3
{
    class CyberSecurityTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Reminder { get; set; }
        public bool IsComplete { get; set; }

        public void Deconstruct(out string title, out string description, out DateTime? reminder)
        {
            title = Title;
            description = Description;
            reminder = Reminder;
        }

        public override string ToString() =>
            $"{Title}|{Description}|{Reminder?.ToString() ?? "null"}|{IsComplete}";
    }
}
