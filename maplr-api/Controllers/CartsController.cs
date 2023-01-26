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
    public class CartsController : ControllerBase
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IMapleSyrupRepository _mapleSyrupRepository;
        public readonly IMapper _mapper;

        public CartsController(ICartsRepository cartsRepository, IMapleSyrupRepository mapleSyrupRepository)
        {
            _cartsRepository = cartsRepository;
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
                  .ForMember(dest => dest.Price, opt => opt.Condition( src => src.Stock > 0) );
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put([FromQuery] string productId)
        {
            var cartDto = await _cartsRepository.GetByKey(productId);

            if (cartDto != null)
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
            _ = _cartsRepository.Update(mappedDto);

            return Accepted();
        }
    }
}
