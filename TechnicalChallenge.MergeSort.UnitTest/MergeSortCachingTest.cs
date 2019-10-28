using System.Linq;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TechnicalChallenge.MergeSort.Persistence;
using Microsoft.Extensions.Caching.Memory;
using TechnicalChallenge.MergeSort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using TechnicalChallenge.MergeSort.Infrastructure;

namespace TechnicalChallenge.MergeSort.UnitTest
{
    public class MergeSortCachingTet
    {
        private readonly Mock<ILogger<MergeSortCaching>> mockLogger;
        private readonly Mock<IMemoryCache> mockCache;

        public MergeSortCachingTet()
        {
             mockLogger = new Mock<ILogger<MergeSortCaching>>();
             mockCache = new Mock<IMemoryCache>();
        }

        [Fact]
        public async Task AddSortCacheTest_Success()
        {
            var mockDictionaryValue = TestData.pendingExecution;
            object mockDictionary = new Dictionary<int, Execution>();
            mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out mockDictionary)).Returns(true);
            var cachEntry =new  Mock<ICacheEntry>();
            mockCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry.Object);

            var instance = new MergeSortCaching(mockCache.Object, mockLogger.Object);
            var result = await instance.AddSortCache(mockDictionaryValue);

            Assert.Equal(mockDictionaryValue.Id, result.Id);
            Assert.Equal(mockDictionaryValue.Status, result.Status);
        }

        [Fact]
        public async Task UpdateSortCache_Success()
        {
            var mockDictionaryValue = TestData.pendingExecution;
            Dictionary<int, Execution> mockDictionary = new Dictionary<int, Execution>();
            mockDictionary.Add(mockDictionaryValue.Id, mockDictionaryValue);
            object mockOutValue = mockDictionary;
            mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out mockOutValue)).Returns(true);
            var mockUpdateValue = TestData.completedExecution;

            var instance = new MergeSortCaching(mockCache.Object, mockLogger.Object);
            await instance.UpdateSortCache(mockUpdateValue);
        }

        [Fact]
        public async Task Get_Success()
        {
            var mockDictionaryValue = TestData.pendingExecution;
            Dictionary<int, Execution> mockDictionary = new Dictionary<int, Execution>();
            mockDictionary.Add(mockDictionaryValue.Id, mockDictionaryValue);
            object mockOutValue = mockDictionary;
            mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out mockOutValue)).Returns(true);

            var instance = new MergeSortCaching(mockCache.Object, mockLogger.Object);
            var result = await instance.Get();

            Assert.Equal(mockDictionary.Count, result.execution.Count);
            mockDictionary.Values.First().Should().BeEquivalentTo(result.execution.First());
        }

        [Fact]
        public async Task GetById_Success()
        {
            var mockDictionaryValue = TestData.pendingExecution;
            Dictionary<int, Execution> mockDictionary = new Dictionary<int, Execution>();
            mockDictionary.Add(mockDictionaryValue.Id, mockDictionaryValue);
            object mockOutValue = mockDictionary;
            mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out mockOutValue)).Returns(true);

            var instance = new MergeSortCaching(mockCache.Object, mockLogger.Object);
            var result = await instance.GetById(5);

            mockDictionaryValue.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void GetById_Failure()
        {
            var mockDictionaryValue = TestData.pendingExecution;
            Dictionary<int, Execution> mockDictionary = new Dictionary<int, Execution>();
            mockDictionary.Add(mockDictionaryValue.Id, mockDictionaryValue);
            object mockOutValue = mockDictionary;
            mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out mockOutValue)).Returns(false);

            var instance = new MergeSortCaching(mockCache.Object, mockLogger.Object);

            Assert.ThrowsAsync<JobNotFoundException>(() => instance.GetById(5));
        }

        [Fact]
        public void GetAll_Failure()
        {
            var mockDictionaryValue = TestData.pendingExecution;
            Dictionary<int, Execution> mockDictionary = new Dictionary<int, Execution>();
            mockDictionary.Add(mockDictionaryValue.Id, mockDictionaryValue);
            object mockOutValue = mockDictionary;
            mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out mockOutValue)).Returns(false);

            var instance = new MergeSortCaching(mockCache.Object, mockLogger.Object);

            Assert.ThrowsAsync<JobNotFoundException>(() => instance.Get());
        }
    }
}
