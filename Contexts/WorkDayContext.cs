using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

using WorkTracker.Models;

namespace WorkTracker.Contexts
{
    public class WorkDayContext : DbContext
    {
        public DbSet<WorkDay> WorkDays { get; set; }
        
        public WorkDayContext(DbContextOptions<WorkDayContext> options)
            :base (options) {
        }
    }
}
