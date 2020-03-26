using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls.WebParts;
using ShipIt.Exceptions;
using ShipIt.Models.ApiModels;
using ShipIt.Repositories;
using ShipIt.Services;

namespace ShipIt.Controllers
{
    public class OutboundOrderController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IStockRepository stockRepository;
        private readonly IProductRepository productRepository;

        public OutboundOrderController(IStockRepository stockRepository, IProductRepository productRepository)
        {
            this.stockRepository = stockRepository;
            this.productRepository = productRepository;
        }

        public OutboundOrderTrucksResponse Post([FromBody] OutboundOrderRequestModel request)
        {
            log.Info(String.Format("Processing outbound order: {0}", request));
            //creates a list of products from an order, throws an error on attempt to add same product twice.
            var gtins = new List<String>();
            foreach (var orderLine in request.OrderLines)
            {
                if (gtins.Contains(orderLine.gtin))
                {
                    throw new ValidationException(
                        String.Format("Outbound order request contains duplicate product gtin: {0}", orderLine.gtin));
                }

                gtins.Add(orderLine.gtin);
            }

            var productDataModels = productRepository.GetProductsByGtin(gtins);
            var products = productDataModels.ToDictionary(p => p.Gtin, p => new Product(p));


            //gets list of product_ids and quantity from order request and puts into a stock alteration list
            var lineItems = new List<StockAlteration>();
            //list of product ids from order request
            var productIds = new List<int>();
            var errors = new List<string>();

            foreach (var orderLine in request.OrderLines)
            {
                if (!products.ContainsKey(orderLine.gtin))
                {
                    errors.Add(string.Format("Unknown product gtin: {0}", orderLine.gtin));
                }
                else
                {
                    var product = products[orderLine.gtin];
                    lineItems.Add(new StockAlteration(product.Id, orderLine.quantity));
                    productIds.Add(product.Id);
                }
            }

            if (errors.Count > 0)
            {
                throw new NoSuchEntityException(string.Join("; ", errors));
            }

            //dictionary of all stock
            var stock = stockRepository.GetStockByWarehouseAndProductIds(request.WarehouseId, productIds);

            var orderLines = request.OrderLines.ToList();
            errors = new List<string>();
            //check if stock available
            for (int i = 0; i < lineItems.Count; i++)
            {
                var lineItem = lineItems[i];
                var orderLine = orderLines[i];

                if (!stock.ContainsKey(lineItem.ProductId))
                {
                    errors.Add(string.Format("Product: {0}, no stock held", orderLine.gtin));
                    continue;
                }

                var item = stock[lineItem.ProductId];
                if (lineItem.Quantity > item.held)
                {
                    errors.Add(
                        string.Format("Product: {0}, stock held: {1}, stock to remove: {2}", orderLine.gtin, item.held,
                            lineItem.Quantity));
                }
            }

            if (errors.Count > 0)
            {
                throw new InsufficientStockException(string.Join("; ", errors));
            }

            stockRepository.RemoveStock(request.WarehouseId, lineItems);

            TruckService newTrucks = new TruckService(productRepository);
            var trucks = newTrucks.GetTrucks(lineItems);
            var response = new OutboundOrderTrucksResponse
            {
                Trucks = trucks,
                NumberOfTrucks = trucks.Count
            };
            return response;
        }
    }
}