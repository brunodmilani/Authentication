﻿using Authentication.Application.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegistrarAsync(UserRequest usuario);
        Task<SignInResult> EntrarAsync(LoginRequest usuario);
    }
}
