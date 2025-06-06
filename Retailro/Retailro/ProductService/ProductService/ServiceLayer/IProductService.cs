﻿using ProductService.Model;
using ProductService.Model.Dtos;

namespace ProductService.ServiceLayer
{
    public interface IProductService
    {
        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        Task AddProduct(AddProductDto product, IFormFile image);
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of all the products.</returns>
        Task<List<Product>> GetAllProducts();
        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The product</returns>
        Task<ProductDto> GetProduct(Guid productId);
        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task DeleteProduct(Product product);
        /// <summary>
        /// Deletes the product by identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        Task DeleteProductById(Guid productId);
        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task UpdateProduct(Product product);
        /// <summary>
        /// Searches the products.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<List<ProductDto>> SearchProducts(string query, string category, string brand, decimal? minPrice, decimal? maxPrice, string sort);
        /// <summary>
        /// Gets the brands.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetBrands();
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetCategories();
        /// <summary>
        /// Gets the newest products.
        /// </summary>
        /// <param name="numberOfProducts">The number of products.</param>
        /// <returns></returns>
        Task<List<ProductDto>> GetNewest(int numberOfProducts = 4);
        /// <summary>
        /// Gets the recommended.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        Task<List<ProductDto>> GetRecommended(List<Guid> productIds);
    }
}
