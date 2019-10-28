using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechnicalChallenge.MergeSort.Models;

namespace TechnicalChallenge.MergeSort.Persistence
{
    public interface IMergeSortCaching
    {
        Task<ExecutionTracker> AddSortCache(Execution job);

        Task UpdateSortCache(Execution job);

        Task<Executions> Get();

        Task<Execution> GetById(int id);
    }
}
