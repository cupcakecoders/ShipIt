using System.Collections.Generic;
using System.Linq;
using ShipIt.Models.ApiModels;
using ShipIt.Models.DataModels;
using ShipIt.Repositories;

namespace ShipIt.Services
{
    public interface ITruckService
    {
        OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems);
    }

    public class TruckService : ITruckService
    {
        private OutboundOrderTrucksResponse _outboundOrderTrucksResponse;
        private readonly IProductRepository productRepository;

        public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            IEnumerable<Truck> getTrucks = new List<Truck>();
            
            foreach (var lineItem in lineItems)
            {
                var product = productRepository.GetProductById(lineItem.ProductId);
                var batch = new Batch
                {
                    Gtin = product.Gtin,
                    Quantity = lineItem.Quantity,
                    ItemWeight = product.Weight,
                    ProductName = product.Name
                };
            }
            return null;
        }

        public List<Truck> GetTrucks(List<Batch> batchedItems)
        {
            List<Truck> allTrucks = new List<Truck>();

            foreach (var batch in batchedItems)
            {
                if (allTrucks.Count < 1)
                {
                    CreateTruck();
                }
            }

            return allTrucks;
        }

        public Truck CreateTruck()
        {
            Truck newTruck = new Truck();
            return newTruck;
        }
        
        /*public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            throw new System.NotImplementedException();
        }*/
    }
    
    
}