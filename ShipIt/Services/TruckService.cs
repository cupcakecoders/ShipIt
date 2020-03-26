using System;
using System.Collections.Generic;
using System.Linq;
using ShipIt.Models.ApiModels;
using ShipIt.Models.DataModels;
using ShipIt.Repositories;

namespace ShipIt.Services
{
    public interface ITruckService
    {
        List<Truck> GetTrucks(List<StockAlteration> lineItems);
    }

    public class TruckService : ITruckService
    {
        private readonly IProductRepository _productRepository;
        //line items go to batches which get put into trucks
        public TruckService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public List<Batch> GetBatches(List<StockAlteration> lineItems)
        {
            List<Batch> batchList = new List<Batch>();
            
            foreach (var lineItem in lineItems)
            {
                var product = _productRepository.GetProductById(lineItem.ProductId);
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
        
        public List<Truck> GetTrucks(List<StockAlteration> lineItems)
        {
            var batchedItems = GetBatches(lineItems);
            List<Truck> allTrucks = new List<Truck>();
            
            foreach (var batch in batchedItems)
            {
                if (allTrucks.Count == 0)
                {
                    var newTruck = CreateTruck(batch);
                    allTrucks.Add(newTruck);
                } 
                else
                {
                    if (batch.TotalWeight <= allTrucks[0].RemainingWeight)
                    {
                        allTrucks[0].Batches.Add(batch);
                    }
                    else //boolean to see if there are any trucks with capacity? If not then create new truck.
                    {
                        var truckWithCapacity = allTrucks.Find(truck => truck.RemainingWeight >= (batch.TotalWeight));
                        truckWithCapacity.Batches.Add(batch);
                    }
                }
            }
            return allTrucks;
        }

        public Truck CreateTruck(Batch batch)
        {
            var truck = new Truck(batch);

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