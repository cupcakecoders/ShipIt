using System.Collections.Generic;
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

            foreach (var item in lineItems)
            {
                
            }
            
            return null;
        }
        /*public OutboundOrderTrucksResponse GetTrucks(List<StockAlteration> lineItems)
        {
            throw new System.NotImplementedException();
        }*/
    }
    
    
}