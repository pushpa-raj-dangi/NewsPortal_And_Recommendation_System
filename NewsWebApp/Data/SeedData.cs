using Microsoft.Extensions.Logging;
using NewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsWebApp.Data
{
    public class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Posts.Any())
                {
                    var brandData = File.ReadAllText("E:\\project\\.NET_MVC_NEWS_PORTAL_SYSTEM_V_2\\Data\\post.json");
                    var brands = JsonSerializer.Deserialize<List<Post>>(brandData);

                    foreach (var item in brands)
                    {
                        context.Posts.Add(item);
                    }
                    await context.SaveChangesAsync();

                }

                //if (!context.ProductTypes.Any())
                //{
                //    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                //    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                //    foreach (var item in types)
                //    {
                //        context.ProductTypes.Add(item);
                //    }
                //    await context.SaveChangesAsync();

                //}

                //if (!context.Products.Any())
                //{
                //    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                //    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                //    foreach (var item in products)
                //    {
                //        context.Products.Add(item);
                //    }
                //    await context.SaveChangesAsync();

                //}

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<SeedData>();
                logger.LogError(ex.Message);
            }
        }
    }
}
