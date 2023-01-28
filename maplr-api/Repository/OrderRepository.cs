using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Mappers;
using maplr_api.Models;

namespace maplr_api.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MaplrContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(MaplrContext maplrContext)
        {
            _context = maplrContext;
            _mapper = MapFields.InitializeRepositoryAutomapper();
        }

        public Task<OrderLineDto> Insert(OrderLineDto entity)
        {
            return Task.Run(() =>
            {
                _context.Add(_mapper.Map<Orders>(entity));
                _context.SaveChanges();
                return entity;
            });
        }

        public Task InsertRange(List<OrderLineDto> entities)
        {
            return Task.Run(() =>
            {
                var orders = new List<Orders>();
                foreach (OrderLineDto orderDto in entities)
                {
                    orders.Add(_mapper.Map<Orders>(orderDto));
                }
                _context.AddRangeAsync(orders);
                _context.SaveChanges();
            });
            
        }

        public Task<IQueryable<OrderLineDto>> Get()
        {
            return Task.Run(() =>
            {
                var data = _context.Orders.AsQueryable();
                if (data.Any())
                {
                    var responseList = new List<OrderLineDto>();
                    foreach(Orders order in data)
                    {
                        responseList.Add(_mapper.Map<OrderLineDto>(order));
                    }
                    return responseList.AsQueryable();
                }
                return new List<OrderLineDto>().AsQueryable();
            });
        }
    }
}
