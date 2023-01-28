using maplr_api.DTO;
using maplr_api.Utils;

namespace maplr_api.Interfaces
{
    public interface IProductRepository
    {
        Task<IQueryable<CatalogueItemDto>> Get(Enums.Type type);

        Task<MapleSyrupDto?> GetByKey(string key);
    }
}
