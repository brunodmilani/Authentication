using Authentication.Shared.Configurations;
using Authentication.Shared.Dtos.Request;
using Authentication.Shared.Dtos.Response;
using Authentication.Shared.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public AuthService(SignInManager<IdentityUser> signInManager, 
                           UserManager<IdentityUser> userManager,
                           IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<CreateUserResponse> RegistrarAsync(CreateUserRequest createUserRequest)
        {
            var identityUser = new IdentityUser
            {
                UserName = createUserRequest.UserName,
                Email = createUserRequest.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, createUserRequest.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(identityUser, false);

            var createUserResponse = new CreateUserResponse(result.Succeeded);
            if (!result.Succeeded && result.Errors.Count() > 0)
                createUserResponse.AddErrors(result.Errors.Select(r => r.Description));

            return createUserResponse;
        }

        public async Task<LoginResponse> EntrarAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginRequest.Password, false, true);

            if (result.Succeeded)
                return await GerarCredenciais(loginRequest.Email);

            var loginResponse = new LoginResponse();
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    loginResponse.AddError("Essa conta está bloqueada");
                else if (result.IsNotAllowed)
                    loginResponse.AddError("Essa conta não tem permissão para fazer login");
                else if (result.RequiresTwoFactor)
                    loginResponse.AddError("É necessário confirmar o login no seu segundo fator de autenticação");
                else
                    loginResponse.AddError("Usuário ou senha estão incorretos");
            }

            return loginResponse;
        }

        #region Métodos Privados

        private async Task<LoginResponse> GerarCredenciais(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
            var refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

            var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
            var dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

            var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
            var refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

            return new LoginResponse(accessToken: accessToken, refreshToken: refreshToken);
        }

        private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dataExpiracao,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> ObterClaims(IdentityUser user, bool adicionarClaimsUsuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString())
            };

            if (adicionarClaimsUsuario)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(userClaims);

                foreach (var role in roles)
                    claims.Add(new Claim("role", role));
            }

            return claims;
        }
        #endregion
    }
}
