using System;
using System.Diagnostics;
using TechnicalChallenge.MergeSort.Models;
using TechnicalChallenge.MergeSort.Persistence;

namespace TechnicalChallenge.MergeSort.Operation
{
    public class MergeSortOperation: IMergeSortOperation
    {
        private readonly IMergeSortCaching _mergeSortCaching;

        public MergeSortOperation(IMergeSortCaching mergeSortCaching)
        {
            _mergeSortCaching = mergeSortCaching;
        }

        public void MergeSort(int[] intArrays)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //To-Do: Implement MergeSort
            sw.Stop();
            Random rnd = new Random();
            
            int[] outputArray = intArrays;
            var execution = new Execution
            {
                Duration = sw.ElapsedMilliseconds,
                Id = rnd.Next(1000),
                Input = intArrays,
                Output = outputArray,
                Status = JobStatus.Completed,
                TimeStamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")
            };
            _mergeSortCaching.AddSortCache(execution);
        }

        public Executions Get()
        {
            return _mergeSortCaching.Get();
        }

        public Execution GetById(int id)
        {
            return _mergeSortCaching.GetById(id);
        }
    }
}
