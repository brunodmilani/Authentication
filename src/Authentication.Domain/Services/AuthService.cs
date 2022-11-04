﻿using Authentication.Domain.Dto;
using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.Services;
using Authentication.Shared.Dto;
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

        public async Task<SignInResult> EntrarAsync(LoginRequest usuario)
        {
            var user = new Usuario
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                EmailConfirmed = true
            };

            return await _signInManager.PasswordSignInAsync(usuario.Email, usuario.ConfirmPassword, false, true);
        }
    }
}
