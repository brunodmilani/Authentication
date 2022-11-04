using System.Text.Json.Serialization;

namespace Authentication.Shared.Base
{
    public class DtoBase
    {
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime DataInclusao { get; set; }
        [JsonIgnore]
        public DateTime? DataAlteracao { get; set; }
    }
}
