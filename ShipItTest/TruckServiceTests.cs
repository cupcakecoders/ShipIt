using System.Collections.Generic;
using NUnit.Framework;
using ShipIt.Models.DataModels;
using ShipIt.Repositories;
using ShipIt.Services;
using FakeItEasy;
using ShipIt.Models.ApiModels;

namespace ShipItTest
{
    public class TruckServiceTests
    {
        private readonly ProductDataModel _testProd1 = new ProductDataModel
        {
            Id = TestId1,
            Gtin = "abc123",
            Name = "Test Item",
            Weight = 100
        };

        private const int TestId1 = 20;

        private TruckService _truckService;
        private IProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _productRepository = A.Fake<IProductRepository>();
            A.CallTo(() => _productRepository.GetProductById(TestId1)).Returns((_testProd1));

            _truckService = new TruckService(_productRepository);
        }

        [Test]
        public void OrderIsPlacedOnTruck()
        {
            var lineItems = new List<StockAlteration>
            {
                new StockAlteration(TestId1, 1)
            };

            var trucks = _truckService.GetBatches(lineItems);
            Assert.AreEqual(_testProd1.Weight, trucks[0].TotalWeight);
            
            /*var items = trucks[0].items.ToList();
            items.Should().HaveCount(1, "");
            items[0].Gtin.Should().Be("abcd1234", "");
            items[0].Name.Should().Be("Test Item", "");
            items[0].Quantity.Should().Be(1, "");
            items[0].WeightPerItem.Should().Be(100, "");
            items[0].TotalWeight.Should().Be(100, "");*/
            
        }
    }
}