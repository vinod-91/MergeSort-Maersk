using System;
using System.Collections.Generic;
using System.Text;
using TechnicalChallenge.MergeSort.Models;

namespace TechnicalChallenge.MergeSort.UnitTest
{
    public static class TestData
    {
         public static Execution pendingExecution = new Execution { Id = 5, Status = JobStatus.Completed.ToString(), Duration = 10000, Input = new int[] { 23, 73, 78, 12 }, Output = new int[] { 23, 73, 78, 12 }, TimeStamp = 63434821 };
         public static Execution completedExecution = new Execution { Id = 5, Status = JobStatus.Completed.ToString(), Duration = 10000, Input = new int[] { 23, 73, 78, 12 }, Output = new int[] { 23, 73, 78, 12 }, TimeStamp = 63434821 };
    }
}
