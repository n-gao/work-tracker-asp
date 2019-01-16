using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WorkTracker.Models
{
    public class WorkDay
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }

        public enum State
        {
            INIT, STARTED, FINISHED
        }

        [JsonIgnore]
        public State CurrentState
        {
            get
            {
                if (!Start.HasValue) return State.INIT;
                if (!End.HasValue) return State.STARTED;
                return State.FINISHED;
            }
        }

        [JsonIgnore]
        public TimeSpan TimeSpent {
            get {
                return this.End.Value - this.Start.Value;
            }
        }
    }
}
