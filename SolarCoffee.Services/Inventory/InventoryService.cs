using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _context;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext context, ILogger<InventoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Returns all current inventory from the database
        /// </summary>
        /// <returns>List<ProductInventory /></returns>
        public List<ProductInventory> GetCurrentInventory() =>
            _context.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();

        /// <summary>
        /// Updates number of unit available of the provided product id
        /// Adjusts QuantityOnHand by adjustment value
        /// </summary>
        /// <param name="id">productId</param>
        /// <param name="adjustment">number of units added / removed from inventory</param>
        /// <returns>ServiceResponse<ProductInventory /></returns>
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _context.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);
                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error create inventory snapshot");
                    _logger.LogError(e.StackTrace);
                }

                _context.SaveChanges();
                return new ServiceResponse<ProductInventory>
                {
                    Data = inventory,
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = $"Product {id} inventory adjusted"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<ProductInventory>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = e.StackTrace
                };
            }
        }

        /// <summary>
        /// Gets a ProductInventory instance by Product id
        /// </summary>
        /// <param name="productId">int product primary key</param>
        /// <returns>ProductInventory</returns>
        public ProductInventory GetByProductId(int productId) =>
            _context.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Id == productId);

        /// <summary>
        /// Return snapshot history for the previous 6 hours
        /// </summary>
        /// <returns>List<ProductInventorySnapshot /></returns>
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
            return _context.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Create a snapshot record using the provided ProductInventory instance
        /// </summary>
        /// <param name="inventory">ProductInventory instance</param>
        private void CreateSnapshot(ProductInventory inventory)
        {
            var snapshot = new ProductInventorySnapshot
            {
                SnapshotTime = DateTime.UtcNow,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand,
                Id = inventory.Id
            };
            _context.Add(snapshot);
        }
    }
}
