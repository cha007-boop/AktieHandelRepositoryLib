using AktieHandelRepositoryLib;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class TestAktieHandelRepository
{
    public AktieHandelRepository TheAktieHandelRepository { get; set; }

    [TestInitialize]
    public void TestInitialize()
    {
        TheAktieHandelRepository = new AktieHandelRepository();
    }

    [TestMethod]
    public void Add_ValidAktieHandel_GetAllCountIncreases()
    {
        // Arrange
        AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
        int initialCount = TheAktieHandelRepository.GetAll().Count;
        // Act
        TheAktieHandelRepository.Add(aktieHandel);
        int newCount = TheAktieHandelRepository.GetAll().Count;
        // Assert
        Assert.AreEqual(initialCount + 1, newCount);
    }

    [TestMethod]
    public void Add_ValidAktieHandel_Successful()
    {
        // Arrange
        AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
        // Act
        AktieHandel addedAktieHandel = TheAktieHandelRepository.Add(aktieHandel);
        // Assert
        Assert.IsNotNull(addedAktieHandel);
        Assert.AreEqual(aktieHandel.Name, addedAktieHandel.Name);
        Assert.AreEqual(aktieHandel.Amount, addedAktieHandel.Amount);
        Assert.AreEqual(aktieHandel.ExchangePrice, addedAktieHandel.ExchangePrice);
    }

    [TestMethod]
    public void Add_NullAktieHandel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TheAktieHandelRepository.Add(null));
    }

    [TestMethod]
    public void GetById_ValidId_ReturnsAktieHandel()
    {
        // Arrange
        AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
        AktieHandel addedAktieHandel = TheAktieHandelRepository.Add(aktieHandel);
        // Act
        AktieHandel? retrievedAktieHandel = TheAktieHandelRepository.GetById(addedAktieHandel.Id);
        // Assert
        Assert.IsNotNull(retrievedAktieHandel);
        Assert.AreEqual(addedAktieHandel.Id, retrievedAktieHandel?.Id);
        Assert.AreEqual(addedAktieHandel.Name, retrievedAktieHandel?.Name);
        Assert.AreEqual(addedAktieHandel.Amount, retrievedAktieHandel?.Amount);
        Assert.AreEqual(addedAktieHandel.ExchangePrice, retrievedAktieHandel?.ExchangePrice);
    }

    [TestMethod]
    public void GetById_InvalidId_ReturnsNull()
    {
        // Act
        AktieHandel? retrievedAktieHandel = TheAktieHandelRepository.GetById(-1);
        // Assert
        Assert.IsNull(retrievedAktieHandel);
    }

    [TestMethod]
    public void GetAll_ReturnsAllAddedAktieHandel()
    {
        // Arrange
        AktieHandel aktieHandel1 = new AktieHandel("TEST1", 1, 10.0);
        AktieHandel aktieHandel2 = new AktieHandel("TEST2", 2, 20.0);
        TheAktieHandelRepository.Add(aktieHandel1);
        TheAktieHandelRepository.Add(aktieHandel2);
        // Act
        List<AktieHandel> allAktieHandel = TheAktieHandelRepository.GetAll();
        // Assert
        Assert.AreEqual(2, allAktieHandel.Count);
        Assert.IsTrue(allAktieHandel.Any(a => a.Name == "TEST1"));
        Assert.IsTrue(allAktieHandel.Any(a => a.Name == "TEST2"));
    }

    [TestMethod]
    [DataRow(10.0, null, 3)]
    [DataRow(20.0, "TEST", 1)]
    [DataRow(5.0, "TEST", 2)]
    [DataRow(30.0, "TEST2", 1)]
    [DataRow(100.0, null, 0)]
    public void Get_ReturnsAllAddedAktieHandel(double exchangePrice, string? name, int expectedCount)
    {
        // Arrange 
        List<AktieHandel> aktieHandelList = new List<AktieHandel> {
            new AktieHandel("TEST", 1, 10.0),
            new AktieHandel("TEST", 2, 20.0),
            new AktieHandel("TEST2", 3, 30.0)
        };
        foreach (var aktieHandel in aktieHandelList)
        {
            TheAktieHandelRepository.Add(aktieHandel);
        }
        // Act
        List<AktieHandel> filteredAktieHandel = TheAktieHandelRepository.Get(exchangePrice, name);
        // Assert
        Assert.AreEqual(expectedCount, filteredAktieHandel.Count);
    }

    [TestMethod]
    public void Delete_ValidId_RemovesAktieHandel()
    {
        // Arrange
        AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
        AktieHandel addedAktieHandel = TheAktieHandelRepository.Add(aktieHandel);
        int initialCount = TheAktieHandelRepository.GetAll().Count;
        // Act
        bool deleteResult = TheAktieHandelRepository.Delete(addedAktieHandel.Id) != null;
        int newCount = TheAktieHandelRepository.GetAll().Count;
        // Assert
        Assert.IsTrue(deleteResult);
        Assert.AreEqual(initialCount - 1, newCount);
    }
    [TestMethod]
    public void Delete_InvalidId_ReturnsNull()
    {
        // Act
        AktieHandel? deletedAktieHandel = TheAktieHandelRepository.Delete(-1);
        // Assert
        Assert.IsNull(deletedAktieHandel);
    }

    [TestMethod]
    public void Update_ValidId_UpdatesAktieHandel()
    {
        // Arrange
        AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
        AktieHandel addedAktieHandel = TheAktieHandelRepository.Add(aktieHandel);
        AktieHandel updatedAktieHandel = new AktieHandel("UPDATED", 2, 20.0);
        // Act
        AktieHandel? result = TheAktieHandelRepository.Update(addedAktieHandel.Id, updatedAktieHandel);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(addedAktieHandel.Id, result?.Id);
        Assert.AreEqual("UPDATED", result?.Name);
        Assert.AreEqual(2, result?.Amount);
        Assert.AreEqual(20.0, result?.ExchangePrice);
    }
    [TestMethod]
    public void Update_InvalidId_ReturnsNull()
    {
        // Arrange
        AktieHandel updatedAktieHandel = new AktieHandel("UPDATED", 2, 20.0);
        // Act
        AktieHandel? result = TheAktieHandelRepository.Update(-1, updatedAktieHandel);
        // Assert
        Assert.IsNull(result);
    }

}
