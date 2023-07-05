using anothertour.Models;
using System.Globalization;

namespace anothertour.ViewModels
{
    public class ScheduleViewModel
    {
        public List<ScheduleItem> Items { get; set; }
        public List<DateTime> Days { get; set; }
    }
}
