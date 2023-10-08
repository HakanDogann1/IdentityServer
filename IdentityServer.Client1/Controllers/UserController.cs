using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        
        public IActionResult Index()
        {
            
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies"); //içine cookiew yazdık ki configde bunu oluşturmuştuk . Örneğin 2 adet login var ise bayi ve kullanıcı olarak hangisinden çıkış yapması gerektiğini belirttik.
            //await HttpContext.SignOutAsync("oidc"); //identity serverdan çıkığ yapması için kullandık
           return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.ClientId = "Client1-Mvc";
            refreshTokenRequest.ClientSecret = "secret";
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;
            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if(token.IsError)
            {
                //yönlendirme
                
            }
            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken },
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken },
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken },
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) },
            };
            var authenticationResult = await HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;
            properties.StoreTokens(tokens);
            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles ="admin")]
        public IActionResult AdminAction()
        {
            return View();
        }
        [Authorize(Roles = "customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}
