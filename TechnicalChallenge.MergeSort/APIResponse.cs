using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalChallenge.MergeSort
{
    public class APIResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public object Payload { get; set; }
    }
}
