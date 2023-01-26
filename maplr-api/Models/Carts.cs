using maplr_api.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace maplr_api.Models
{
    public class Carts
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; } = -1;

        [JsonPropertyName("qty")]
        public double Qty { get; set; } = -1;

        public Carts()
        {
            
        }
    }
}
