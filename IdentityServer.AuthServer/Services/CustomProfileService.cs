using IdentityServer.AuthServer.Repository;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;

        public CustomProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();
            var user = await _userRepository.FindById(int.Parse(subId));
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name",user.UserName),
                new Claim("city", user.City),
            };

            if (user.Id == 1)
            {
                claims.Add(new Claim("role", "admin")); //Stratupdaki roleClaim ile aynı ada sahip olmalı örn orada role5 yazıyor buradada role5 yazmalı.
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claims); //Claimleri vermek için kullandık. Claim olarak görülür jwtde görümez
            //context.IssuedClaims = claims; Jwt içinde görülür
        }

        public async Task IsActiveAsync(IsActiveContext context) // Kullanıcının id si var mı kullanıcı var mı ?
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userRepository.FindById(int.Parse(userId));

            context.IsActive=user!= null?true:false;
            
        }
    }
}
