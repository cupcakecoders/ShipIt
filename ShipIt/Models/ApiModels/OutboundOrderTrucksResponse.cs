using System.Collections.Generic;
using System.Linq;
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
        public int TruckId { get; set; }
        public List<Batch> Batches { get; set; }
        public double TruckTotalWeight => Batches.Sum(batch => batch.TotalWeight);
    }

    public class Batch
    {
        public string Gtin { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int ItemWeight { get; set; }
        public int TotalWeight => ItemWeight * Quantity;
    }
}
