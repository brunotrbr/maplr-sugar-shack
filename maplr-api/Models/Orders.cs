using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace maplr_api.Models
{
    public class Orders
    {
        [Key]
        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonPropertyName("qty")]
        public int Qty { get; set; }

        public Orders()
        {
        }
    }
}
