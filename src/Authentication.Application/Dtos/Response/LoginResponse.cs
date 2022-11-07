using System.Text.Json.Serialization;

namespace Authentication.Application.Dtos.Response
{
    public class LoginResponse
    {
        public bool Success => Errors.Count == 0;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; private set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; private set; }

        public List<string> Errors { get; private set; }

        public LoginResponse() =>
            Errors = new List<string>();

        public LoginResponse(string accessToken, string refreshToken) : this()
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public void AddError(string erro) =>
            Errors.Add(erro);

        public void AddErrors(IEnumerable<string> erros) =>
            Errors.AddRange(erros);
    }
}
