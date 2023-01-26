using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;
using maplr_api.Repository;
using maplr_api.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace maplr_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartsRepository _cartsRepository;

        public CartsController(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CartLineDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var carts = await _cartsRepository.Get();
            return Ok(carts);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var cartDto = await _cartsRepository.GetByKey(id);

            if (cartDto == null)
            {
                var error = "Id inexistente.";
                return NotFound(error);
            }

            _ = await _cartsRepository.Delete(id);
            return Accepted();
        }
    }
}
