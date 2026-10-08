using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AktieHandelRepositoryLib;

namespace UnitTests
{
    [TestClass]
    public class AktieHandelRepositoryDBTests
    {
        
        
        [TestMethod]
        public async Task GetAll_InvalidSortBy_ThrowsArgumentException()
        {
            // Arrange
            var repo = new AktieHandelRepositoryDB();

            // Act & Assert
            await AssertThrowsAsync<ArgumentException>(async () =>
            {
                await repo.GetAll(sortBy: "invalid_column");
            });
        }

        [TestMethod]
        public async Task Add_WhenDatabaseUnavailable_ThrowsException()
        {
            // Arrange
            var repo = new AktieHandelRepositoryDB();
            var aktie = new AktieHandel("TestName", 10, 5.0);

            // Act & Assert
            await AssertThrowsAsync<Exception>(async () =>
            {
                await repo.Add(aktie);
            });
        }

        [TestMethod]
        public async Task GetById_WhenDatabaseUnavailable_ThrowsException()
        {
            // Arrange
            var repo = new AktieHandelRepositoryDB();

            // Act & Assert
            await AssertThrowsAsync<Exception>(async () =>
            {
                await repo.GetById(1);
            });
        }

        [TestMethod]
        public async Task Update_WhenDatabaseUnavailable_ThrowsException()
        {
            // Arrange
            var repo = new AktieHandelRepositoryDB();
            var aktie = new AktieHandel("TestName", 10, 5.0);

            // Act & Assert
            await AssertThrowsAsync<Exception>(async () =>
            {
                await repo.Update(1, aktie);
            });
        }

        [TestMethod]
        public async Task Delete_WhenDatabaseUnavailable_ThrowsException()
        {
            // Arrange
            var repo = new AktieHandelRepositoryDB();

            // Act & Assert
            await AssertThrowsAsync<Exception>(async () =>
            {
                await repo.Delete(1);
            });
        }

        private static async Task AssertThrowsAsync<TException>(Func<Task> action) where TException : Exception
        {
            try
            {
                await action();
                Assert.Fail($"Expected exception of type {typeof(TException)} but no exception was thrown.");
            }
            catch (Exception ex)
            {
                if (ex is TException)
                {
                    // expected
                    return;
                }
                throw; // rethrow unexpected
            }
        }
    }
}
