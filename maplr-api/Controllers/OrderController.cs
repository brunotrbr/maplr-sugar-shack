using maplr_api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace maplr_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // POST api/<OrderController>
        [HttpPost]
        public IEnumerable<MapleSyrup> Post([FromBody] string value)
        {
            return new MapleSyrup[] { };
        }

    }
}
