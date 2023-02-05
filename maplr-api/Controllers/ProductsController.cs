using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace maplr_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _mapleSyrupRepository;

        public ProductsController(IProductRepository mapleSyrupRepository)
        {
            _mapleSyrupRepository = mapleSyrupRepository;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<CatalogueItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] Enums.Type type)
        {
            var products = await _mapleSyrupRepository.Get(type);
            return Ok(products);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(MapleSyrupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string productId)
        {
            var product = await _mapleSyrupRepository.GetByKey(productId);
            if(product == null)
            {
                return NotFound("Id not found");
            }
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            throw new Exception("Just a test");
        }
    }
}
