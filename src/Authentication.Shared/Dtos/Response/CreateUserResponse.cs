namespace Authentication.Shared.Dtos.Response
{
    public class CreateUserResponse
    {
        public bool Success { get; private set; }
        public List<string> Errors { get; private set; }

        public CreateUserResponse() =>
            Errors = new List<string>();

        public CreateUserResponse(bool success = true) : this() =>
            Success = success;

        public void AddErrors(IEnumerable<string> erros) =>
            Errors.AddRange(erros);
    }
}
