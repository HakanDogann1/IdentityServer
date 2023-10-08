using IdentityServer.AuthServer.Models;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Repository
{
    public interface IUserRepository
    {
        Task<bool> Validation(string email,string password);
        Task<CustomUser> FindById(int id);
        Task<CustomUser> FindByEmail(string email);

    }
}
