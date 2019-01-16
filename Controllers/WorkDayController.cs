using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WorkTracker.Contexts;
using WorkTracker.Models;

namespace WorkTracker.Controllers {
    [Route("api/[controller]")]
    public class WorkDayController : Controller {

        private WorkDayContext _db;

        public WorkDayController(WorkDayContext db) {
            _db = db;
        }

        [HttpGet]
        public async Task<WorkDay> GetDay(int? dayId) {
            if (dayId != null) {
                return _db.WorkDays.Where(d => d.CurrentState == WorkDay.State.STARTED).First();
            }
            return await _db.WorkDays.FindAsync(dayId.Value);
        }

        private async Task<WorkDay> GetOrAddWorkDayAsync(int? id) {
            WorkDay result;
            if (id.HasValue) {
                result = await _db.WorkDays.FindAsync(id.Value);
                if (result != null) {
                    return result;
                }
            }
            result = new WorkDay();
            await _db.AddAsync(result);
            await _db.SaveChangesAsync();
            return result;
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteDay(int? id) {
            if (!id.HasValue) {
                return Json(new {
                    error = "No ID specified"
                });
            }
            var toDelete = await _db.WorkDays.FindAsync(id.Value);
            _db.WorkDays.Remove(toDelete);
            await _db.SaveChangesAsync();
            return Json(new {
                error = ""
            });
        }

        [HttpPost]
        public async Task<WorkDay> PostDay(int? dayId, long? timeStamp) {
            Console.WriteLine($"Called Post with {dayId}");
            DateTimeOffset time;
            if (timeStamp.HasValue) {
                time = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp.Value);
            } else {
                time = DateTimeOffset.UtcNow;
            }
            WorkDay day = await GetOrAddWorkDayAsync(dayId);
            switch(day.CurrentState) {
                case WorkDay.State.INIT:
                    day.Start = time;
                    break;
                case WorkDay.State.STARTED:
                    day.End = time;
                    break;
                default:
                    break;
            }
            _db.Update(day);
            await _db.SaveChangesAsync();
            return day;
        }
    }
}
