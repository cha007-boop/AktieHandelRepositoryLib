using AktieHandelRepositoryLib;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class TestAktieHandel
    {
        [TestMethod]
        [DataRow("TEST1", 1, 12.0)]
        [DataRow("TEST2", -1, 15.0)]
        public void AktieHandel_SuccesfulCreation_NoException(string name, int amount, double exchangePrice)
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel(name, amount, exchangePrice);

            // Act

            // Assert
            Assert.AreEqual(name + amount + exchangePrice, aktieHandel.Name + aktieHandel.Amount + aktieHandel.ExchangePrice);
        }

        [TestMethod]
        public void AktieHandel_NameNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AktieHandel(null, 1, 10.0));
        }

        [TestMethod]
        public void AktieHandel_AttemptToSetNameToNull_ThrowsArgumentNullException()
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => aktieHandel.Name = null);
        }

        [TestMethod]
        [DataRow("t")]
        [DataRow("ttt")]
        public void AktieHandel_NameTooShort_ThrowsArgumentException(string name)
        {
            Assert.Throws<ArgumentException>(() => new AktieHandel(name, 1, 10.0));
        }

        [TestMethod]
        [DataRow("t")]
        [DataRow("ttt")]
        public void AktieHandel_AttemptToSetNameTooShort_ThrowsArgumentException(string name)
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => aktieHandel.Name = name);
        }

        [TestMethod]
        public void AktieHandel_SuccesfulNameChange_NoException()
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
            // Act
            aktieHandel.Name = "NEWNAME";
            // Assert
            Assert.AreEqual("NEWNAME", aktieHandel.Name);
        }

        [TestMethod]
        public void AktieHandel_Amount0_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AktieHandel("TEST", 0, 10.0));
        }

        [TestMethod]
        public void AktieHandel_AttemptToSetAmount0_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => aktieHandel.Amount = 0);
        }

        [TestMethod]
        public void AktieHandel_SuccesfulAmountChange_NoException()
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
            // Act
            aktieHandel.Amount = 5;
            // Assert
            Assert.AreEqual(5, aktieHandel.Amount);
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(-100.0)]
        public void AktieHandel_ExchangePriceNegative_ThrowsArgumentOutOfRangeException(double exchangePrice)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AktieHandel("TEST", 1, exchangePrice));
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(-100.0)]
        public void AktieHandel_AttemptToSetExchangePriceNonPositive_ThrowsArgumentOutOfRangeException(double exchangePrice)
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => aktieHandel.ExchangePrice = exchangePrice);
        }

        [TestMethod]
        public void AktieHandel_SuccesfulExchangePriceChange_NoException()
        {
            // Arrange
            AktieHandel aktieHandel = new AktieHandel("TEST", 1, 10.0);
            // Act
            aktieHandel.ExchangePrice = 20.0;
            // Assert
            Assert.AreEqual(20.0, aktieHandel.ExchangePrice);
        }

        [TestMethod]
        public void AktieHandel_ConstructorWithId_SetsIdCorrectly()
        {
            // Arrange
            int expectedId = 42;
            string name = "TEST";
            int amount = 1;
            double exchangePrice = 10.0;
            // Act
            AktieHandel aktieHandel = new AktieHandel(expectedId, name, amount, exchangePrice);
            // Assert
            Assert.AreEqual(expectedId, aktieHandel.Id);
        }

    }
}
