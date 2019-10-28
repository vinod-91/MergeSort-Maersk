using System;

namespace TechnicalChallenge.MergeSort.Infrastructure
{
    public class JobNotFoundException: Exception
    {
        public JobNotFoundException(): base("No Job Exist")
        {

        }

        public JobNotFoundException(int jobId)
            : base(String.Format("The Job with ID: {0} cannot be found", jobId))
        {

        }
    }
}
