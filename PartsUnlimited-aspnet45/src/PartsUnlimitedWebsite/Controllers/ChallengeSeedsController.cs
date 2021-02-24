using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PartsUnlimited.Models;



namespace PartsUnlimited.Controllers
{
    public class ChallengeSeedsController : Controller
    {
        private readonly PartsUnlimitedContext db;

        public ChallengeSeedsController(PartsUnlimitedContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        private void SkewPricesAndInventory()
        {
            Random pRandom = new Random();


            //mobile specific - pricing and inventory incongruities
            var mProducts = db.Products.ToList();
            foreach (var p in mProducts)
            {
                var newPrice = p.Price / 100 * (pRandom.Next(100, 200));
                if (newPrice <= 0.00M) newPrice = 5.00M;
                if (newPrice >= 500.00M) newPrice = 500.00M;
                p.Inventory = pRandom.Next(1, 100);
                p.LeadTime = pRandom.Next(0, 14);
                p.Price = newPrice;
                p.SalePrice = newPrice * .85M;
            }
            db.SaveChanges();
        }

        public ActionResult SeedMobile()
        {
            SkewPricesAndInventory();
            return RedirectToAction("Index");
        }

        private int AddCategory(string name, string description, string imageURI)
        {
            var existing = db.Categories.SingleOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
            if (existing != null) return 0;

            db.Categories.Add(new Category { Name = name, Description = description, ImageUrl = imageURI });
            return db.SaveChanges();
        }

        private int AddCategoryProduct(string categoryName, Product p)
        {
            var existing = db.Products.SingleOrDefault(x => x.Title.ToLower().Equals(p.Title.ToLower()));
            if (existing != null) return 0;

            db.Products.Add(p);
            return db.SaveChanges();
        }

        public ActionResult SeedStore1()
        {
            SkewPricesAndInventory();
            AddCategory("Accessories", "Driving Accessories", "drivingaccessories0.jpg");
            var categoriesMap = db.Categories.ToDictionary(c => c.Name, c => c.CategoryId);

            AddCategoryProduct("Accessories", new Product()  
            { 
               SkuNumber = "GPS-0001",
                Created = DateTime.Now,
                Title = "GPS Tesla Upgrade",
                CategoryId = categoriesMap["Accessories"],
                Price = 149.99M,
                SalePrice = 119.99M,
                ProductArtUrl = "drivingaccessories_gps_1.jpg",
                ProductDetails = "{ \"Type\": \"GPS\", }",
                Description = "This is an upgrade to the default screen in a Tesla.  You will need to work with an official installer to install in your Tesla",
                Inventory = 25,
                LeadTime = 2,
                RecommendationId = 19
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "GPS-0002",
                Created = DateTime.Now,
                Title = "GPS Phone Mount 1",
                CategoryId = categoriesMap["Accessories"],
                Price = 19.99M,
                SalePrice = 17.99M,
                ProductArtUrl = "drivingaccessories_phonegpsmount_1.jpg",
                ProductDetails = "{ \"Type\": \"GPS\", }",
                Description = "Mount your GPS device with this.",
                Inventory = 5,
                LeadTime = 2,
                RecommendationId = 20
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "GPS-0003",
                Created = DateTime.Now,
                Title = "GPS Phone Mount 2",
                CategoryId = categoriesMap["Accessories"],
                Price = 29.99M,
                SalePrice = 27.99M,
                ProductArtUrl = "drivingaccessories_phonegpsmount_2.jpg",
                ProductDetails = "{ \"Type\": \"GPS\", \"Detail\": \"Mounting Device\"}",
                Description = "Mount your GPS device with this.",
                Inventory = 2,
                LeadTime = 1,
                RecommendationId = 21
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "GPS-0003",
                Created = DateTime.Now,
                Title = "GPS Phone Mount 3",
                CategoryId = categoriesMap["Accessories"],
                Price = 13.99M,
                SalePrice = 8.99M,
                ProductArtUrl = "drivingaccessories_phonegpsmount_3.jpg",
                ProductDetails = "{ \"Type\": \"GPS\", \"Detail\": \"Mounting Device\"}",
                Description = "Mount your GPS device with this.",
                Inventory = 17,
                LeadTime = 0,
                RecommendationId = 22
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "SG-0001",
                Created = DateTime.Now,
                Title = "Fancy Sunglasses",
                CategoryId = categoriesMap["Accessories"],
                Price = 43.99M,
                SalePrice = 38.99M,
                ProductArtUrl = "drivingaccessories_sunglasses_1.jpg",
                ProductDetails = "{ \"Type\": \"Vision\", \"Detail\": \"Shades\"}",
                Description = "Don't let the sun get you",
                Inventory = 3,
                LeadTime = 0,
                RecommendationId = 23
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "SG-0002",
                Created = DateTime.Now,
                Title = "Basic Sunglasses",
                CategoryId = categoriesMap["Accessories"],
                Price = 13.99M,
                SalePrice = 38.99M,
                ProductArtUrl = "drivingaccessories_sunglasses_2.jpg",
                ProductDetails = "{ \"Type\": \"Vision\", \"Detail\": \"Shades\"}",
                Description = "Don't let the sun get you",
                Inventory = 40,
                LeadTime = 0,
                RecommendationId = 24
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "SG-0003",
                Created = DateTime.Now,
                Title = "Super Fancy Sunglasses",
                CategoryId = categoriesMap["Accessories"],
                Price = 143.99M,
                SalePrice = 128.99M,
                ProductArtUrl = "drivingaccessories_sunglasses_3.jpg",
                ProductDetails = "{ \"Type\": \"Vision\", \"Detail\": \"Shades\"}",
                Description = "Don't let the sun get you",
                Inventory = 1,
                LeadTime = 0,
                RecommendationId = 25
            });
            AddCategoryProduct("Accessories", new Product()
            {
                SkuNumber = "SG-0004",
                Created = DateTime.Now,
                Title = "Multi-Purpose Sunglasses",
                CategoryId = categoriesMap["Accessories"],
                Price = 36.99M,
                SalePrice = 31.99M,
                ProductArtUrl = "drivingaccessories_sunglasses_4.jpg",
                ProductDetails = "{ \"Type\": \"Vision\", \"Detail\": \"Shades\"}",
                Description = "Don't let the sun get you",
                Inventory = 22,
                LeadTime = 0,
                RecommendationId = 26
            });

            return RedirectToAction("Index");
        }

        public ActionResult SeedStore2()
        {
            SkewPricesAndInventory();
            AddCategory("Fancy Rims", "Fancy rims for your buggy", "rims0.jpg");
            var categoriesMap = db.Categories.ToDictionary(c => c.Name, c => c.CategoryId);

            AddCategoryProduct("Fancy Rims", new Product()
            {
                SkuNumber = "RIM-0001",
                Created = DateTime.Now,
                Title = "Rim 2 Dimension 2",
                CategoryId = categoriesMap["Fancy Rims"],
                Price = 172.49M,
                SalePrice = 172.49M,
                ProductArtUrl = "rims1.jpg",
                ProductDetails = "{ \"Material\" : \"Steel\",  \"Design\" : \"Spoke\", \"Spokes\" : \"8\",  \"Number of Lugs\" : \"4\", \"Wheel Diameter\" : \"19 in.\", \"Color\" : \"Gray\", \"Finish\" : \"Standard\", \"Pre-Assembled\" : \"Yes\" } ",
                Description = "These are the rims you are looking for",
                Inventory = 1,
                LeadTime = 4,
                RecommendationId = 19
            });
            AddCategoryProduct("Fancy Rims", new Product()
            {
                SkuNumber = "RIM-0002",
                Created = DateTime.Now,
                Title = "Central 3 Plane Open",
                CategoryId = categoriesMap["Fancy Rims"],
                Price = 99.49M,
                SalePrice = 79.49M,
                ProductArtUrl = "rims2.jpg",
                ProductDetails = "{ \"Material\" : \"Steel\",  \"Design\" : \"Spoke\", \"Spokes\" : \"8\",  \"Number of Lugs\" : \"4\", \"Wheel Diameter\" : \"19 in.\", \"Color\" : \"Gray\", \"Finish\" : \"Standard\", \"Pre-Assembled\" : \"Yes\" } ",
                Description = "These are the rims you are looking for",
                Inventory = 1,
                LeadTime = 4,
                RecommendationId = 20
            });
            AddCategoryProduct("Fancy Rims", new Product()
            {
                SkuNumber = "RIM-0003",
                Created = DateTime.Now,
                Title = "Angry Zeus 3",
                CategoryId = categoriesMap["Fancy Rims"],
                Price = 172.49M,
                SalePrice = 172.49M,
                ProductArtUrl = "rims3.jpg",
                ProductDetails = "{ \"Material\" : \"Steel\",  \"Design\" : \"Spoke\", \"Spokes\" : \"8\",  \"Number of Lugs\" : \"4\", \"Wheel Diameter\" : \"19 in.\", \"Color\" : \"Gray\", \"Finish\" : \"Standard\", \"Pre-Assembled\" : \"Yes\" } ",
                Description = "These are the rims you are looking for",
                Inventory = 21,
                LeadTime = 2,
                RecommendationId = 21
            });
            AddCategoryProduct("Fancy Rims", new Product()
            {
                SkuNumber = "RIM-0004",
                Created = DateTime.Now,
                Title = "Rim 2 Dimension 2",
                CategoryId = categoriesMap["Fancy Rims"],
                Price = 172.49M,
                SalePrice = 172.49M,
                ProductArtUrl = "rims4.jpg",
                ProductDetails = "{ \"Material\" : \"Steel\",  \"Design\" : \"Spoke\", \"Spokes\" : \"8\",  \"Number of Lugs\" : \"4\", \"Wheel Diameter\" : \"19 in.\", \"Color\" : \"Gray\", \"Finish\" : \"Standard\", \"Pre-Assembled\" : \"Yes\" } ",
                Description = "These are the rims you are looking for",
                Inventory = 1,
                LeadTime = 4,
                RecommendationId = 22
            });
            AddCategoryProduct("Fancy Rims", new Product()
            {
                SkuNumber = "RIM-0005",
                Created = DateTime.Now,
                Title = "Heavy Kit 47",
                CategoryId = categoriesMap["Fancy Rims"],
                Price = 89.99M,
                SalePrice = 79.99M,
                ProductArtUrl = "rims5.jpg",
                ProductDetails = "{ \"Material\" : \"Steel\",  \"Design\" : \"Spoke\", \"Spokes\" : \"8\",  \"Number of Lugs\" : \"4\", \"Wheel Diameter\" : \"19 in.\", \"Color\" : \"Gray\", \"Finish\" : \"Standard\", \"Pre-Assembled\" : \"Yes\" } ",
                Description = "These are the rims you are looking for",
                Inventory = 52,
                LeadTime = 14,
                RecommendationId = 23
            });
            AddCategoryProduct("Fancy Rims", new Product()
            {
                SkuNumber = "RIM-0006",
                Created = DateTime.Now,
                Title = "Rim 4 Dimension 5",
                CategoryId = categoriesMap["Fancy Rims"],
                Price = 172.49M,
                SalePrice = 172.49M,
                ProductArtUrl = "rims6.jpg",
                ProductDetails = "{ \"Material\" : \"Steel\",  \"Design\" : \"Spoke\", \"Spokes\" : \"8\",  \"Number of Lugs\" : \"4\", \"Wheel Diameter\" : \"19 in.\", \"Color\" : \"Gray\", \"Finish\" : \"Standard\", \"Pre-Assembled\" : \"Yes\" } ",
                Description = "These are the rims you are looking for",
                Inventory = 15,
                LeadTime = 7,
                RecommendationId = 24
            });
            return RedirectToAction("Index");
        }
    }
}
