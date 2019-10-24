using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using TechnicalChallenge.MergeSort.Models;

namespace TechnicalChallenge.MergeSort.Persistence
{
    public class MergeSortCaching: IMergeSortCaching
    {
        private IMemoryCache _cache;
        private Dictionary<int, Execution> _jobDictionary; 
        public MergeSortCaching(IMemoryCache cache)
        {
            _cache = cache;
            _jobDictionary = new Dictionary<int, Execution>();
        }

        public void AddSortCache(Execution job)
        {
            if(_cache.TryGetValue("mergeSortCache", out _jobDictionary))
            {
                _jobDictionary.Add(job.Id, job);
            }
            else
            {
                _jobDictionary = new Dictionary<int, Execution>();
                _jobDictionary.Add(job.Id, job);
            }
            _cache.Set("mergeSortCache", _jobDictionary, SetCachingStrategy());
        }

        public Executions Get()
        {
            if (_cache.TryGetValue("mergeSortCache", out _jobDictionary))
            {
                return new Executions { execution = _jobDictionary.Values.ToList() };
            }
            return null;
        }

        public Execution GetById(int id)
        {
            if (_cache.TryGetValue("mergeSortCache", out _jobDictionary))
            {
                _jobDictionary.TryGetValue(id, out Execution execution);
                return execution;
            }
            return null;
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
