using maplr_api.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace maplr_api.Models
{
    public class MapleSyrup
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; } = -1;

        [JsonPropertyName("stock")]
        public int Stock { get; set; } = -1;

        [JsonPropertyName("type")]
        public Enums.Type Type { get; set; }

        public MapleSyrup(string id, string name, string description, string image, double price, int stock, Enums.Type type)
        {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            Stock = stock;
            Type = type;
        }
    }
}
