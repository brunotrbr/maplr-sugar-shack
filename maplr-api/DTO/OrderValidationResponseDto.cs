using System.Text.Json.Serialization;

namespace maplr_api.DTO
{
    public class OrderValidationResponseDto
    {
        [JsonPropertyName("isOrderValid")]
        public bool IsOrderValid { get; set; }

        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }

        public OrderValidationResponseDto(bool isOrderValid, string[] errors)
        {
            IsOrderValid = isOrderValid;
            Errors = errors;
        }
    }
}
