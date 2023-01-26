using maplr_api.DTO;
using maplr_api.Utils;

namespace maplr_api.Interfaces
{
    public interface ICartsRepository
    {
        Task<IQueryable<CartLineDto>> Get();

        Task<CartLineDto?> GetByKey(string key);

        Task<CartLineDto> Update(string key, CartLineDto entity);

        Task<string> Delete(string key);

        Task<CartLineDto> Patch(string key, CartLineDto entity);
    }
}
