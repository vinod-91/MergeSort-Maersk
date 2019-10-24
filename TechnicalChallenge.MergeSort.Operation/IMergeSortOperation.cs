using System;
using System.Collections.Generic;
using System.Text;
using TechnicalChallenge.MergeSort.Models;

namespace TechnicalChallenge.MergeSort.Operation
{
    public interface IMergeSortOperation
    {
        void MergeSort(int[] intArrays);

        Executions Get();

        Execution GetById(int id);
    }
}
