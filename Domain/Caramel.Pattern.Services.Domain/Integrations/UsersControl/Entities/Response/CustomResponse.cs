using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Response
{
    [ExcludeFromCodeCoverage]
    public class CustomResponse<T>
    {

        public CustomResponse(T data, StatusProcess status)
        {
            Status = status;
            Description = status.GetDescription();
            Data = data;
        }

        public CustomResponse() { }

        [JsonPropertyName("data")]
        public T Data { get; set; }
        [JsonPropertyName("status")]
        public StatusProcess Status { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
