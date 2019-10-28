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
        public async Task<ExecutionTracker> Post([FromBody] int[] input)
        {
            return await _mergeSortOperation.MergeSort(input);
        }
        [HttpGet]
        public async Task<Executions> Executions()
        {
            return await _mergeSortOperation.Get();
        }
        [HttpGet]
        public async Task<Execution> ExecutionsById(int id)
        {
            return await _mergeSortOperation.GetById(id);
        }
    }
}
