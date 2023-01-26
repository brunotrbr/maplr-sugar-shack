using maplr_api.Utils;

namespace maplr_api.DTO
{
    public class CatalogueItemDto
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public int MaxQty { get; set; }
        public string Name { get; set; }      
        public double Price { get; set; }
        public Enums.Type Type { get; set; }

        public CatalogueItemDto(string id, string image, int maxQty, string name, double price, Enums.Type type)
        {
            Id = id;
            Image = image;
            MaxQty = maxQty;
            Name = name;
            Price = price;
            Type = type;
        }
    }
}
