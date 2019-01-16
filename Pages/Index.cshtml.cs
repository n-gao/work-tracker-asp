using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WorkTracker.Models;
using WorkTracker.Contexts;

namespace WorkTracker.Pages
{
    public class IndexModel : PageModel
    {
        public WorkDay CurrentWork { get; set; }

        public List<WorkDay> DoneWork { get; set; }

        private WorkDayContext _db;

        public object CurrentId
        {
            get
            {
                if (CurrentWork != null)
                    return CurrentWork.Id;
                return "null";
            }
        }

        public DateTimeOffset CurrentDate {
            get {
                if (CurrentWork != null)
                    return CurrentWork.Start.Value;
                return DateTimeOffset.UtcNow;
            }
        }

        public TimeSpan TotalTimeSpent {
            get {
                TimeSpan result = new TimeSpan();
                DoneWork.ForEach(w => result += w.TimeSpent);
                return result;
            }
        }

        public TimeSpan TimeSpent(IEnumerable<WorkDay> work) {
            TimeSpan result = new TimeSpan();
            foreach (var item in work)
            {
                result += item.TimeSpent;
            }
            return result;
        }

        public IndexModel(WorkDayContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            CurrentWork = _db.WorkDays.Where(d => d.CurrentState == WorkDay.State.STARTED)
                .FirstOrDefault();
            var now = DateTime.UtcNow;
            now = new DateTime(now.Year, now.Month, 1, 0, 0, 0, 0);
            DoneWork = _db.WorkDays
                .Where(d => d.Start >= now)
                .Where(d => d.CurrentState == WorkDay.State.FINISHED)
                .ToList();
            foreach (var day in _db.WorkDays.ToList())
            {
                Console.WriteLine(day.CurrentState);
                Console.WriteLine(day.Start);
                Console.WriteLine(day.End);
            }
        }
    }
}
