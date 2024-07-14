using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MovieApp.Models;
using System.Security.Claims;

namespace MovieApp.Manager
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserManager(IUserStore<ApplicationUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, 
            IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, 
            ILogger<UserManager<ApplicationUser>> logger)
         : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
         
        }

        public async Task<string> GetPreferredNameAsync(ClaimsPrincipal user)
        {
            var appUser = await this.GetUserAsync(user);
            return appUser?.PreferredName;
        }
    }

}