using Authentication.Application.Dtos.Request;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegistrarAsync(CreateUserRequest usuario);
        Task<SignInResult> EntrarAsync(LoginRequest usuario);
    }
}
