using maplr_api.Interfaces;
using maplr_api.Models;
using maplr_api.Repository;
using maplr_api.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace maplr_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapleSyrupRepository _mapleSyrupRepository;

        public ProductsController(IMapleSyrupRepository mapleSyrupRepository)
        {
            _mapleSyrupRepository = mapleSyrupRepository;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<MapleSyrup>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] Enums.Type type)
        {
            var products = await _mapleSyrupRepository.Get();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(MapleSyrup), StatusCodes.Status200OK)]
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
    }
}
