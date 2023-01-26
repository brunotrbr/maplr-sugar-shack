using maplr_api.DTO;
using maplr_api.Models;
using maplr_api.Utils;

namespace maplr_api.Interfaces
{
    public interface ICartRepository
    {
        Task<IQueryable<CartLineDto>> Get(string productId = "");

        Task<CartLineDto?> GetByKey(string key);

        Task<CartLineDto> Update(CartLineDto entity);

        Task<string> Delete(string key);

        Task<CartLineDto> Patch(CartLineDto entity);
    }
}
