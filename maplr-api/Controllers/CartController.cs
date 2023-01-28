using AutoMapper;
using maplr_api.Context;
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
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _mapleSyrupRepository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository, IProductRepository mapleSyrupRepository)
        {
            _cartRepository = cartRepository;
            _mapleSyrupRepository = mapleSyrupRepository;
            _mapper = InitializeAutomapper();
        }

        private static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MapleSyrupDto, CartLineDto>()
                  .ForSourceMember(src => src.Description, opt => opt.DoNotValidate())
                  .ForSourceMember(src => src.Type, opt => opt.DoNotValidate())
                  .AfterMap((src, dest) => { dest.ProductId = src.Id; dest.Qty = src.Stock > 0 ? 1 : 0; });
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        private CartLineDto MapleSyrupDtoToCartLineDto(MapleSyrupDto mapleSyrupDto)
        {
            return _mapper.Map<CartLineDto>(mapleSyrupDto);
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

            var mappedDto = MapleSyrupDtoToCartLineDto(mapleSyrupDto);
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
