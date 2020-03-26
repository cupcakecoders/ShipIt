using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Runtime.CompilerServices;
using ShipIt.Models.DataModels;
namespace ShipIt.Models.ApiModels
{
    public class OutboundOrderTrucksResponse
    {
        public IEnumerable<Truck> Trucks { get; set; }
        public double NumberOfTrucks { get; set; }
    }

    public class Truck
    {
        public List<Batch> Batches { get; set; }
        public double TruckTotalWeight => Batches.Sum(batch => batch.TotalWeight);
        public double RemainingWeight => Batches.Sum(batch => 2000 - batch.TotalWeight);
        
        public Truck(Batch batch)
        {
            Batches = new List<Batch>{batch};
        }
    }
    
    public class Batch
    {

        public string Gtin { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double ItemWeight { get; set; }
        public double TotalWeight => ItemWeight * Quantity;
        
    }
}
