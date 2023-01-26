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

        public MapleSyrupDto()
        {
        }
    }
}
