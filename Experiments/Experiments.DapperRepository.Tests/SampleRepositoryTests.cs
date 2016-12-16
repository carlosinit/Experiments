using Experiments.DapperRepository.Factories;
using Experiments.DapperRepository.Models;
using Experiments.DapperRepository.Tests.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Experiments.DapperRepository.Tests
{
    [TestClass]
    public class SampleRepositoryTests
    {
        [TestMethod]
        public void Get_List()
        {
            // Arrange
            var data = new[]
            {
                new Sample("Name1", "Description1"),
                new Sample("Name2", "Description2"),
                new Sample("Name3", "Description3")
            };

            var connection = new DatabaseBuilder()
                .WithSamplesTable(data)
                .Build();

            using (connection)
            {
                var connectionFactoryMock = new Mock<IDbConnectionFactory>();
                connectionFactoryMock.Setup(f => f.Create()).Returns(connection);

                // Act
                var sut = new SampleRepository(connectionFactoryMock.Object);
                var result = sut.Get().ToArray();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(data.Length, result.Length);
            }
        }

        [TestMethod]
        public void Get_ById()
        {
            // Arrange
            var data = new[]
            {
                new Sample("Name1", "Description1"),
                new Sample("Name2", "Description2"),
                new Sample("Name3", "Description3")
            };

            var connection = new DatabaseBuilder()
                .WithSamplesTable(data)
                .Build();

            using (connection)
            {
                var connectionFactoryMock = new Mock<IDbConnectionFactory>();
                connectionFactoryMock.Setup(f => f.Create()).Returns(connection);

                // Act
                var sut = new SampleRepository(connectionFactoryMock.Object);
                var result = sut.Get(2);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Id);
                Assert.AreEqual(data[1].Name, result.Name);
                Assert.AreEqual(data[1].Description, result.Description);
            }
        }
    }
}