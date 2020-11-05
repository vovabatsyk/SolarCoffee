using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService: IProductService
    {
        private readonly SolarDbContext _context;

        public ProductService(SolarDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Retrieves all product from the database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts() => _context.Products.ToList();
        
        /// <summary>
        /// Retrieves a product by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductById(int id) => _context.Products.Find(id);

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product">New product</param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _context.Products.Add(product);
                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };
                _context.ProductInventories.Add(newInventory);
                _context.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = "Saved new product"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = e.StackTrace
                };
            }
           
        }

        /// <summary>
        /// Archives a product by setting boolean IsArchived
        /// </summary>
        /// <param name="id">Product's primary key</param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                product.IsArchived = true;
                _context.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = "Archived product"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = e.StackTrace
                };
            }
        }
    }
}
