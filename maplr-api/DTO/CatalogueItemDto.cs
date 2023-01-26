using maplr_api.Utils;
using AutoMapper;
using maplr_api.Models;
using AutoMapper.Configuration.Annotations;

namespace maplr_api.DTO
{
    public class CatalogueItemDto
    {
        public string Id { get; set; }
        
        public string Image { get; set; }

        public int MaxQty { get; set; }
        
        public string Name { get; set; }      
        
        public double Price { get; set; }
        
        public Enums.Type Type { get; set; } = 0;
        
        public CatalogueItemDto()
        {
            
        }
    }
}
