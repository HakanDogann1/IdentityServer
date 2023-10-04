using IdentityModel.Client;
using IdentityServer.Client1.Models;
using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _httpClient;

        public ProductsController(IConfiguration configuration, IApiResourceHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient _client =await _httpClient.GetHttpClient();
            var response = await _client.GetAsync("https://localhost:5016/api/product/getproducts");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductViewModel>>(content);
                return View(products);
            }
            else
            {
                //loglama
            }
            return View();
        }
    }
}
