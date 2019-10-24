using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechnicalChallenge.MergeSort.Models;
using TechnicalChallenge.MergeSort.Operation;

namespace TechnicalChallenge.MergeSort.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MergeSortController : ControllerBase
    {
        private readonly IMergeSortOperation _mergeSortOperation;
        public MergeSortController(IMergeSortOperation mergeSortOperation)
        {
            _mergeSortOperation = mergeSortOperation;
        }
        [HttpPost]
        public void Post([FromBody] int[] input)
        {
            _mergeSortOperation.MergeSort(input);
        }
        [ActionName("GetAllExecution")]
        [HttpGet]
        public Executions Get()
        {
            return _mergeSortOperation.Get();
        }
        [ActionName("GetExecutionById")]
        [HttpGet]
        public Execution GetById(int id)
        {
            return _mergeSortOperation.GetById(id);
        }
    }
}
