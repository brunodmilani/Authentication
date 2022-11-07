using Authentication.Application.Dtos.Request;
using Authentication.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthService(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegistrarAsync(CreateUserRequest usuario)
        {
            var user = new IdentityUser
            { 
                UserName = usuario.Email,
                Email = usuario.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, usuario.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(user, false);
            return result;
        }

        public async Task<SignInResult> EntrarAsync(LoginRequest usuario)
        {
            return await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, false, true);
        }
    }
}
