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
        private readonly IMapleSyrupRepository _mapleSyrupRepository;
        public readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository, IMapleSyrupRepository mapleSyrupRepository)
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
            var carts = await _cartRepository.Get(productId);

            if (carts.Any())
            {
                var error = "Id already exists.";
                return Conflict(error);
            }

            var mapleSyrupDto = await _mapleSyrupRepository.GetByKey(productId);

            if (mapleSyrupDto == null)
            {
                var error = "Id not found.";
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
            var cartsDto = await _cartRepository.Get(productId);

            if (cartsDto.Any() == false)
            {
                var error = "Id not found.";
                return NotFound(error);
            }

            _ = await _cartRepository.Delete(cartsDto.First().ProductId);
            return Accepted();
        }
    }
}
