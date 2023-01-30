using maplr_api.DTO;

namespace maplr_api.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderLineDto?> Insert(OrderLineDto entity);
        Task InsertRange(List<OrderLineDto> entities);
        Task<string> Delete(string key);
        Task<IQueryable<OrderLineDto>> Get();
    }
}
