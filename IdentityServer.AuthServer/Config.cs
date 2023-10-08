using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("resource_api1"){
                Scopes={ "api1.read", "api1.write", "api1.update" }
                },
                new ApiResource("resource_api2")
                {
                      Scopes={ "api2.read", "api2.write", "api2.update" }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read","Api1 için okuma izni"),
                new ApiScope("api1.write","Api 1 için yazma izni"),
                new ApiScope("api1.update","Api 1 için güncelleme izni"),

                 new ApiScope("api2.read","Api 2 için okuma izni"),
                new ApiScope("api2.write","Api 2 için yazma izni"),
                new ApiScope("api2.update","Api 2 için güncelleme izni"),
            };
        }

        public static IEnumerable<Client> GetClient()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="Client1",
                    ClientName="Client 1 web uygulaması",
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"api1.read"},

                },
                 new Client()
                {
                    ClientId="Client2",
                    ClientName="Client 2 web uygulaması",
                    ClientSecrets = new[]
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"api1.read", "api2.write","api2.update"},

                },
                 new Client()
                 {
                     ClientId="Client1-Mvc",
                     RequirePkce=false, //Server side uygulama olduğu için false set ettik
                     ClientName="Client1 app web uygulaması",
                     ClientSecrets = new[]
                     {
                         new Secret("secret".Sha256())
                     },
                     AllowedGrantTypes=GrantTypes.Hybrid,
                     RedirectUris=new List<string>{ "https://localhost:5006/signin-oidc" }, //Geri Dönüş hangi url e olacak
                     PostLogoutRedirectUris = new List<string>{ "https://localhost:5006/signout-callback-oidc" }, //Ezbere yazılmıyor. OpenId connect isimlendirilmeleridir.
                     AllowedScopes={IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                     AccessTokenLifetime=2*60*60, //Access tokena ömür verdik
                     AllowOfflineAccess=true, // Refresh tokeni aktif ettik
                     RefreshTokenUsage=TokenUsage.ReUse, //Bu refresh token bir kez mi kullanılsın birden fazla mı kullanılsın
                     RefreshTokenExpiration=TokenExpiration.Absolute, //kesin bir süre verdik
                     //AbsoluteRefreshTokenLifetime kesin bir gün verilir. Örneğin 3 gün sonra refresh tokenin ömrü bitsin gibi
                     //SlidingRefreshTokenLifetime Refresh tokena default olarak 15 gün içinde istek yaparsak refresh tokenin ömrünü 15 gün daha uzatıyor.
                     AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, //Refresh tokenin ömrü 60 gündür.
                     RequireConsent=true,

                 },
                 new Client()
                 {
                     ClientId="Client1-ResourceOwner-Mvc",
                     RequirePkce=false, //Server side uygulama olduğu için false set ettik
                     ClientName="Client1 app web uygulaması",
                     ClientSecrets = new[]
                     {
                         new Secret("secret".Sha256())
                     },
                     AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                     AllowedScopes={IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                     AccessTokenLifetime=2*60*60, //Access tokena ömür verdik
                     AllowOfflineAccess=true, // Refresh tokeni aktif ettik
                     RefreshTokenUsage=TokenUsage.ReUse, //Bu refresh token bir kez mi kullanılsın birden fazla mı kullanılsın
                     RefreshTokenExpiration=TokenExpiration.Absolute, //kesin bir süre verdik
                     //AbsoluteRefreshTokenLifetime kesin bir gün verilir. Örneğin 3 gün sonra refresh tokenin ömrü bitsin gibi
                     //SlidingRefreshTokenLifetime Refresh tokena default olarak 15 gün içinde istek yaparsak refresh tokenin ömrünü 15 gün daha uzatıyor.
                     AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, //Refresh tokenin ömrü 60 gündür.
                     

                 }

            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),  //İçinde token döndüğünde kullanıcının ıd si olmalıdır. Resource de olmazsa olmazdır.SubId
                new IdentityResources.Profile(),
                new IdentityResource()
                {
                    Name="CountryAndCity",
                    DisplayName="Country and City",
                    Description="Kullanıcının ülke ve şehir bilgisi",
                    UserClaims = new[]
                    {
                        "country",
                        "city"
                    },
                },
                new IdentityResource()
                {
                    Name="Roles",
                    DisplayName="Roles",
                    Description="Kullanıcı Rolleri",
                    UserClaims = new[]
                    {
                        "role"

                    }
                }
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser() {SubjectId="1",Username="dgnhakan1",Password="password",Claims=new List<Claim>(){
                new Claim("given_name","Hakan"),
                new Claim("family_name","Doğan"),
                new Claim("country","Türkiye"),
                new Claim("city","Karabük"),
                new Claim("role","admin"),
                } },
                 new TestUser() {SubjectId="2",Username="dgnhakan2",Password="password",Claims=new List<Claim>(){
                new Claim("given_name","Ahmet"),
                new Claim("family_name","Doğan"),
                new Claim("country","Türkiye"),
                new Claim("city","Ankara"),
                new Claim("role","customer"),

                } }
            };
        }


    }
}
