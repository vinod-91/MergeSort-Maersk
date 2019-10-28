using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechnicalChallenge.MergeSort.Models;

namespace TechnicalChallenge.MergeSort.Operation
{
    public interface IMergeSortOperation
    {
        Task<ExecutionTracker> MergeSort(int[] intArrays);

        Task<Executions> Get();

        Task<Execution> GetById(int id);
    }
}
