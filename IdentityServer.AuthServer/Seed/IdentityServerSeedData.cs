using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace IdentityServer.AuthServer.Seed
{
    public static class IdentityServerSeedData
    {
        public static void Seed(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClient())  // Clientlar boş ise veritabanına kaydetmesi için oluşturuldu.
                {
                    context.Clients.Add(client.ToEntity()); //Clientları entitye çevirir.
                }
            }

            if(!context.ApiResources.Any())
            {
                foreach (var item in Config.GetApiResource())
                {
                    context.ApiResources.Add(item.ToEntity());
                }
            }

            if(!context.ApiScopes.Any())
            {
                foreach(var item in Config.GetApiScopes())
                {
                    context.ApiScopes.Add(item.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                Config.GetIdentityResources().ToList().ForEach(x =>
                {
                    context.IdentityResources.Add(x.ToEntity());
                });
            }

            context.SaveChanges();
        }
    }
}
