﻿using KingICT_akademija.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace KingICT_akademija.Services
{
    public class ProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<Product>> GetProductsAPI()
        {
            var response = await httpClient.GetAsync("https://dummyjson.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var products = JObject.Parse(json)["products"].ToArray();

            return products.Select(p => new Product
            {
                Id = p["id"]?.Value<int>() ?? 0,
                Title = p["title"]?.ToString() ?? "No title",
                Price = p["price"]?.Value<decimal>() ?? 0,
                ShortDescription = ShortenDescription(p["description"]?.ToString() ?? string.Empty),
                Image = p["thumbnail"]?.ToString() ?? string.Empty,
                Category = p["category"]?.ToString() ?? "No category"
            }).ToList();
        }

        public async Task<Product> GetProductByIdAPI(int id)
        {
            var response = await httpClient.GetStringAsync($"https://dummyjson.com/products/{id}");
            var product = JObject.Parse(response).ToObject<Product>();

            return product;
        }

        public async Task<List<Product>> GetFilterProductsAPI(string category, decimal? minPrice, decimal? maxPrice)
        {
            var response = await httpClient.GetAsync("https://dummyjson.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var products = JObject.Parse(json)["products"].ToArray();

            return products.Where(p =>
                (string.IsNullOrEmpty(category) || p["category"]?.ToString() == category) &&
                (!minPrice.HasValue || p["price"]?.Value<decimal>() >= minPrice) &&
                (!maxPrice.HasValue || p["price"]?.Value<decimal>() <= maxPrice))
                .Select(p => new Product
                {
                    Id = p["id"]?.Value<int>() ?? 0,
                    Title = p["title"]?.ToString() ?? "No title",
                    Price = p["price"]?.Value<decimal>() ?? 0,
                    ShortDescription = ShortenDescription(p["description"]?.ToString() ?? string.Empty),
                    Image = p["thumbnail"]?.ToString() ?? string.Empty,
                    Category = p["category"]?.ToString() ?? "No category"
                }).ToList();
        }

        public async Task<List<Product>> GetProductsQueryAPI(string query)
        {
            var response = await httpClient.GetStringAsync("https://dummyjson.com/products");
            var products = JObject.Parse(response)["products"].ToArray()
                .Select(p => new Product
                {
                    Id = p["id"]?.Value<int>() ?? 0,
                    Title = p["title"]?.ToString() ?? "No Title",
                    Price = p["price"]?.Value<decimal>() ?? 0,
                    ShortDescription = ShortenDescription(p["description"]?.ToString() ?? string.Empty),
                    Image = p["thumbnail"]?.ToString() ?? string.Empty,
                    Category = p["category"]?.ToString() ?? "No Category"
                })
                .Where(p => p.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return products;
        }


        private string ShortenDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return "No description available.";

            if (description.Length <= 100)
                return description;

            return description.Substring(0, 100);
        }


    }
}
