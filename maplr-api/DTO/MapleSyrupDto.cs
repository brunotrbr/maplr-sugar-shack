using maplr_api.Utils;

namespace maplr_api.DTO
{
    public class MapleSyrupDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public Enums.Type Type { get; set; }

        public MapleSyrupDto(string id, string name, string description, string image, double price, int stock, Enums.Type type)
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
