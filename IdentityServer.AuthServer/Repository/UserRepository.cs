using IdentityServer.AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CustomDbContext _customDbContext;

        public UserRepository(CustomDbContext customDbContext)
        {
            _customDbContext = customDbContext;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _customDbContext.CustomUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _customDbContext.CustomUsers.FindAsync(id);
        }

        public async Task<bool> Validation(string email, string password)
        {
            return await _customDbContext.CustomUsers.AnyAsync(x=>x.Email == email && x.Password == password);
        }
    }
}
