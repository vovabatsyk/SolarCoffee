using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(SolarDbContext context, ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Gets all SalesOrders in the systems
        /// </summary>
        /// <returns>List<SalesOrder /></returns>
        public List<SalesOrder> GetOrders() =>
            _context.SalesOrders
                .Include(o => o.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                .Include(o => o.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();

        /// <summary>
        /// Creates an open SalesOrder
        /// </summary>
        /// <param name="order">salesOrder instance</param>
        /// <returns>ServiceResponse<bool /></returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            _logger.LogInformation("Generating new order");
            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);
                var inventoryId = _inventoryService.GetByProductId(item.Product.Id).Id;
                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            try
            {
                _context.SalesOrders.Add(order);
                _context.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Open order created"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace
                };
            }
        }


        /// <summary>
        /// Marks as open SalesOrder as paid
        /// </summary>
        /// <param name="id">sales order id</param>
        /// <returns>ServiceResponse<bool /></returns>
        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var now = DateTime.UtcNow;
            var order = _context.SalesOrders.Find(id);
            order.UpdatedOn = now;
            order.IsPaid = true;

            try
            {
                _context.SalesOrders.Update(order);
                _context.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Data = true,
                    IsSuccess = true,
                    Time = now,
                    Message = $"Order {order.Id} closed!"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Time = now,
                    Message = e.StackTrace
                };
            }
        }
    }
}
