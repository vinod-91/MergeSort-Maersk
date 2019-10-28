using System;
using System.Collections.Generic;

namespace TechnicalChallenge.MergeSort.Models
{
    public class Executions
    {
        public List<Execution> execution { get; set; }
    }

    public class Execution: ExecutionTracker
    {
        public int TimeStamp { get; set; }
        public long Duration { get; set; }
        public int[] Input { get; set; }
        public int[] Output { get; set; }
    }

    public class ExecutionTracker
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

    public enum JobStatus
    {
        Pending =0,
        Completed
    }
}
