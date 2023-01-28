using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
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
            _mapper = InitializeAutomapper();
        }

        private static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Orders, OrderLineDto>();

                cfg.CreateMap<OrderLineDto, Orders>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        private Orders OrderLineDtoToOrders(OrderLineDto orderLineDto)
        {
            return _mapper.Map<Orders>(orderLineDto);
        }

        private OrderLineDto OrdersToOrderLineDto(Orders orders)
        {
            return _mapper.Map<OrderLineDto>(orders);
        }

        public Task<OrderLineDto> Insert(OrderLineDto entity)
        {
            return Task.Run(() =>
            {
                _context.Add(OrderLineDtoToOrders(entity));
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
                    orders.Add(OrderLineDtoToOrders(orderDto));
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
                        responseList.Add(OrdersToOrderLineDto(order));
                    }
                    return responseList.AsQueryable();
                }
                return new List<OrderLineDto>().AsQueryable();
            });
        }
    }
}
