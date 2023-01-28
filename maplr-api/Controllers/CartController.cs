using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Mappers;
using maplr_api.Models;
using maplr_api.Repository;
using maplr_api.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace maplr_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _mapleSyrupRepository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository, IProductRepository mapleSyrupRepository)
        {
            _cartRepository = cartRepository;
            _mapleSyrupRepository = mapleSyrupRepository;
            _mapper = MapFields.InitializeControllerAutomapper();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CartLineDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var carts = await _cartRepository.Get();
            return Ok(carts);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put([FromQuery] string productId)
        {
            var cart = await _cartRepository.GetByKey(productId);

            if (cart != null)
            {
                var error = "Product already exists.";
                return Conflict(error);
            }

            var mapleSyrupDto = await _mapleSyrupRepository.GetByKey(productId);

            if (mapleSyrupDto == null)
            {
                var error = "Product not found.";
                return NotFound(error);
            }

            var mappedDto = _mapper.Map<CartLineDto>(mapleSyrupDto);
            _ = await _cartRepository.Update(mappedDto);

            return Accepted();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string productId)
        {
            var cart = await _cartRepository.GetByKey(productId);

            if (cart == null)
            {
                var error = "Product not found.";
                return NotFound(error);
            }

            _ = await _cartRepository.Delete(cart.ProductId);
            return Accepted();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch([FromQuery] string productId, int newQty)
        {
            var cart = await _cartRepository.GetByKey(productId);

            if (cart == null)
            {
                var error = "Product not found.";
                return NotFound(error);
            }

            var mapleSyrupDto = await _mapleSyrupRepository.GetByKey(productId);

            if (mapleSyrupDto == null)
            {
                var error = "Product not found.";
                return NotFound(error);
            }

            if(mapleSyrupDto.Stock < newQty)
            {
                var error = "Insufficiente products in stock.";
                return BadRequest(error);
            }

            cart.Qty = newQty;

            _ = await _cartRepository.Patch(cart);
            return Accepted();
        }
    }
}
