using System.Collections.Generic;
using NUnit.Framework;
using ShipIt.Models.ApiModels;

namespace ShipIt.Services
{
    public interface ITruckService
    {
        OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems);
    }

    public class TruckService : ITruckService
    {
        private OutboundOrderTrucksResponse _outboundOrderTrucksResponse;
        public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            IEnumerable<Truck> getTrucks = new List<Truck>();
            //create batch from lineitems
            //create truck from batch
            return null;
        }

        public List<Batch> GetBatches(List<StockAlteration> lineItems)
        {
            List<Batch> batchedUpItems = new List<Batch>();

            return batchedUpItems;
        }
        
        /*public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            throw new System.NotImplementedException();
        }*/
    }
    
    
}