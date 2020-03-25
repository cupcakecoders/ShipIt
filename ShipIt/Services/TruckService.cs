using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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

        public List<ProductDataModel> GetProductDetails(List<StockAlteration> lineItems)
        {
            var lineItemPid = lineItems.Select(item => item.ProductId);
            List<ProductDataModel> productsById = new List<ProductDataModel>();

            foreach (var pid in lineItemPid)
            {
                var productDataModelId = productRepository.GetProductById(pid);
                productsById.Add(productDataModelId);
            }
            return productsById;
        }

        public List<Batch> getBatch(List<ProductDataModel> productsById)
        {
            Batch newbatch = new Batch();

            List<Batch> batchedItems = new List<Batch>();

            foreach (var product in productsById)
            {
                newbatch.Gtin = product.Gtin;
                newbatch.ItemWeight = product.Weight;
                newbatch.ProductName = product.Name;
                
                batchedItems.Add(newbatch);
            }
            return batchedItems;
        }
        
        /*public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            throw new System.NotImplementedException();
        }*/
    }
    
    
}