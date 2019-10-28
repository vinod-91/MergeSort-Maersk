using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using TechnicalChallenge.MergeSort.Models;
using System.Threading.Tasks;
using TechnicalChallenge.MergeSort.Infrastructure;
using Microsoft.Extensions.Logging;

namespace TechnicalChallenge.MergeSort.Persistence
{
    public class MergeSortCaching: IMergeSortCaching
    {
        private IMemoryCache _cache;
        private Dictionary<int, Execution> _jobDictionary;
        private readonly ILogger<MergeSortCaching> _logger;
        public MergeSortCaching(IMemoryCache cache, ILogger<MergeSortCaching> logger)
        {
            _cache = cache;
            _logger = logger;
            _jobDictionary = new Dictionary<int, Execution>();
        }

        public async Task<ExecutionTracker> AddSortCache(Execution job)
        {
            return await Task.Run(() =>
            {
                if (_cache.TryGetValue("mergeSortCache", out _jobDictionary))
                {
                    _jobDictionary.Add(job.Id, job);
                }
                else
                {
                    _jobDictionary = new Dictionary<int, Execution>();
                    _jobDictionary.Add(job.Id, job);
                }
                _cache.Set("mergeSortCache", _jobDictionary, SetCachingStrategy());
                _logger.LogInformation($"New job with id:{job.Id} is added to the cache");
                return new ExecutionTracker { Id = job.Id, Status = job.Status };
            });
        }

        public async Task UpdateSortCache(Execution job)
        {
            await Task.Run(() => {
                if (_cache.TryGetValue("mergeSortCache", out _jobDictionary))
                {
                    _jobDictionary.Remove(job.Id);
                    _jobDictionary.Add(job.Id, job);
                }
                _logger.LogInformation($"Job with id:{job.Id} is Completed");
            });
        }

        public async Task<Executions> Get()
        {
            return await Task.Run(() =>
            {
                if (_cache.TryGetValue("mergeSortCache", out _jobDictionary))
                {
                    _logger.LogInformation("All the job list is fetched");
                    return new Executions { execution = _jobDictionary.Values.ToList() };
                }
                throw new JobNotFoundException();
            });
        }

        public async Task<Execution> GetById(int id)
        {
            return await Task.Run(() =>
            {
                if (_cache.TryGetValue("mergeSortCache", out _jobDictionary))
                {
                    _logger.LogInformation($"Job with id:{id} is fetched");
                    _jobDictionary.TryGetValue(id, out Execution execution);
                    return execution;
                }
                throw new JobNotFoundException(id);
            });
        }

        private MemoryCacheEntryOptions SetCachingStrategy()
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.SlidingExpiration = TimeSpan.FromMinutes(30);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            return cacheExpirationOptions;
        }
    }
}
