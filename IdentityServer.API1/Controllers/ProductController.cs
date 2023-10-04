using IdentityServer.API1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]

        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product{ProductId=1,Name="Kalem",Price=100,Stock=50},
                new Product{ProductId=2,Name="Kitap",Price=200,Stock=20},
                new Product{ProductId=3,Name="Defter",Price=150,Stock=25},
                new Product{ProductId=4,Name="Bant",Price=150,Stock=25},
                new Product{ProductId=5,Name="Defter",Price=150,Stock=25}
            };

            return Ok(productList);
        }

        [Authorize(Policy = "UpdateOrWrite")]
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"Id {id} olan ürün güncellenmiştir.");
        }
        [Authorize(Roles ="UpdateOrWrite")]
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }
    }
}
