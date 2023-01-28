using AutoMapper;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace maplr_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _mapper = InitializeAutomapper();
            _productRepository = productRepository;
            _cartRepository = cartRepository;
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

        [HttpPost]
        [ProducesResponseType(typeof(OrderValidationResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] List<OrderLineDto> ordersDto)
        {
            var repOrdersDto = await _orderRepository.Get();
            var cartsDto = await _cartRepository.Get();
            var productsDto = await _productRepository.Get(0);
            List<string> errors = new List<string>();
            bool hasErrors = false;

            if (repOrdersDto.Any())
            {
                foreach (OrderLineDto repOrderDto in repOrdersDto)
                {
                    foreach (OrderLineDto orderDto in ordersDto)
                    {
                        if (repOrderDto.ProductId.Equals(orderDto.ProductId, StringComparison.InvariantCultureIgnoreCase))
                        {
                            errors.Add($"ProductId {orderDto.ProductId} already placed");
                        }
                    }
                }
                hasErrors = errors.Count > 0;
            }
            if (cartsDto.Any())
            {
                bool existInCart;
                foreach (OrderLineDto orderDto in ordersDto)
                {
                    existInCart = false;
                    foreach (CartLineDto cartDto in cartsDto)
                    {
                        if (cartDto.ProductId.Equals(orderDto.ProductId, StringComparison.InvariantCultureIgnoreCase))
                        {
                            existInCart = true;
                            break;
                        }
                    }
                    if (existInCart == false)
                    {
                        errors.Add($"ProductId {orderDto.ProductId} not in chart");
                    }
                }
                hasErrors = errors.Count > 0;
            } 
            else
            {
                foreach (OrderLineDto orderDto in ordersDto)
                {
                    errors.Add($"ProductId {orderDto.ProductId} not in chart");
                }
            }
            if (productsDto.Any())
            {
                bool existInCatalogue;
                foreach (OrderLineDto orderDto in ordersDto)
                {
                    existInCatalogue = false;
                    foreach (CatalogueItemDto catalogueDto in productsDto)
                    {
                        if (catalogueDto.Id.Equals(orderDto.ProductId, StringComparison.InvariantCultureIgnoreCase))
                        {
                            existInCatalogue = true;
                            if (orderDto.Qty > catalogueDto.MaxQty)
                            {
                                errors.Add($"ProductId {orderDto.ProductId} quantity is greather than quantity in stock");
                            }
                        }
                    }
                    if(existInCatalogue == false)
                    {
                        errors.Add($"ProductId {orderDto.ProductId} not registered in stock"); 
                    }
                }
                hasErrors = errors.Count > 0;
            }

            if (hasErrors)
            {
                return Ok(new OrderValidationResponseDto(false, errors.ToArray()));
            }

            await _orderRepository.InsertRange(ordersDto);
            return Ok(new OrderValidationResponseDto(true, new string[0]));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var repOrdersDto = await _orderRepository.Get();
            var ordersDto = await _orderRepository.Get();

            var items = (from x in repOrdersDto
                         join y in ordersDto on x.ProductId equals y.ProductId
                         select x).ToList();
            bool hasMatchAnys = repOrdersDto.Any(x => ordersDto.Any(y => y.ProductId == x.ProductId));

            bool hasMatchIntersects = repOrdersDto.Select(x => x.ProductId)
                                      .Intersect(ordersDto.Select(y => y.ProductId))
                                      .Any();
            return Ok();
        }
    }
}
