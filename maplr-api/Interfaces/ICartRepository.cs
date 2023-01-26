using maplr_api.DTO;
using maplr_api.Utils;

namespace maplr_api.Interfaces
{
    public interface ICartRepository
    {
        Task<IQueryable<CartLineDto>> Get();

        Task<CartLineDto> Update(string key, CartLineDto entity);

        Task<Guid> Delete(string key);

        Task<CartLineDto> Patch(string key, CartLineDto entity);
    }
}
