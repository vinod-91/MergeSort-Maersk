using MergeSort;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using TechnicalChallenge.MergeSort.Models;
using TechnicalChallenge.MergeSort.Persistence;
using Microsoft.Extensions.Logging;
namespace TechnicalChallenge.MergeSort.Operation
{
    public class MergeSortOperation: IMergeSortOperation
    {
        private readonly IMergeSortCaching _mergeSortCaching;
        private readonly ILogger<MergeSortOperation> _logger;

        public MergeSortOperation(IMergeSortCaching mergeSortCaching, ILogger<MergeSortOperation> logger)
        {
            _mergeSortCaching = mergeSortCaching;
            _logger = logger;
        }

        public async Task<ExecutionTracker> MergeSort(int[] intArrays)
        {
            Random rnd = new Random();
            var execution = new Execution
            {
                Id = rnd.Next(1000),
                Input = intArrays,
                Status = JobStatus.Pending.ToString(),
                TimeStamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
            };
            await _mergeSortCaching.AddSortCache(execution);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            execution.Output = MergeSort<int>.Sort(intArrays.AsEnumerable<int>()).ToArray();
            sw.Stop();
            execution.Duration = sw.ElapsedMilliseconds;
            execution.Status = JobStatus.Completed.ToString();
            await Task.Run(() => _mergeSortCaching.UpdateSortCache(execution));
            _logger.LogInformation($"Job with Id:{execution.Id} has been sorted in {sw.ElapsedMilliseconds}ms");
            return new ExecutionTracker { Id = execution.Id, Status = execution.Status.ToString()};
        }

        public async Task<Executions> Get()
        {
            return await _mergeSortCaching.Get();
        }

        public async Task<Execution> GetById(int id)
        {
            return await _mergeSortCaching.GetById(id);
        }
    }
}
