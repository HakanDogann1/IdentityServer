using IdentityModel;
using IdentityServer.AuthServer.Repository;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isUser = await _userRepository.Validation(context.UserName, context.Password); //sistemde kendiliğinden username aldığı için bize userName email verecek
            if (isUser)
            {
                var user = await _userRepository.FindByEmail(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(),OidcConstants.AuthenticationMethods.Password); //Buradaki password Resource Owner Credentials Grant anlamına gelir.

            }
        }
    }
}
