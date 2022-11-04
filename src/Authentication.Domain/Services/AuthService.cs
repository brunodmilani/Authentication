using Authentication.Domain.Dto;
using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public AuthService(
            SignInManager<Usuario> signInManager, 
            UserManager<Usuario> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegistrarAsync(UserRequest usuario)
        {
            var user = new Usuario
            { 
                UserName = usuario.UserName,
                Email = usuario.Email,
                EmailConfirmed = true
            };

            return await _userManager.CreateAsync(user, usuario.Password);
        }

        public async Task<Usuario> EntrarAsync(UserRequest usuario)
        {
            var user = new Usuario
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                EmailConfirmed = true
            };

            await _signInManager.SignInAsync(user, false);
            return user;
        }
    }
}
