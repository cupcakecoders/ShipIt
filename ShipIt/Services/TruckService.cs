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

            return null;
        }

        public List<Batch> GetBatches(List<StockAlteration> lineItems)
        {
            List<Batch> batchList = new List<Batch>();
            
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
                batchList.Add(batch);
            }
            return batchList;
        }
        
        public List<Truck> GetTrucks(List<Batch> batchedItems)
        {
            List<Truck> allTrucks = new List<Truck>();
            
            foreach (var batch in batchedItems)
            {
                if (allTrucks.Count <= 0)
                {
                    CreateTruck(batch);
                } 
                if (allTrucks.Count >= 1)
                {
                    if (batch.TotalWeight <= allTrucks[0].RemainingWeight)
                    {
                        allTrucks[0].Batches.Add(batch);
                    }
                    else
                    {
                        loop again until a truck with remaingweight <= batch is reached
                    }
                }
                
            }

            return allTrucks;
        }

        public Truck CreateTruck(Batch batch)
        {
            var truck = new Truck();

            if (batch.TotalWeight <= truck.RemainingWeight)
            {
                truck.Batches.Add(batch);
            }
            return truck;
        }
        
        /*public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            throw new System.NotImplementedException();
        }*/
    }
    
    
}