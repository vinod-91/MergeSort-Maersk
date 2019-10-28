using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TechnicalChallenge.MergeSort.Persistence;
using TechnicalChallenge.MergeSort.Operation;
using System.Threading.Tasks;
using TechnicalChallenge.MergeSort.Models;
using TechnicalChallenge.MergeSort.Infrastructure;
using FluentAssertions;

namespace TechnicalChallenge.MergeSort.UnitTest
{
    public class MergeSortOperationTest
    {
        private readonly Mock<ILogger<MergeSortOperation>> mockLogger;
        private readonly Mock<IMergeSortCaching> mockCache;

        public MergeSortOperationTest()
        {
            mockLogger = new Mock<ILogger<MergeSortOperation>>();
            mockCache = new Mock<IMergeSortCaching>();
        }
        [Fact]
        public async Task MergeSortTest_Success()
        {
            var instance = new MergeSortOperation(mockCache.Object, mockLogger.Object);

            var result = await instance.MergeSort(new int[] { 34, 54, 78, 12 });

            Assert.True(result.Id > 0);
            Assert.Equal(JobStatus.Completed.ToString(), result.Status);
        }

        [Fact]
        public async Task GetAllTest_Success()
        {
            var mockValue = new Executions { execution = new List<Execution> { TestData.pendingExecution } };
            mockCache.Setup(x => x.Get()).ReturnsAsync(mockValue);

            var instance = new MergeSortOperation(mockCache.Object, mockLogger.Object);
            var result = await instance.Get();

            mockValue.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetByIdTest_Success()
        {
            var mockValue = TestData.completedExecution;
            mockCache.Setup(x => x.GetById(5)).ReturnsAsync(mockValue);

            var instance = new MergeSortOperation(mockCache.Object, mockLogger.Object);
            var result =await instance.GetById(5);

            mockValue.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void GetByIdTest_ThrowException()
        {
            mockCache.Setup(x => x.GetById(5)).ThrowsAsync(new JobNotFoundException(5));

            var instance = new MergeSortOperation(mockCache.Object, mockLogger.Object);

            Assert.ThrowsAsync<JobNotFoundException>(() => instance.GetById(5));
        }

        [Fact]
        public void GetAllTest_ThrowException()
        {
            mockCache.Setup(x => x.Get()).ThrowsAsync(new JobNotFoundException());

            var instance = new MergeSortOperation(mockCache.Object, mockLogger.Object);

            Assert.ThrowsAsync<JobNotFoundException>(() => instance.Get());
        }
    }
}
