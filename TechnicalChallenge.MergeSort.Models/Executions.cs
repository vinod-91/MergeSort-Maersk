using System;
using System.Collections.Generic;

namespace TechnicalChallenge.MergeSort.Models
{
    public class Executions
    {
        public List<Execution> execution { get; set; }
    }

    public class Execution
    {
        public int Id { get; set; }
        public string TimeStamp { get; set; }
        public long Duration { get; set; }
        public JobStatus Status { get; set; }
        public int[] Input { get; set; }
        public int[] Output { get; set; }
    }

    public enum JobStatus
    {
        Pending =0,
        Completed
    }
}
