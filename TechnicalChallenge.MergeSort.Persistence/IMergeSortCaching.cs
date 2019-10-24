using System;
using System.Collections.Generic;
using System.Text;
using TechnicalChallenge.MergeSort.Models;

namespace TechnicalChallenge.MergeSort.Persistence
{
    public interface IMergeSortCaching
    {
        void AddSortCache(Execution job);

        Executions Get();

        Execution GetById(int id);
    }
}
