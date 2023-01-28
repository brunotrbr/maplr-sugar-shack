using maplr_api.BusinessLayers;
using maplr_api.DTO;
using maplr_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace maplr_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrdersBL _ordersBL;

        public OrderController(IOrderRepository orderRepository, OrdersBL ordersBL)
        {
            _orderRepository = orderRepository;
            _ordersBL = ordersBL;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderValidationResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] List<OrderLineDto> ordersDto)
        {
            var errors = await _ordersBL.ValidateOrder(ordersDto);

            if (errors.Count > 0)
            {
                return Ok(new OrderValidationResponseDto(false, errors.ToArray()));
            }

            await _orderRepository.InsertRange(ordersDto);
            return Ok(new OrderValidationResponseDto(true, new string[0]));
        }
    }
}
