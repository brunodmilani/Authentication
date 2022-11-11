using Authentication.Shared.Dtos.Request;
using Authentication.Shared.Dtos.Response;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Shared.Interfaces.Services
{
    public interface IAuthService
    {
        Task<CreateUserResponse> RegistrarAsync(CreateUserRequest usuario);
        Task<LoginResponse> EntrarAsync(LoginRequest usuario);
    }
}
