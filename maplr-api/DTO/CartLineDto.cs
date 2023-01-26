using AutoMapper;
using AutoMapper.Configuration.Annotations;
using maplr_api.Models;

namespace maplr_api.DTO
{
    [AutoMap(typeof(Carts))]
    public class CartLineDto
    {
        [SourceMember("Id")]
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }

        public CartLineDto()
        {
        }
    }
}
