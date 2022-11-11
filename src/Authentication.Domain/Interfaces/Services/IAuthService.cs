using Authentication.Shared.Dtos.Request;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Shared.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegistrarAsync(CreateUserRequest usuario);
        Task<SignInResult> EntrarAsync(LoginRequest usuario);
    }
}
